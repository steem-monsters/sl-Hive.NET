using Newtonsoft.Json.Linq;

namespace sl_Hive.Models
{
    public class HiveTransaction
    {
        public string Ref_Block_Num { get; set; } = string.Empty;
        public string Ref_Block_Prefix { get; set; } = string.Empty;
        public DateTime? Expiration { get; set; } = null;
        public List<JArray> Operations = new List<JArray>();
        public List<JObject> Extensions { get; set; } = new List<JObject>();
        public List<string> Signatures { get; set; } = new List<string>();
        public string Transaction_id { get; set; } = string.Empty;
        public Int64 Block_Num { get; set; } = -1;
        public Int64 Transaction_Num { get; set; } = -1;
    }
}
