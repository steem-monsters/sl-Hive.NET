using Cryptography.ECDSA;
using Newtonsoft.Json.Linq;
using sl_Hive.Attributes;
using sl_Hive.Engine;
using sl_Hive.Models;
using sl_Hive.Requests;
using sl_Hive.Splinterlands_Ops;
using System;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace sl_Hive
{
    public class HiveEngine
    {
        private readonly HttpClient _httpClient;
        private readonly IReadOnlyList<RPCNode> _nodes;
        private int _currentNode;
        private readonly string CHAINID = "beeab0de00000000000000000000000000000000000000000000000000000000";

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

        public async Task<string> BroadcastCustomJson(SMOperation op, string user, string key, bool isActiveKeyRequired = false, int expirationSeconds = 30)
        {
            var response = await this.QueryBlockchain<HiveDynamicGlobalProperties>(HiveDynamicGlobalPropertiesRequest.Instance);
            var request = new CustomJsonTx();

            var opId = op.GetType().GetCustomAttribute<SMOperationAttribute>(false);
            if (opId is null) throw new Exception("Must use SMOperationAttribute");

            op.n = HiveEngine.RandomString(8);

            var trans = new Transaction()
            {
                ref_block_num = (ushort)((ushort)response.Result?.Head_Block_Number & (ushort)0xFFFF),
                ref_block_prefix = response.Result?.Ref_Block_Prefix ?? 0,
                operations = new object[] { 
                    new custom_json()
                    {
                        id = opId.Id, // whatever operation the game should perform
                        required_posting_auths = !isActiveKeyRequired ?[user] : [], // posting key ops
                        required_auths = isActiveKeyRequired ? [user] : [],         // active key ops
                        json = JsonSerializer.Serialize(op, HiveEngine._options),
                    }
                },
                expiration = response?.Result?.Time.AddSeconds(expirationSeconds) ?? DateTime.UtcNow.AddSeconds(expirationSeconds),
                signatures = new string[0],
                extensions = new string[0],
            };
            var serializer = new SignatureSerializer();
            var msg = serializer.Serialize(trans);
            using (var memStream = new MemoryStream())
            {
                var chainIdBytes = Hex.HexToBytes(CHAINID);
                memStream.Write(chainIdBytes, 0, chainIdBytes.Length);
                memStream.Write(msg, 0, msg.Length);

                var digest = Sha256Manager.GetHash(memStream.ToArray());
                var signatureBytes = CBase58.DecodePrivateWif(key);


                trans.signatures = [Hex.ToString(Secp256K1Manager.SignCompressedCompact(digest, signatureBytes))];                
            };
            request.Params = new JsonArray(JsonSerializer.SerializeToNode(trans.ToParams(), HiveEngine._options));
            var post = await this.QueryBlockchain<JObject>(request);
            var trxId = Hex.ToString(Sha256Manager.GetHash(msg)).Substring(0, 40);
            if (post.Result != null) return trxId;
            return "ERRROR";
        }

        private static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
