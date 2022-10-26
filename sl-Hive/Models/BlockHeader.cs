using System.Text.Json.Nodes;

namespace sl_Hive.Models
{
    public class BlockHeader
    {
        public string Previous { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Witness { get; set; }
        public string Transaction_Merkle_Root { get; set; }
        public IReadOnlyList<JsonNode> Extensions { get; set; }
    }
}
