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
