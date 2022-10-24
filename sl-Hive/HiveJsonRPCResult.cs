namespace sl_Hive
{
    public class HiveJsonRPCResult<TResultType>
    {
        public string Jsonrpc { get; set; } = string.Empty;
        public TResultType? Result { get; set; }
    }
}
