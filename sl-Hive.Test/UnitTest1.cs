using Cryptography.ECDSA;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using sl_Hive.Models;
using sl_Hive.Requests;
using System.Security.Cryptography;

namespace sl_Hive.Test
{
    [TestClass]
    public class UnitTest1
    {
        private HiveEngine hive = new HiveEngine();
        [TestMethod]
        public void ReadGlobalBlockchainProperties()
        {
            var task = hive.QueryBlockchain<HiveDynamicGlobalProperties>(new HiveDynamicGlobalPropertiesRequest());
            Task.WaitAll(task);

            Assert.IsNotNull(task.Result);
            var response = task.Result;

            Assert.IsNotNull(response.Result);
        }

        [TestMethod]
        public void ReadBlock()
        {
            var task = hive.QueryBlockchain<HiveDynamicGlobalProperties>(new HiveDynamicGlobalPropertiesRequest());
            Task.WaitAll(task);

            Assert.IsNotNull(task.Result);
            var response = task.Result;
            Assert.IsNotNull(response.Result);

            var blockTask = hive.QueryBlockchain<Block>(new BlockRequest() { BlockNumber = new List<Int64>() { response.Result.Head_Block_Number } });
            Task.WaitAll(blockTask);
            Assert.IsNotNull(blockTask.Result);
            Assert.IsNotNull(blockTask.Result.Result);

        }

        [TestMethod]
        public void GetAccounts()
        {
            var task = hive.QueryBlockchain<Accounts[]>(
                new AccountsRequest()
                {
                    Accounts = new List<List<string>>() {
                        new List<string>() {
                            "farpetrad", "ahsoka", "cryptomancer", "antiosh"
                        }
                    }
                }
            );
            Task.WaitAll(task);
            Assert.IsNotNull(task.Result);
            var response = task.Result;
            Assert.IsNotNull(response.Result);

        }

        [TestMethod]
        public void GetBlockHeader()
        {
            var task = hive.QueryBlockchain<HiveDynamicGlobalProperties>(new HiveDynamicGlobalPropertiesRequest());
            Task.WaitAll(task);

            Assert.IsNotNull(task.Result);
            var response = task.Result;
            Assert.IsNotNull(response.Result);

            var blockTask = hive.QueryBlockchain<BlockHeader>(new BlockHeaderRequest() { BlockNumber = new List<Int64>() { response.Result.Head_Block_Number } });
            Task.WaitAll(blockTask);
            Assert.IsNotNull(blockTask.Result);
            Assert.IsNotNull(blockTask.Result.Result);
        }

        [TestMethod]
        public void MemoEncode()
        {
            var memo = new Memo();
            //var x = memo.Encode("#testingtesting", "STM8m5UgaFAAYQRuaNejYdS8FVLVp9Ss3K1qAVk5de6F8s3HnVbvA", "5JdeC9P7Pbd1uGdFVEsJ41EkEnADbbHGq6p1BwFxm6txNBsQnsw");




            var key = PrivateKey.From("5JdeC9P7Pbd1uGdFVEsJ41EkEnADbbHGq6p1BwFxm6txNBsQnsw");
            var pub = key.GetPublicKey();

            var pk = Secp256K1Manager.GetPublicKey(key.Bytes, false);


            var pkey = PublicKey.From("STM8m5UgaFAAYQRuaNejYdS8FVLVp9Ss3K1qAVk5de6F8s3HnVbvA");
            var S = GetSharedSecret(pkey, key);
            var X = key.GetSharedSecret(pkey);
            Assert.IsTrue(S.SequenceEqual(X));
        }

        byte[] GetSharedSecret(PublicKey publicKey, PrivateKey privateKey)
        {
            if (publicKey == null || privateKey == null) throw new Exception("Both keys must be valid");
            var KB = publicKey?.Q?.GetEncoded(false);

            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());


            var c = domain.Curve.Configure().SetCoordinateSystem(Org.BouncyCastle.Math.EC.ECCurve.COORD_AFFINE).Create();


            var lower = KB?.Skip(1).Take(32).ToArray();
            var upper = KB?.Skip(33).ToArray();


            var KBP = c.CreatePoint(
                new Org.BouncyCastle.Math.BigInteger(1, lower),
                new Org.BouncyCastle.Math.BigInteger(1, upper)
                );

            var r = privateKey.D.ToByteArray().Take(32).Reverse().ToArray();

            var P = KBP.Multiply(new Org.BouncyCastle.Math.BigInteger(1, r));
            var S = P.AffineXCoord.ToBigInteger().ToByteArrayUnsigned();

            using (var sha512 = SHA512.Create())
            {
                return sha512.ComputeHash(S);
            }
        }
    }
}