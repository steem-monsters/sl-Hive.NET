using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Utilities;
using SimpleBase;

namespace sl_Hive
{
    public class Memo
    {
        private readonly ref struct EncryptedMemoObject
        {
            public ReadOnlySpan<byte> From { get; init; }
            public ReadOnlySpan<byte> To { get; init; }
            public long Check { get; init; }
            public ReadOnlySpan<byte> Nonce { get; init; }
            public ReadOnlySpan<byte> Encrypted { get; init; }

            public ReadOnlySpan<byte> Pack() {
                if( From.Length == 0 || To.Length == 0 || Encrypted.Length == 0 ) throw new Exception("Invalid object");

                return Buffers.From(
                    From,
                    To,
                    Nonce,
                    // TODO: Only getting bytes is NOT architecture independent.
                    // This should be explicit network byte order.
                    BitConverter.GetBytes(Check).AsSpan()[..4],
                    EncodeVarInt32(Encrypted.Length),
                    Encrypted
                );
            }
        }

        public string MemoPrefix { get; } = "#";
        public string AddressPrefix { get; } = "STM";

        public Memo() { }

        public Memo(string memoPrefix, string addressPrefix) {
            MemoPrefix = memoPrefix;
            AddressPrefix = addressPrefix;
        }

        public string Decode(string memo, string privateKey)
        {
            if (string.IsNullOrEmpty(memo)) throw new ArgumentException("Invalid memo");
            if (string.IsNullOrEmpty(privateKey)) throw new ArgumentException("Invalid private key");

            return Decode(
                memo.StartsWith(MemoPrefix) ? memo[MemoPrefix.Length..] : memo,
                PrivateKey.From(privateKey)
            );
        }

        private string Decode(string memo, PrivateKey privateKey)
        {            
            var bytes = Base58.Bitcoin.Decode(memo);

            var x = bytes.ToArray();
            
            var from = bytes[..33];
            bytes = bytes.Slice(33);


            var fromKey = PublicKey.From(from);

            var to = bytes[..33];
            var toKey = PublicKey.From(to);
            bytes = bytes.Slice(33);

            var nonce = bytes[..8];
            var nonceValue = BitConverter.ToUInt64(nonce);
            bytes = bytes.Slice(8);

            var check = bytes[..4];
            var checkValue = BitConverter.ToUInt32(check);
            bytes = bytes.Slice(4);
            
            var messageLength = DecodeVarInt32(bytes[..1]);
            bytes = bytes.Slice(1);
            if (bytes.Length != messageLength) throw new Exception("Invalid encoded message");

            var pubKey = PublicKey.From(privateKey.GetPublicKey());
            var otherPub = pubKey.Key.SequenceEqual(fromKey.Key) ? toKey : fromKey;


            return Decrypt(bytes, privateKey, otherPub, checkValue, nonce);
        }

        private static string Decrypt(ReadOnlySpan<byte> message, PrivateKey privateKey, PublicKey publicKey, uint checksum, ReadOnlySpan<byte> nonce)
        {
            if (message.Length == 0) throw new Exception("Invalid message");
            Span<byte> encryptionKey = SHA512.HashData(Buffers.From(
               nonce,
               privateKey.GetSharedSecret(publicKey)
           ));
            var iv = encryptionKey.Slice(32, 16);
            var key = encryptionKey[..32];

            var checkValue = BitConverter.ToInt32(SHA256.HashData(encryptionKey), 0);
            if (checksum != checkValue) throw new Exception("Invalid checksum, unable to decrypt");

            return Decrypt(message, iv.ToArray(), key.ToArray());
        }

        public string Encode(string memo, string publicKey, string privateKey, ReadOnlySpan<byte> nonce = default) {
            if( string.IsNullOrEmpty(memo) ) throw new ArgumentException("Invalid memo");
            if( string.IsNullOrEmpty(publicKey) || !publicKey.StartsWith(AddressPrefix) ) throw new ArgumentException("Invalid public key");
            if( string.IsNullOrEmpty(privateKey) ) throw new ArgumentException("Invalid private key");

            return Encode(
                memo.StartsWith(MemoPrefix) ? memo[MemoPrefix.Length..] : memo,
                PublicKey.From(publicKey),
                PrivateKey.From(privateKey),
                nonce.Length == 0 ? UniqueNonce() : nonce
            );
        }

        private string Encode(string memo, PublicKey publicKey, PrivateKey privateKey, ReadOnlySpan<byte> nonce) {
            if( !publicKey.IsValid || !privateKey.IsValid ) throw new Exception("Unable to load public or private keys");


            var bytes = Encoding.UTF8.GetBytes(memo);

            var memoBuffer = Buffers.From(
                EncodeVarInt32(bytes.Length),
                bytes
            );

            Span<byte> encryptionKey = SHA512.HashData(Buffers.From(
                nonce,
                privateKey.GetSharedSecret(publicKey)
            ));
            var iv = encryptionKey.Slice(32, 16);
            var key = encryptionKey[..32];


            // TODO: Should this be int or maybe uint?
            // TODO: Explicit network byte order.
            
            var checkValue = BitConverter.ToInt32(SHA256.HashData(encryptionKey), 0);

            var encrypted = Encrypt(memoBuffer, iv.ToArray(), key.ToArray());

            var encryptedMemo = new EncryptedMemoObject {
                Check = checkValue,
                Nonce = nonce,
                Encrypted = encrypted,
                From = PublicKey.From(privateKey.GetPublicKey()).Key,
                To = publicKey.Key
            }.Pack();

            return $"{MemoPrefix}{Base58.Bitcoin.Encode(encryptedMemo)}";
        }

        private static Aes CreateCrypto(byte[] iv, byte[] key) {
            var result = Aes.Create();
            result.Mode = CipherMode.CBC;
            result.Key = key;
            result.IV = iv;
            return result;
        }

        private static ReadOnlySpan<byte> Encrypt(ReadOnlySpan<byte> buffer, byte[] iv, byte[] key) {
            using var crypto = CreateCrypto(iv, key);
            using var encryptor = crypto.CreateEncryptor();
            using var memory = new MemoryStream();
            using(var swEncrypt = new BinaryWriter(new CryptoStream(memory, encryptor, CryptoStreamMode.Write, false))) {
                swEncrypt.Write(buffer);
            }

            return memory.AsReadOnlySpan();
        }

        private static string Decrypt(ReadOnlySpan<byte> buffer, byte[] iv, byte[] key)
        {
            var text = "";
            using var crypto = CreateCrypto(iv, key);
            using var encryptor = crypto.CreateDecryptor();
            using var memory = new MemoryStream(buffer.ToArray());
            using (var swEncrypt = new BinaryReader(new CryptoStream(memory, encryptor, CryptoStreamMode.Read, false)))
            {
                swEncrypt.ReadByte();   // clear the stupid eos bit
                var bufferResult = swEncrypt.ReadBytes(buffer.Length);
                text = Encoding.UTF8.GetString(bufferResult);
            }
            return text;
        }


        private string Decrypt(byte[] buffer, byte[] iv, byte[] key) {
            using var crypto = CreateCrypto(iv, key);
            using var decryptor = crypto.CreateDecryptor();
            using var strStream = new StreamReader(new CryptoStream(new MemoryStream(buffer), decryptor, CryptoStreamMode.Read, false));
            return strStream.ReadToEnd();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CalculateVarInt32(uint value) => value switch {
            < 1 << 7 => 1,
            < 1 << 14 => 2,
            < 1 << 21 => 3,
            < 1 << 28 => 4,
            _ => 5
        };

        private static byte[] EncodeVarInt32(uint value) {
            // ref: https://github.com/protobufjs/bytebuffer.js/blob/master/src/types/varints/varint32.js

            var result = new byte[CalculateVarInt32(value)];
            var i = 0;
            while( value > 0x7Fu ) {
                result[i++] = (byte)(value | ~0x7Fu);
                value >>= 7;
            }

            result[i++] = (byte)value;

            return result;
        }

        private static int DecodeVarInt32(ReadOnlySpan<byte> buffer)
        {
            var i = 0;
            var c = 0;
            var b = 0;
            var value = 0;
            do
            {
                if (i > buffer.Length) throw new Exception("Unable to decode VarInt32");
                b = buffer[i++];
                if( c < 5)
                {
                    value |= (b & 0x7f) << (7 * c);
                }
                ++c;
            } while ((b & 0x80) != 0);
            value |= 0;
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] EncodeVarInt32(int value) => EncodeVarInt32(unchecked((uint)value));

        private static Span<byte> UniqueNonce() {
            Span<byte> result = new byte[32];
            RandomNumberGenerator.Fill(result);
            return result;
        }
    }
}
