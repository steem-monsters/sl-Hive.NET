namespace sl_Hive.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class RpcMethodAttribute : Attribute
	{
		public string Database { get; }
		public string Method { get; }

		public RpcMethodAttribute(string database, string method) {
			Database = database;
			Method = method;
		}
	}
}
