using System.Text.Json.Serialization;
using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    [RpcMethod("condenser_api", "get_accounts")]
    public class AccountsRequest : HiveJsonRequest
    {
        [JsonPropertyName("params")]
        public IReadOnlyList<IReadOnlyList<string>> Accounts { get; init; } = default!;
    }
}
