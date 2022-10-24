using Newtonsoft.Json;
using sl_Hive.Attributes;
using System.Reflection;

namespace sl_Hive.Requests
{
    [RpcMethod("", "")]
    public abstract class HiveJsonRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 1;
        
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; } = "2.0";
        
        [JsonProperty("method")]
        public virtual string Method => GetCurrentRpcMethod();

        private string GetCurrentRpcMethod() {
            var rpcMethod = GetType().GetCustomAttribute<RpcMethodAttribute>(false);

            if (rpcMethod is null) return string.Empty;

            return rpcMethod.Database.Length > 0 ? rpcMethod.Database + "." + rpcMethod.Method : rpcMethod.Method;
        }
    }
}
