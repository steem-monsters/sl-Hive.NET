using sl_Hive.Attributes;

namespace sl_Hive.Splinterlands_Ops
{
    [SMOperationAttribute(id: "sm_land_operation")]
    public class DoResearch: SMOperation
    {
        public string op { get; } = "research";
        public string region_uid { get; set; } = string.Empty;
        public string[] plot_uids { get; set; } = [];
        public bool auto_buy_grain { get; set; } = false;
    }
}
