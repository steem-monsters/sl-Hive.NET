using Newtonsoft.Json;
using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    [RpcMethod("condenser_api", "get_accounts")]
    public class AccountsRequest : HiveJsonRequest
    {
        [JsonProperty("params")]
        public IReadOnlyList<IReadOnlyList<string>> Accounts { get; init; } = default!;
    }
}
