using sl_Hive.Models;
using sl_Hive.Requests;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace sl_Hive
{
    public static class CustomJsonWrapperExtgension
    {
        /// <summary>
        /// Converts a Transaction to CustomJsonParams for broadcasting to Hive
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        public static CustomJsonParams ToParams(this Transaction wrapper)
        {
            var ops = new JsonArray(JsonSerializer.SerializeToNode("custom_json"));
            foreach (var item in wrapper.operations)
            {
                ops.Add(JsonSerializer.SerializeToNode(item, HiveEngine._options));
            }

            var jsonParams = new CustomJsonParams()
            {
                ref_block_num = wrapper.ref_block_num,
                ref_block_prefix = wrapper.ref_block_prefix,
                expiration = wrapper.expiration,
                operations = new JsonArray() { ops },
                signatures = wrapper.signatures,
            };

            return jsonParams;
        }
    }
}
