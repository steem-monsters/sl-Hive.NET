using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC;

namespace sl_Hive
{
    public readonly ref struct PublicKey
    {
        public static PublicKey From(string key) {
            var wif = KeyUtils.DecodePublicWif(key);
            if( wif.Buffer == null || wif.Prefix == null ) throw new Exception($"Invalid public key {key}");

            var pkey = new PublicKey(wif.Buffer, key, wif.Prefix);
            return pkey;
        }

        private readonly string PubKey;
        public ReadOnlySpan<byte> Key { get; init; }
        public ECPoint Q { get; init; }
        public string Prefix { get; init; }
        
        private PublicKey(ReadOnlySpan<byte> wif, string pubKey, string prefix = "STM") {
            PubKey = pubKey;
            Key = wif;
            Prefix = prefix;

            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            Q = domain.Curve.DecodePoint(wif.ToArray());
        }

        public bool IsValid => Key != default;
    }
}
