using System.Text.Json.Nodes;

namespace sl_Hive.Models
{
    public class Block
    {
        public string Previous { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Witness { get; set; }
        public string Transaction_Merkle_Root { get; set; }
        public IReadOnlyList<JsonNode> Extensions { get; set; }
        public string Witness_Signature { get; set; }
        public IReadOnlyList<HiveTransaction> Transactions { get; set; }
        public string Block_Id { get; set; }
        public string Signing_Key { get; set; }
        public IReadOnlyList<string> Transaction_Ids { get; set; }
    }
}
