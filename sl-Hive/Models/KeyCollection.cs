using Newtonsoft.Json.Linq;

namespace sl_Hive.Models
{
    public class KeyCollection
    {
        public int Weight_Threshold { get; set; } = 0;
        public List<JArray> Account_Auths { get; set; } = new List<JArray>();
        public List<JArray> Key_Auths { get; set; } = new List<JArray>();
    }
}
