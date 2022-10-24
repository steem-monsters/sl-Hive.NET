using Newtonsoft.Json;
using sl_Hive.Requests;
using System.Text;

namespace sl_Hive
{
    public class HiveEngine
    {
        private readonly HttpClient _httpClient;
        private readonly IReadOnlyList<RPCNode> _nodes;
        private int _currentNode;

        public HiveEngine(HttpClient httpClient, IReadOnlyList<RPCNode> nodes) {
            _httpClient = httpClient;
            _nodes = nodes;
        }
        
        private string GetActiveNodeUrl() {
            if( _nodes.Count == 0 ) {
                throw new InvalidOperationException("No nodes available");
            }

            return _nodes[_currentNode].Url;
        }

        private void NextNode() {
            _currentNode = (_currentNode + 1) % _nodes.Count;
        }


        public async Task<HiveJsonRPCResult<TResponseType>> QueryBlockchain<TResponseType>(HiveJsonRequest request) {
            try {
                var requestText = JsonConvert.SerializeObject(request);
                using var rawResponse = await _httpClient.PostAsync(
                    GetActiveNodeUrl(),
                    new StringContent(requestText,
                        Encoding.UTF8,
                        "application/json")
                );
                rawResponse.EnsureSuccessStatusCode();
                var text = await rawResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<HiveJsonRPCResult<TResponseType>>(text);
            }
            catch( Exception ex ) {
                NextNode();
                throw new Exception($"Unable to process request: {ex.Message}");
            }
        }
    }
}
