using sl_Hive.Models;
using sl_Hive.Requests;

namespace sl_Hive.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReadGlobalBlockchainProperties()
        {
            var hive = new HiveEngine();
            var task = hive.QueryBlockchain<HiveDynamicGlobalProperties>(new HiveDynamicGlobalPropertiesRequest());
            Task.WaitAll(task);

            Assert.IsNotNull(task.Result);
            var response = task.Result;

            Assert.IsNotNull(response.Result);
        }
    }
}