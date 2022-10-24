using System.Text.Json.Nodes;

namespace sl_Hive.Models
{
    public class HiveTransaction
    {
        public ulong Ref_Block_Num { get; set; }
        public ulong Ref_Block_Prefix { get; set; }
        public DateTime? Expiration { get; set; } = null;
        public IReadOnlyList<JsonArray> Operations = new List<JsonArray>();
        public IReadOnlyList<JsonNode> Extensions { get; set; } = new List<JsonNode>();
        public IReadOnlyList<string> Signatures { get; set; } = new List<string>();
        public string Transaction_id { get; set; } = string.Empty;
        public ulong Block_Num { get; set; }
        public ulong Transaction_Num { get; set; }
    }
}
