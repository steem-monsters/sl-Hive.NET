using System.Text.Json.Serialization;
using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    [RpcMethod("condenser_api", "get_block")]
    public class BlockRequest : HiveJsonRequest
    {
        [JsonPropertyName("params")]
        public IReadOnlyList<uint> BlockNumber { get; set; }
    }
}
