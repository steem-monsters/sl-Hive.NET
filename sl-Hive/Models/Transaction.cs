namespace sl_Hive.Models
{
    public class Transaction
    {
        public required ushort ref_block_num;
        public required UInt32 ref_block_prefix;
        public required DateTime expiration;
        public required object[] operations;
        public required object[] extensions = Array.Empty<object>();
        public required string[] signatures = Array.Empty<string>();
    }
}
