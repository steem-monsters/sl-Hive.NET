using System.Text.Json.Serialization;

namespace sl_Hive
{
    public class HiveJsonRPCResult<TResultType>
    {
        [JsonPropertyName("jsonrpc")]
        public string Jsonrpc { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public TResultType? Result { get; set; }
    }
}
