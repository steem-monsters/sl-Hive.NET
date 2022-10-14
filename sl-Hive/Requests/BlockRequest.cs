using Newtonsoft.Json;
using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    [RPCMethod("condenser_api", "get_block")]
    public class BlockRequest : HiveJsonRequest
    {
        [JsonProperty("params")]
        public List<Int64> BlockNumber { get; set; } = new List<Int64>();
    }
}
