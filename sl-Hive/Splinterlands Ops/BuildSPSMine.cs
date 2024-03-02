using sl_Hive.Attributes;

namespace sl_Hive.Splinterlands_Ops
{
    [SMOperationAttribute(id: "sm_land_operation")]
    public  class BuildSPSMine: SMOperation
    {
        public string op { get; } = "worksite_sps_construction";
        public string region_uid { get; set; } = string.Empty;
        public string[] deed_uids { get; set; } = [];
        public int time_crystals { get; set; } = 0;
        public int project_id { get; set; } = 0;
        public bool auto_buy_grain { get; set; } = false;
    }
}
