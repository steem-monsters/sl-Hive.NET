using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    [RpcMethod("database_api", "get_dynamic_global_properties")]
    public class HiveDynamicGlobalPropertiesRequest : HiveJsonRequest
    {
    }
}
