namespace sl_Hive;

public class RPCNode
{
    public RPCNode(string url) {
        Url = url ?? throw new ArgumentNullException(nameof(url));
    }
        
    public string Url { get; }
}