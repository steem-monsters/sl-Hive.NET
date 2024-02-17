using System.Security.Cryptography;
using Cryptography.ECDSA;
using Base58 = SimpleBase.Base58;


namespace sl_Hive
{
    public class KeyUtils
    {
        public static int CHECKSUM_SIZE_BYTES = 4;
        public static byte NETWORK_ID = 0x80;

        public static ReadOnlySpan<byte> DecodePrivateWif(string encodedKey) {
            var data = Base58.Bitcoin.Decode(encodedKey);

            if( data[0] != NETWORK_ID ) throw new Exception("Error private key network mismatch");

            var checkSum = data[^CHECKSUM_SIZE_BYTES..];

            var actualChecksum = DoubleHash(data[..^4])[..4];
            if( !actualChecksum.SequenceEqual(checkSum) ) {
                throw new Exception("Invalid checksum");
            }

            return data[1..^4];
        }

        public static string EncodePrivateWif(ReadOnlySpan<byte> buffer) {
            if( buffer[0] != NETWORK_ID ) throw new Exception("Error private key network mismatch");
            var checkSum = DoubleHash(buffer)[..4];
            return Base58.Bitcoin.Encode(Buffers.From(buffer, checkSum));
        }

        private static ReadOnlySpan<byte> DoubleHash(ReadOnlySpan<byte> data) => SHA256.HashData(SHA256.HashData(data));

        public static DecodedPublicKey DecodePublicWif(string encodedKey) {
            var prefix = encodedKey[..3];
            var slicedKey = encodedKey[3..];

            var buffer = Base58.Bitcoin.Decode(slicedKey);

            var checkSum = buffer[^CHECKSUM_SIZE_BYTES..];
            var key = buffer[..^CHECKSUM_SIZE_BYTES];

            var actualChecksum = Ripemd160Manager.GetHash(key.ToArray()).AsSpan()[..4];

            if( !checkSum.SequenceEqual(actualChecksum) ) throw new Exception("Invalid checksum");

            return new DecodedPublicKey {
                Buffer = key,
                Prefix = prefix
            };
        }

        /// <summary>
        /// Compute the public key from a private key Wif
        /// </summary>
        /// <param name="publicKey">32 byte private wif</param>
        /// <param name="prefix">Prefix to add to key, Default STM for Hive</param>
        /// <returns></returns>
        public static string EncodePublicWif(ReadOnlySpan<byte> publicKey, string prefix = "STM") {
            var checksum = Ripemd160Manager.GetHash(publicKey.ToArray()).AsSpan();
            var expandedData = Buffers.From(
                publicKey,
                checksum[..4]
            );

            return prefix + Base58.Bitcoin.Encode(expandedData);
        }
    }

    public readonly ref struct DecodedPublicKey
    {
        public Span<byte> Buffer { get; init; }
        public string? Prefix { get; init; }
    }
}
