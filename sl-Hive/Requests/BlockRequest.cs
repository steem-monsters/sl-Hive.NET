using Newtonsoft.Json;
using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    [RpcMethod("condenser_api", "get_block")]
    public class BlockRequest : HiveJsonRequest
    {
        [JsonProperty("params")]
        public IReadOnlyList<long> BlockNumber { get; set; }
    }
}
