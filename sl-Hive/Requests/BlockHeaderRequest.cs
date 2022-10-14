using Newtonsoft.Json;
using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    //get_block_header
    [RPCMethod("condenser_api", "get_block_header")]
    public class BlockHeaderRequest : HiveJsonRequest
    {
        [JsonProperty("params")]
        public List<Int64> BlockNumber { get; set; } = new List<Int64>();
    }
}
