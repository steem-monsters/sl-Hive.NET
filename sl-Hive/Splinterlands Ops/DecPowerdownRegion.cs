using sl_Hive.Attributes;

namespace sl_Hive.Splinterlands_Ops
{
    [SMOperationAttribute(id: "sm_dec_powerdown_region")]
    public class DecPowerdownRegion : SMOperation
    {
        public int amount { get; set; } = 0;
        public string region_uid { get; set; } = string.Empty;
    }
}
