using Newtonsoft.Json;
using sl_Hive.Requests;
using System.Text;

namespace sl_Hive
{
    public class HiveEngine
    {
        public RPCNodeCollection RPCNodeCollection => new RPCNodeCollection();

        private readonly HttpClient httpClient;
        private int activeNode = -1;

        public HiveEngine()
        {
            httpClient = new HttpClient();
            activeNode = RPCNodeCollection.Nodes.Count() > 0 ? 0 : -1;
        }



        public async Task<HiveJsonRPCResult<ResponseType>> QueryBlockchain<ResponseType>(HiveJsonRequest request)
        {
            try
            {
                var result = "";
                string strTest = JsonConvert.SerializeObject(request);
                using (var rawResponse = await httpClient.PostAsync(
                    RPCNodeCollection.Nodes.ToList()[activeNode].Url, 
                    new StringContent(strTest, 
                    Encoding.UTF8, 
                    "application/json")
                    ))
                {
                    rawResponse.EnsureSuccessStatusCode();
                    result = await rawResponse.Content.ReadAsStringAsync();
                }

                var hiveResponse = JsonConvert.DeserializeObject<HiveJsonRPCResult<ResponseType>>(result);
                return hiveResponse;
            }
            catch (Exception ex)
            {
                RotateActiveNode();
                throw new Exception($"Unable to process request: {ex.Message}");
            }
        }

        private void RotateActiveNode()
        {
            activeNode = activeNode < RPCNodeCollection.Nodes.Count() ? activeNode + 1 : 0;
        }
    }
}
