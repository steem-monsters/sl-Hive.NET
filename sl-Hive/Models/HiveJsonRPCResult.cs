using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sl_Hive.Models
{
    public class HiveJsonRPCResult<ResultType>
    {
        public string Jsonrpc { get; set; } = string.Empty;
        public ResultType? Result { get; set; }
    }
}
