namespace sl_Hive.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RPCMethod : Attribute
    {
        public string Database { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;

        public RPCMethod(string database, string method)
        {
            Database = database;
            Method = method;
        }
    }
}
