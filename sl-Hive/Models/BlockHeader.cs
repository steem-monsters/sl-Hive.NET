using System.Text.Json.Nodes;

namespace sl_Hive.Models
{
    public class BlockHeader
    {
        public string Previous { get; set; } = string.Empty;
        public DateTime? TimeStamp { get; set; }
        public string Witness { get; set; } = string.Empty;
        public string Transaction_Merkle_Root { get; set; } = string.Empty;
        public IReadOnlyList<JsonNode> Extensions { get; set; } = new List<JsonNode>();
    }
}
