using Cryptography.ECDSA;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;
using System.Linq;
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

            public byte[] ToByteArray()
            {
                if (From == null || To == null || Encrypted == null) throw new Exception("Invalid object");
                var variant = Memo.WriteVariant32(Encrypted.Length);
                if (variant == null) throw new Exception("Unable to generate variant during encoding");
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

        public Memo()
        {

        }

        public Memo(string memoPrefix, string addressPrefix)
        {
            MemoPrefix = memoPrefix;
            AddressPrefix = addressPrefix;
        }

        public string Encode(string memo, string publicKey, string privateKey)
        {
            if (memo == null || memo.Length == 0) throw new ArgumentException("Invalid memo");
            if (publicKey == null || publicKey.Length == 0 || !publicKey.StartsWith(AddressPrefix)) throw new ArgumentException("Invalid public key");
            if (privateKey == null || privateKey.Length == 0) throw new ArgumentException("Invalid private key");

            if (memo.StartsWith(MemoPrefix))
            {
                memo = memo.Substring(MemoPrefix.Length);
            }

            var pubKey = PublicKey.From(publicKey);
            var privKey = PrivateKey.From(privateKey);
            if (pubKey == null || privKey == null) throw new Exception("Unable to load public or private keys");


            var bytes = Encoding.UTF8.GetBytes(memo);
            if (bytes == null) throw new Exception("Unable to encode message buffer");
            var memoBuffer = WriteVariant32(bytes.Length)?.Concat(bytes).ToArray();
            if (memoBuffer == null) throw new Exception("Error creating memo buffer");

            var nonce = Convert.ToUInt64(109219769622765344);//UniqueNonce());

            var buffer = BitConverter.GetBytes(nonce);
            if (buffer == null) throw new Exception("Error getting nonce");
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

            var encryptedMemo = new EncryptedMemoObject()
            {
                Check = checkValue,
                Nonce = nonce,
                Encrypted = encrypted,
                From = PublicKey.From(privKey.GetPublicKey()).Key,
                To = pubKey.Key
            }.ToByteArray();

            return $"{MemoPrefix}{Base58.Encode(encryptedMemo)}";
        }

        private static byte[] Encrypt(byte[] buffer, byte[] iv, byte[] key)
        {
            using(var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = key;
                aesAlg.IV = iv;


                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(Encoding.UTF8.GetString(buffer));
                            }
                            var encrypted = msEncrypt.ToArray();
                            return encrypted;
                        }
                    }


                }
            }
        }

        private string Decrypt(byte[] buffer, byte[] iv, byte[] key)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (var memStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (var strStream = new StreamReader(cryptoStream))
                            {
                                return strStream.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        private static int CalculateVarint32(int value)
        {
            // ref: src/google/protobuf/io/coded_stream.cc
            var result = value >> 0;
            if (result < 1 << 7) return 1;
            else if (result < 1 << 14) return 2;
            else if (result < 1 << 21) return 3;
            else if (result < 1 << 28) return 4;
            else return 5;
        }
        internal static byte[]? WriteVariant32(int value)
        {
            // ref: https://github.com/protobufjs/bytebuffer.js/blob/master/src/types/varints/varint32.js
            var size = CalculateVarint32(value);
            var b = 0;
            var result = value >> 0;

            var bytes = new List<byte>();

            while (result >= 0x80)
            {
                b = (result & 0x7f) | 0x80;
                //? if (NODE)
                bytes.Add(BitConverter.GetBytes(b)[0]);
                result = result >> 7;
            }
            bytes.Add(BitConverter.GetBytes(result)[0]);

            return bytes.ToArray();
        }
        
        private static byte[] UniqueEntrophy = RandomNumberGenerator.GetBytes(2);

        private static long UniqueNonce()
        {
            var time = DateTime.Now.Ticks;
            var entropy = Convert.ToInt32(UniqueEntrophy[0] << 8 | UniqueEntrophy[1]) % 0xffff;
            return time << 16 | long.Parse(entropy.ToString());
        }
    }
}
