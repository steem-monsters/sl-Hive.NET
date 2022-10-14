using Newtonsoft.Json;
using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    [RPCMethod("condenser_api", "get_accounts")]
    public class AccountsRequest : HiveJsonRequest
    {
        [JsonProperty("params")]
        public List<List<string>> Accounts { get; set; } = new List<List<string>>();
    }
}
