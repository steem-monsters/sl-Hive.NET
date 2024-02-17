using sl_Hive.Requests;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

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


        public static readonly JsonSerializerOptions _options = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            IncludeFields = true,

        };

        public async Task<HiveJsonRPCResult<TResponseType>> QueryBlockchain<TResponseType>(HiveJsonRequest request) {
            try {
                var jsonRequest = JsonSerializer.Serialize<object>(request, _options);

                using var rawResponse = await _httpClient.PostAsync(
                    GetActiveNodeUrl(),
                    new StringContent(jsonRequest,
                        Encoding.UTF8,
                        "application/json")
                );
                rawResponse.EnsureSuccessStatusCode();
                var response = await rawResponse.Content.ReadAsStringAsync();

                if( string.IsNullOrEmpty(response) ) {
                    throw new Exception("Unexpected Json response");
                }

                return JsonSerializer.Deserialize<HiveJsonRPCResult<TResponseType>>(response, _options)!;
            }
            catch( Exception ex ) {
                NextNode();
                throw new Exception($"Unable to process request: {ex.Message}");
            }
        }
    }
}
