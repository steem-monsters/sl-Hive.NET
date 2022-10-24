using Cryptography.ECDSA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace sl_Hive
{
    public class Memo
    {
        private class EncryptedMemoObject
        {
            public byte[]? From { get; set; } = null;
            public byte[]? To { get; set; } = null;
            public long Check { get; set; } = 0;
            public ulong Nonce { get; set; } = 0;
            public byte[]? Encrypted { get; set; } = null;

            public byte[] ToByteArray() {
                if( From == null || To == null || Encrypted == null ) throw new Exception("Invalid object");
                var variant = Memo.WriteVarInt32(Encrypted.Length);
                if( variant == null ) throw new Exception("Unable to generate variant during encoding");
                var result = From.Concat(To)
                    .Concat(BitConverter.GetBytes(Nonce))
                    .Concat(BitConverter.GetBytes(Check).Take(4))
                    .Concat(variant)
                    .Concat(Encrypted).ToArray();

                return result;
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
            if( memo == null || memo.Length == 0 ) throw new ArgumentException("Invalid memo");
            if( publicKey == null || publicKey.Length == 0 || !publicKey.StartsWith(AddressPrefix) ) throw new ArgumentException("Invalid public key");
            if( privateKey == null || privateKey.Length == 0 ) throw new ArgumentException("Invalid private key");

            if( memo.StartsWith(MemoPrefix) ) {
                memo = memo.Substring(MemoPrefix.Length);
            }

            var pubKey = PublicKey.From(publicKey);
            var privKey = PrivateKey.From(privateKey);
            if( pubKey == null || privKey == null ) throw new Exception("Unable to load public or private keys");


            var bytes = Encoding.UTF8.GetBytes(memo);
            if( bytes == null ) throw new Exception("Unable to encode message buffer");
            var memoBuffer = WriteVarInt32(bytes.Length)?.Concat(bytes).ToArray();
            if( memoBuffer == null ) throw new Exception("Error creating memo buffer");

            var nonce = Convert.ToUInt64(109219769622765344); //UniqueNonce());

            var buffer = BitConverter.GetBytes(nonce);
            if( buffer == null ) throw new Exception("Error getting nonce");
            buffer = buffer.Concat(privKey.GetSharedSecret(pubKey))
                .ToArray();

            var encryptionKey = SHA512.HashData(buffer);

            var iv = encryptionKey.Skip(32)
                .Take(16)
                .ToArray();
            var key = encryptionKey.Take(32)
                .ToArray();

            var checkValue = BitConverter.ToInt32(SHA256.HashData(encryptionKey)
                .Take(4)
                .ToArray());

            var encrypted = Encrypt(memoBuffer, iv, key);

            var encryptedMemo = new EncryptedMemoObject() {
                Check = checkValue,
                Nonce = nonce,
                Encrypted = encrypted,
                From = PublicKey.From(privKey.GetPublicKey()).Key,
                To = pubKey.Key
            }.ToByteArray();

            return $"{MemoPrefix}{Base58.Encode(encryptedMemo)}";
        }

        private static Aes CreateCrypto(byte[] iv, byte[] key) {
            var result = Aes.Create();
            result.Mode = CipherMode.CBC;
            result.Key = key;
            result.IV = iv;
            return result;
        }

        private static byte[] Encrypt(byte[] buffer, byte[] iv, byte[] key) {
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

        private static byte[] WriteVarInt32(uint value) {
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
        private static byte[] WriteVarInt32(int value) => WriteVarInt32(unchecked((uint)value));

        private static byte[] UniqueEntrophy = RandomNumberGenerator.GetBytes(2);

        private static long UniqueNonce() {
            var time = DateTime.Now.Ticks;
            var entropy = Convert.ToInt32(UniqueEntrophy[0] << 8 | UniqueEntrophy[1]) % 0xffff;
            return time << 16 | long.Parse(entropy.ToString());
        }
    }
}
