using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace sl_Hive.Models;

public class CustomJson
{
    [JsonPropertyName("required_auths")]
    public IReadOnlyList<string> RequiredAuths { get; init; }

    [JsonPropertyName("required_posting_auths")]
    public IReadOnlyList<string> RequiredPostingAuths { get; init; }

    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("json")]
    public JsonNode? Json { get; init; }
}