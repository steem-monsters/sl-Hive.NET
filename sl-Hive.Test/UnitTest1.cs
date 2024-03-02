using Microsoft.Extensions.Configuration;
using sl_Hive.Models;
using sl_Hive.Requests;
using sl_Hive.Splinterlands_Ops;
using System.Reflection;

namespace sl_Hive.Test
{
    [TestClass]
    public class UnitTest1
    {
        private HiveEngine hive = new HiveEngine(new HttpClient(), RPCNodeCollection.DefaultNodes);
        const string PrivKey = "5JdeC9P7Pbd1uGdFVEsJ41EkEnADbbHGq6p1BwFxm6txNBsQnsw";
        const string PublicKey = "STM8m5UgaFAAYQRuaNejYdS8FVLVp9Ss3K1qAVk5de6F8s3HnVbvA";
        private readonly string User;
        private readonly string PrivatePostingKey;

        public UnitTest1()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly(), true).AddEnvironmentVariables();

            var Configuration = builder.Build();

            PrivatePostingKey = Configuration["KEY"] ?? string.Empty;
            User = Configuration["HIVEUSERNAME"] ?? string.Empty;

            if (PrivatePostingKey == string.Empty)
            {
                PrivatePostingKey = Environment.GetEnvironmentVariable("KEY") ?? string.Empty;
            }
            if (User == string.Empty)
            {
                User = Environment.GetEnvironmentVariable("HIVEUSERNAME") ?? string.Empty;
            }
        }

        [TestMethod]
        public async Task ReadGlobalBlockchainProperties() {
            var response = await hive.QueryBlockchain<HiveDynamicGlobalProperties>(HiveDynamicGlobalPropertiesRequest.Instance);
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
        }

        [TestMethod]
        public async Task ReadBlock() {
            var response = await hive.QueryBlockchain<HiveDynamicGlobalProperties>(HiveDynamicGlobalPropertiesRequest.Instance);
            Assert.IsNotNull(response.Result);

            var block = await hive.QueryBlockchain<Block>(new BlockRequest { BlockNumber = new[] { response.Result.Head_Block_Number } });
            Assert.IsNotNull(block);
            Assert.IsNotNull(block.Result);
        }

        [TestMethod]
        public async Task GetAccounts() {
            var response = await hive.QueryBlockchain<Accounts[]>(
                new AccountsRequest {
                    Accounts = new[] {
                        new[] {
                            "farpetrad", "ahsoka", "cryptomancer", "antiosh"
                        }
                    }
                }
            );
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Result);
        }

        [TestMethod]
        public async Task PostTransaction()
        {
            //var hash = Sha256Manager.GetHash(
            //    Encoding.ASCII.GetBytes(
            //        "farpetrad" + new DateTimeOffset(new DateTime()).ToUnixTimeMilliseconds().ToString())
            //    );
            //var bytes = CBase58.DecodePrivateWif("");
            //var sig = Secp256K1Manager.SignCompressedCompact(hash, bytes);
            //var signature = Hex.ToString(sig);


            /*var response = await hive.QueryBlockchain<HiveDynamicGlobalProperties>(HiveDynamicGlobalPropertiesRequest.Instance);
            var request = new CustomJsonTx();

            var trx = new custom_json()
            {
                id = "sm_stake_tokens", // whatever operation the game should perform
                required_posting_auths = ["ahsoka"], // posting key ops
                required_auths = [],                 // active key ops
                json = JsonSerializer.Serialize(new StakeTokens() { qty = 1 }, HiveEngine._options),
            };
            var trans = new Transaction()
            {
                ref_block_num = (ushort)((ushort)response.Result?.Head_Block_Number & (ushort)0xFFFF),
                ref_block_prefix = response.Result?.Ref_Block_Prefix ?? 0,
                operations = new object[] { trx },
                expiration = response?.Result?.Time.AddSeconds(30) ?? DateTime.UtcNow.AddSeconds(30),
                signatures = new string[0],
                extensions = new string[0],
            };
            var serializer = new SignatureSerializer();
            var msg = serializer.Serialize(trans);
            using(var memStream = new MemoryStream())
            {
                var chainIdBytes = Hex.HexToBytes(CHAINID);
                memStream.Write(chainIdBytes, 0, chainIdBytes.Length);
                memStream.Write(msg, 0, msg.Length);
                
                var digest = Sha256Manager.GetHash(memStream.ToArray());
                var signatureBytes = CBase58.DecodePrivateWif("");


                trans.signatures = new[] { Hex.ToString(Secp256K1Manager.SignCompressedCompact(digest, signatureBytes)) };
                var trxId = Hex.ToString(Sha256Manager.GetHash(msg)).Substring(0, 40);
            };
            request.Params = new JsonArray(JsonSerializer.SerializeToNode(trans.ToParams(), HiveEngine._options));
            var post = await hive.QueryBlockchain<JObject>(request);*/
            var post = await hive.BroadcastCustomJson(new StakeTokens() { qty = 1 },
                                                      User,
                                                      PrivatePostingKey);
            Assert.IsNotNull(post);
        }

        [TestMethod]
        public async Task GetBlockHeader() {
            var response = await hive.QueryBlockchain<HiveDynamicGlobalProperties>(new HiveDynamicGlobalPropertiesRequest());
            Assert.IsNotNull(response.Result);

            var block = await hive.QueryBlockchain<BlockHeader>(new BlockHeaderRequest {
                BlockNumber = new ulong[] { response.Result.Head_Block_Number }
            });
            Assert.IsNotNull(block);
            Assert.IsNotNull(block.Result);
        }

        [TestMethod]
        public void MemoEncode() {
            var testMemo = "testingtesting";

            var memo = new Memo();
            var encodedMemo = memo.Encode(
                $"{'#'}{testMemo}",
                PublicKey,
                PrivKey,
                BitConverter.GetBytes(Convert.ToUInt64(109219769622765344))
            );

            Assert.IsTrue("#K55WaPFbgNW8w8UiPzFGRejmMLZH3CA6guETaVLS7fUGgYhSwWTXjQ26ozhA6zFtG339Tsjw5AXqce8v4HCsYZ9kFuiPKJ4UMujGLTXckCyYsEW1wKcec9Zz4fkvshNE3" == encodedMemo);

            var decodedMemo = memo.Decode(encodedMemo, PrivKey);

            Assert.IsTrue(testMemo == decodedMemo);
        }
    }
}
