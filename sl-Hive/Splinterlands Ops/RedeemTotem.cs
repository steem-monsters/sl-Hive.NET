using sl_Hive.Attributes;

namespace sl_Hive.Splinterlands_Ops
{
    [SMOperationAttribute(id: "sm_redeem_totem")]
    public class RedeemTotem : SMOperation
    {
        public int qty { get; set; } = 0;
        /// <summary>
        /// Can be one of the following values 'TOTEMC' | 'TOTEMR' | 'TOTEME' | 'TOTEML'
        /// </summary>
        public string totem { get; set; } = string.Empty;
    }
}
