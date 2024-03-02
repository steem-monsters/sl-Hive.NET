using sl_Hive.Attributes;

namespace sl_Hive.Splinterlands_Ops
{
    [SMOperationAttribute(id: "sm_combine_artifacts")]
    public class CombineTotemFragments : SMOperation
    {
        public int qty { get; set; } = 0;
        /// <summary>
        /// Can be one of the following values 'TOTEMFC' | 'TOTEMFR' | 'TOTEMFE' | 'TOTEMFL'
        /// </summary>
        public string totem { get; set; } = string.Empty;
    }
}
