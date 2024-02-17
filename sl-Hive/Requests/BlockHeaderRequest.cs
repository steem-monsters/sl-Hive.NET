using System.Text.Json.Serialization;
using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    //get_block_header
    [RpcMethod("condenser_api", "get_block_header")]
    public class BlockHeaderRequest : HiveJsonRequest
    {
        [JsonPropertyName("params")]
        public IReadOnlyList<long> BlockNumber { get; set; }
    }
}
