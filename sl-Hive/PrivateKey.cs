using Cryptography.ECDSA;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using System.Numerics;
using System.Security.Cryptography;

namespace sl_Hive
{
    public readonly ref struct PrivateKey
    {
        public static PrivateKey From(string key) {
            var wif = KeyUtils.DecodePrivateWif(key);

            var networkId = new byte[] { 0x80 };
            var buffered = Buffers.From(
                networkId,
                wif
            );

            var decodedWif = KeyUtils.EncodePrivateWif(buffered);
            var pkey = new PrivateKey(wif);


            return pkey;
        }

        public readonly ReadOnlySpan<byte> Bytes;
        public BigInteger D { get; init; } = 0;
        
        public bool IsValid => Bytes != default;

        private PrivateKey(ReadOnlySpan<byte> wif) {
            Bytes = wif;
            D = new BigInteger(wif, isBigEndian: true, isUnsigned: true);
        }

        public ReadOnlySpan<byte> GetSharedSecret(PublicKey publicKey) {
            if( publicKey.Q == null ) throw new Exception("Public key must be valid");
            var KB = publicKey.Q.GetEncoded(false).AsSpan();

            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            // need a curve in the correct coordinate system!
            var c = domain.Curve.Configure().SetCoordinateSystem(Org.BouncyCastle.Math.EC.ECCurve.COORD_AFFINE).Create();

            var lower = KB.Slice(1, 32);
            var upper = KB[33..];

            // super important to signify that these are signed integers!
            var KBP = c.CreatePoint(
                new Org.BouncyCastle.Math.BigInteger(1, lower.ToArray()),
                new Org.BouncyCastle.Math.BigInteger(1, upper.ToArray())
            );

            var r = D.ToByteArray().AsSpan(0, 32);
            r.Reverse();

            var P = KBP.Multiply(new Org.BouncyCastle.Math.BigInteger(1, r.ToArray()));
            var S = P.AffineXCoord.ToBigInteger().ToByteArrayUnsigned().AsSpan();
            
            return SHA512.HashData(S);
        }

        public string GetPublicKey() {
            var bytes = Secp256K1Manager.GetPublicKey(Bytes.ToArray(), true);
            var key = KeyUtils.EncodePublicWif(bytes);
            return key;
        }
    }
}
