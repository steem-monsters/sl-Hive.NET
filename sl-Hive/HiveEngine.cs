using Newtonsoft.Json;
using sl_Hive.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            activeNode = RPCNodeCollection.nodes.Count() > 0 ? 0 : -1;
        }



        public async Task<HiveJsonRPCResult<ResponseType>> QueryBlockchain<ResponseType>(HiveJsonRequest request)
        {
            try
            {
                var result = "";
                var val = request.Method;

                string strTest = JsonConvert.SerializeObject(request);
                using (var oResponse = await httpClient.PostAsync(RPCNodeCollection.nodes.ToList()[activeNode].Url, new StringContent(strTest, System.Text.Encoding.UTF8, "application/json")))
                {
                    oResponse.EnsureSuccessStatusCode();
                    result = await oResponse.Content.ReadAsStringAsync();
                }

                var hiveResponse = JsonConvert.DeserializeObject<HiveJsonRPCResult<ResponseType>>(result);
                return hiveResponse;
            } 
            catch(Exception ex)
            {
                RotateActiveNode();
                throw new Exception($"Unable to process request: {ex.Message}");
            }
        }

        private void RotateActiveNode()
        {
            activeNode = activeNode < RPCNodeCollection.nodes.Count() ? activeNode + 1 : 0;
        }
    }
}
