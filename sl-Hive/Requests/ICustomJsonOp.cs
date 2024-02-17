using System.Text.Json.Serialization;

namespace sl_Hive.Requests
{   

    
    public class TokenType
    {
        public uint qty { get; set; } = 0;
        public string token { get; set; } = "SPS";
    }
    
    public class FindMatchOp
    {
        public string match_type { get; set; } = string.Empty;
        public string app { get; set; } = "usb/1.0";
        public string n { get; set; } = "9gq1SBPKGW";
    }
}
