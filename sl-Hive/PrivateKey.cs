using Cryptography.ECDSA;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using System.Numerics;
using System.Security.Cryptography;

namespace sl_Hive
{
	public class PrivateKey
	{
		public static PrivateKey From(string key) {
			var wif = KeyUtils.DecodePrivateWif(key);

			var networkId = Convert.FromHexString("80");
			var buffered = networkId.Concat(wif).ToArray();

			var decodedWif = KeyUtils.EncodePrivateWif(buffered);
			var pkey = new PrivateKey(wif);


			return pkey;
		}

		public readonly byte[] Bytes;
		public BigInteger D { get; set; } = 0;

		private PrivateKey(byte[] wif) {
			Bytes = wif;
			D = new BigInteger(wif, isBigEndian: true, isUnsigned: true);
		}

		public byte[] GetSharedSecret(PublicKey publicKey) {
			if( publicKey.Q == null ) throw new Exception("Public key must be valid");
			var KB = publicKey.Q.GetEncoded(false);

			var curve = SecNamedCurves.GetByName("secp256k1");
			var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

			// need a curve in the correct coordinate system!
			var c = domain.Curve.Configure().SetCoordinateSystem(Org.BouncyCastle.Math.EC.ECCurve.COORD_AFFINE).Create();

			var lower = KB.Skip(1)
				.Take(32)
				.ToArray();
			var upper = KB.Skip(33)
				.ToArray();
			// super important to signify that these are signed integers!
			var KBP = c.CreatePoint(
				new Org.BouncyCastle.Math.BigInteger(1, lower),
				new Org.BouncyCastle.Math.BigInteger(1, upper));

			var r = D.ToByteArray().Take(32).Reverse().ToArray();

			var P = KBP.Multiply(new Org.BouncyCastle.Math.BigInteger(1, r));
			var S = P.AffineXCoord.ToBigInteger().ToByteArrayUnsigned();


			return SHA512.HashData(S);
		}

		public string GetPublicKey() {
			var bytes = Secp256K1Manager.GetPublicKey(Bytes, true);
			var key = KeyUtils.EncodePublicWif(bytes);
			return key;
		}
	}
}
