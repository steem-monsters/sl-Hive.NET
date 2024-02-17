using sl_Hive.Attributes;

namespace sl_Hive.Splinterlands_Ops
{
    [SMOperationAttribute(id: "sm_stake_tokens")]
    public class StakeTokens: SMOperation
    {
        public uint qty { get; set; } = 0;
        public string token { get; set; } = "SPS";
    }
}
