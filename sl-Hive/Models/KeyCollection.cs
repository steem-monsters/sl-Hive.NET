using System.Text.Json.Nodes;

namespace sl_Hive.Models
{
    public class KeyCollection
    {
        public int Weight_Threshold { get; set; } = 0;
        public IReadOnlyList<JsonArray> Account_Auths { get; set; } = new List<JsonArray>();
        public IReadOnlyList<JsonArray> Key_Auths { get; set; } = new List<JsonArray>();
    }
}
