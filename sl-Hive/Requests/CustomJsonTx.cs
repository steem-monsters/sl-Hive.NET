using sl_Hive.Attributes;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace sl_Hive.Requests
{
    public class CustomJsonParams
    {
        public ushort ref_block_num { get; set; }
        public UInt32 ref_block_prefix { get; set; }
        public DateTime? expiration { get; set; } = null;
        public JsonArray operations { get; set; } = new JsonArray();
        public JsonNode[] extensions { get; set; } = new JsonNode[0];
        public string[] signatures { get; set; } = new string[0];

    }

    

    

    [RpcMethod("condenser_api", "broadcast_transaction")]
    public class CustomJsonTx : HiveJsonRequest
    {
        [JsonPropertyName("params")]
        public JsonArray Params = new JsonArray();    
    }
}
