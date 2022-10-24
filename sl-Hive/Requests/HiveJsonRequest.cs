using Newtonsoft.Json;
using sl_Hive.Attributes;
using System.Reflection;

namespace sl_Hive.Requests
{
    [RPCMethod("", "")]
    public abstract class HiveJsonRequest
    {
        [JsonProperty("id")]
        public int Id { get; set; } = 1;
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; } = "2.0";
        [JsonProperty("method")]
        public virtual string Method => GetRpcMethodFromDecorator();

        private string GetRpcMethodFromDecorator()
        {
            var props = this.GetType().GetCustomAttributes();
            var types = props.Select(p => p.GetType());
            var rpcMethod = props.Where((p) => p is RPCMethod).FirstOrDefault() as RPCMethod;

            if (rpcMethod == null) return "";

            return rpcMethod.Database.Length > 0 ? rpcMethod.Database + "." + rpcMethod.Method : rpcMethod.Method;
        }
    }
}
