namespace sl_Hive;

public static class RPCNodeCollection
{
	public static IReadOnlyList<RPCNode> DefaultNodes { get; } = new[] {
		new RPCNode("https://hived.splinterlands.com"),
		new RPCNode("https://api.hive.blog"),
		new RPCNode("https://anyx.io")
	};
}
