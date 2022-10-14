using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace sl_Hive.Models
{
    public class CustomJson
    {
        [JsonProperty("required_auths")]
        public List<string> Required_Auths { get; set; } = new List<string>();
        [JsonProperty("required_posting_auths")]
        public List<string> Required_Posting_Auths { get; set; } = new List<string>();
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;
        [JsonProperty("json")]
        public JObject? Json { get; set; } = null;
    }
}
