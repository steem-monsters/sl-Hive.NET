using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using SimpleBase;

namespace sl_Hive
{
    public class Memo
    {
        private class EncryptedMemoObject
        {
            public byte[]? From { get; init; }
            public byte[]? To { get; init; }
            public long Check { get; init; }
            public ulong Nonce { get; init; }
            public byte[]? Encrypted { get; init; }

            public ReadOnlySpan<byte> Pack() {
                if( From == null || To == null || Encrypted == null ) throw new Exception("Invalid object");

                return Buffers.From(
                    From,
                    To,
                    // Hack: Only getting bytes is NOT architecture independent.
                    // This should be explicit network byte order.
                    BitConverter.GetBytes(Nonce),
                    BitConverter.GetBytes(Check).AsSpan()[..4],
                    EncodeVarInt32(Encrypted.Length),
                    Encrypted
                );
            }
        }

        public string MemoPrefix { get; set; } = "#";
        public string AddressPrefix { get; set; } = "STM";

        public Memo() { }

        public Memo(string memoPrefix, string addressPrefix) {
            MemoPrefix = memoPrefix;
            AddressPrefix = addressPrefix;
        }

        public string Encode(string memo, string publicKey, string privateKey) {
            if( string.IsNullOrEmpty(memo) ) throw new ArgumentException("Invalid memo");
            if( string.IsNullOrEmpty(publicKey) || !publicKey.StartsWith(AddressPrefix) ) throw new ArgumentException("Invalid public key");
            if( string.IsNullOrEmpty(privateKey) ) throw new ArgumentException("Invalid private key");

            return Encode(
                memo.StartsWith(MemoPrefix) ? memo[MemoPrefix.Length..] : memo,
                PublicKey.From(publicKey),
                PrivateKey.From(privateKey)
            );
        }

        private string Encode(string memo, PublicKey publicKey, PrivateKey privateKey) {
            if( publicKey == null || privateKey == null ) throw new Exception("Unable to load public or private keys");


            var bytes = Encoding.UTF8.GetBytes(memo);
            if( bytes == null ) throw new Exception("Unable to encode message buffer");
            
            var memoBuffer = Buffers.From(
                EncodeVarInt32(bytes.Length),
                bytes
            );

            var nonce = Convert.ToUInt64(109219769622765344); //UniqueNonce());
            
            Span<byte> encryptionKey = SHA512.HashData(Buffers.From(
                BitConverter.GetBytes(nonce),
                privateKey.GetSharedSecret(publicKey)
            ));
            var iv = encryptionKey.Slice(32, 16);
            var key = encryptionKey[..32];

            
            // TODO: Should this be int or maybe uint?
            // TODO: Explicit network byte order.
            var checkValue = BitConverter.ToInt32(SHA256.HashData(encryptionKey), 0);

            var encrypted = Encrypt(memoBuffer, iv.ToArray(), key.ToArray());

            var encryptedMemo = new EncryptedMemoObject() {
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

        private static byte[] Encrypt(ReadOnlySpan<byte> buffer, byte[] iv, byte[] key) {
            using var crypto = CreateCrypto(iv, key);
            using var encryptor = crypto.CreateEncryptor();
            using var memory = new MemoryStream();
            using var swEncrypt = new StreamWriter(new CryptoStream(memory, encryptor, CryptoStreamMode.Write, false));
            swEncrypt.Write(Encoding.UTF8.GetString(buffer));
            return memory.ToArray();
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte[] EncodeVarInt32(int value) => EncodeVarInt32(unchecked((uint)value));

        private static byte[] UniqueEntrophy = RandomNumberGenerator.GetBytes(2);

        private static long UniqueNonce() {
            var time = DateTime.Now.Ticks;
            var entropy = Convert.ToInt32(UniqueEntrophy[0] << 8 | UniqueEntrophy[1]) % 0xffff;
            return time << 16 | long.Parse(entropy.ToString());
        }
    }
}
