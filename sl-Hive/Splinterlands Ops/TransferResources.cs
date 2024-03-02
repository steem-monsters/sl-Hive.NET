using sl_Hive.Attributes;

namespace sl_Hive.Splinterlands_Ops
{
    [SMOperationAttribute(id: "sm_resource_shipment")]
    public class TransferResources : SMOperation
    {
        /// <summary>
        /// Op can be either 'load_balance' or 'resource_shipment'
        /// </summary>
        public string op { get; set; } = string.Empty;
        public string from_region_uid { get; set; } = string.Empty;
        public string to_region_uid { get; set; } = string.Empty;
        public string to_player { get; set; } = string.Empty;
        public double amount { get; set; } = 0.0d;
        public string resource_symbol { get; set; } = string.Empty;
    }
}
