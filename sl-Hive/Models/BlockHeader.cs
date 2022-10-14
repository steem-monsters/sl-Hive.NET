using Newtonsoft.Json.Linq;

namespace sl_Hive.Models
{
    public class BlockHeader
    {
        public string Previous { get; set; } = string.Empty;
        public DateTime? TimeStamp { get; set; } = null;
        public string Witness { get; set; } = string.Empty;
        public string Transaction_Merkle_Root { get; set; } = string.Empty;
        public List<JObject> Extensions { get; set; } = new List<JObject>();
    }
}
