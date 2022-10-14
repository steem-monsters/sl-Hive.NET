using Newtonsoft.Json;
using sl_Hive.Models;
using sl_Hive.Requests;

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
                new AccountsRequest() {  
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
    }
}