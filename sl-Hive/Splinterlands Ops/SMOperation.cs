using System.Text.Json.Serialization;

namespace sl_Hive.Splinterlands_Ops
{
    [JsonDerivedType(typeof(StakeTokens))]
    public abstract class SMOperation
    {
        public string app { get; set; } = "sl-hive";
        public string n { get; set; } = string.Empty;
    }
}
