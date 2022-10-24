using Cryptography.ECDSA;
using System.Security.Cryptography;

namespace sl_Hive
{
    public class KeyUtils
    {
        public static int CHECKSUM_SIZE_BYTES = 4;
        public static byte[] NETWORK_ID = Convert.FromHexString("80");
        public static byte[] DecodePrivateWif(string encodedKey)
        {
            var s = Base58.Decode(encodedKey);

            var network = new byte[1];
            Array.Copy(s, network, network.Length);

            if (!network.SequenceEqual(NETWORK_ID)) throw new Exception("Error private key network mismatch");

            var checkSum = s.Skip(s.Length - 4).ToArray();

            var key = CutLastBytes(s, 4);
            var checksumVerify = Checksum(key).Take(4)
                                              .ToArray();

            if (!ValidateChecksum(s, checksumVerify, 4)) throw new Exception("Invalid checksum");
            key = CutFirstBytes(key, 1);
            return key;
        }


        public static string EncodePrivateWif(byte[] buffer)
        {
            var network = buffer.Take(1).ToArray();

            if (!network.SequenceEqual(NETWORK_ID)) throw new Exception("Error private key network mismatch");

            var checkSum = Checksum(buffer).Take(4).ToArray();

            return Base58.Encode(buffer.Concat(checkSum).ToArray());
        }

        private static byte[] CutLastBytes(byte[] source, int cutCount)
        {
            return source.Take(source.Length - cutCount).ToArray();
        }

        private static byte[] CutFirstBytes(byte[] source, int cutCount)
        {
            return source.Skip(cutCount).ToArray();
        }

        private static byte[] Checksum(byte[] hash)
        {            
            return SHA256.HashData(SHA256.HashData((hash)));
        }

        private static bool ValidateChecksum(byte[] s, byte[] checkSum, int byteLength = 4)
        {
            for (var i = 0; i < byteLength; i++)
            {
                if (checkSum[i] != s[s.Length - byteLength + i])
                    return false;
            }
            return true;
        }

        public static DecodedPublicKey DecodePublicWif(string encodedKey)
        {
            var prefix = encodedKey.Substring(0, 3);
            var slicedKey = encodedKey.Substring(3);

            var buffer = Base58.Decode(slicedKey);

            var checkSum = buffer.Skip(buffer.Length - CHECKSUM_SIZE_BYTES).ToArray();

            var key = buffer.Take(buffer.Length - CHECKSUM_SIZE_BYTES).ToArray();

            var checkSumVerify = Ripemd160Manager.GetHash(key).Take(4).ToArray();

            if (!checkSum.SequenceEqual(checkSumVerify)) throw new Exception("Invalid checksum");

            return new DecodedPublicKey() { Buffer = key, Prefix = prefix };
        }

        /// <summary>
        /// Compute the public key from a private key Wif
        /// </summary>
        /// <param name="publicKey">32 byte private wif</param>
        /// <param name="preFix">Prefix to add to key, Default STM for Hive</param>
        /// <returns></returns>
        public static string EncodePublicWif(byte[] publicKey, string preFix = "STM")
        {
            var checksum = Ripemd160Manager.GetHash(publicKey);
            var expandedData = publicKey.Concat(checksum.Take(4)).ToArray();
            var pubdata = Base58.Encode(expandedData);
            return preFix + pubdata;
        }
    }

    public class DecodedPublicKey
    {
        public byte[]? Buffer { get; set; }
        public string? Prefix { get; set; }
    }
}
