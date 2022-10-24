namespace sl_Hive
{
    public class RPCNodeCollection
    {
        public IEnumerable<RPCNode> nodes = new List<RPCNode>() {
            new RPCNode() {
                Url = "https://hived.splinterlands.com"
            },
            new RPCNode() {
                Url = "https://api.hive.blog"
            },
            new RPCNode() {
                Url = "https://anyx.io"
            }
        };
    }
}
