using sl_Hive.Attributes;

namespace sl_Hive.Requests
{
    [RPCMethod("database_api", "get_dynamic_global_properties")]
    public class HiveDynamicGlobalPropertiesRequest : HiveJsonRequest
    {
    }
}
