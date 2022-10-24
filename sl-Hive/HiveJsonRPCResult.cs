namespace sl_Hive
{
    public class HiveJsonRPCResult<ResultType>
    {
        public string Jsonrpc { get; set; } = string.Empty;
        public ResultType? Result { get; set; }
    }
}
