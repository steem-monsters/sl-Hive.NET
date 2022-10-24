﻿using sl_Hive.Requests;
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


        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        public async Task<HiveJsonRPCResult<TResponseType>> QueryBlockchain<TResponseType>(HiveJsonRequest request) {
            try {
                var requestText = JsonSerializer.Serialize<object>(request, _options);
                using var rawResponse = await _httpClient.PostAsync(
                    GetActiveNodeUrl(),
                    new StringContent(requestText,
                        Encoding.UTF8,
                        "application/json")
                );
                rawResponse.EnsureSuccessStatusCode();
                var text = await rawResponse.Content.ReadAsStringAsync();

                if( string.IsNullOrEmpty(text) ) {
                    throw new Exception("Unexpected Json response");
                }

                return JsonSerializer.Deserialize<HiveJsonRPCResult<TResponseType>>(text, _options)!;
            }
            catch( Exception ex ) {
                NextNode();
                throw new Exception($"Unable to process request: {ex.Message}");
            }
        }
    }
}
