namespace sl_Hive;

public class RPCNodeCollection
{
	public readonly IReadOnlyList<RPCNode> Nodes = new[] {
		new RPCNode("https://hived.splinterlands.com"),
		new RPCNode("https://api.hive.blog"),
		new RPCNode("https://anyx.io")
	};
}