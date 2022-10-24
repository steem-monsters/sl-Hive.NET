using Newtonsoft.Json.Linq;

namespace sl_Hive.Models
{
    public class HiveTransaction
    {
        public string Ref_Block_Num { get; set; } = string.Empty;
        public string Ref_Block_Prefix { get; set; } = string.Empty;
        public DateTime? Expiration { get; set; } = null;
        public IReadOnlyList<JArray> Operations = new List<JArray>();
        public IReadOnlyList<JObject> Extensions { get; set; } = new List<JObject>();
        public IReadOnlyList<string> Signatures { get; set; } = new List<string>();
        public string Transaction_id { get; set; } = string.Empty;
        public long Block_Num { get; set; } = -1;
        public long Transaction_Num { get; set; } = -1;
    }
}
