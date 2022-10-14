using Newtonsoft.Json.Linq;

namespace sl_Hive.Models
{
    public class Block
    {
        public string Previous { get; set; } = string.Empty;
        public DateTime? Timestamp { get; set; } = null;
        public string Witness { get; set; } = string.Empty;
        public string Transaction_Merkle_Root { get; set; } = string.Empty;
        public List<JObject> Extensions { get; set; } = new List<JObject>();
        public string Witness_Signature { get; set; } = string.Empty;
        public List<HiveTransaction> Transactions { get; set; } = new List<HiveTransaction>();
        public string Block_Id { get; set; } = string.Empty;
        public string Signing_Key { get; set; } = string.Empty;
        public List<string> Transaction_Ids { get; set; } = new List<string>();
    }
}
