using sl_Hive.Models;
using sl_Hive.Requests;

namespace sl_Hive.Test
{
    [TestClass]
    public class UnitTest1
    {
        private HiveEngine hive = new HiveEngine(new HttpClient(), RPCNodeCollection.DefaultNodes);

        [TestMethod]
        public void ReadGlobalBlockchainProperties() {
            var task = hive.QueryBlockchain<HiveDynamicGlobalProperties>(new HiveDynamicGlobalPropertiesRequest());
            Task.WaitAll(task);

            Assert.IsNotNull(task.Result);
            var response = task.Result;

            Assert.IsNotNull(response.Result);
        }

        [TestMethod]
        public async Task ReadBlock() {
            var response = await hive.QueryBlockchain<HiveDynamicGlobalProperties>(new HiveDynamicGlobalPropertiesRequest());
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
        public async Task GetBlockHeader() {
            var response = await hive.QueryBlockchain<HiveDynamicGlobalProperties>(new HiveDynamicGlobalPropertiesRequest());
            Assert.IsNotNull(response.Result);

            var block = await hive.QueryBlockchain<BlockHeader>(new BlockHeaderRequest {
                BlockNumber = new[] { response.Result.Head_Block_Number }
            });
            Assert.IsNotNull(block);
            Assert.IsNotNull(block.Result);
        }

        [TestMethod]
        public void MemoEncode() {
            var memo = new Memo();
            var encodedMemo = memo.Encode(
                "#testingtesting",
                "STM8m5UgaFAAYQRuaNejYdS8FVLVp9Ss3K1qAVk5de6F8s3HnVbvA",
                "5JdeC9P7Pbd1uGdFVEsJ41EkEnADbbHGq6p1BwFxm6txNBsQnsw",
                BitConverter.GetBytes(Convert.ToUInt64(109219769622765344))
            );
        }
    }
}
