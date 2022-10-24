using System.Text;

namespace sl_Hive
{
    public class Memo
    {
        public string MemoPrefix { get; set; } = "#";
        public string AddressPrefix { get; set; } = "STM";

        public Memo()
        {

        }

        public Memo(string memoPrefix, string addressPrefix)
        {
            MemoPrefix = memoPrefix;
            AddressPrefix = addressPrefix;
        }

        public string Encode(string memo, string publicKey, string privateKey)
        {
            if (memo == null || memo.Length == 0) throw new ArgumentException("Invalid memo");
            if (publicKey == null || publicKey.Length == 0 || !publicKey.StartsWith(AddressPrefix)) throw new ArgumentException("Invalid public key");
            if (privateKey == null || privateKey.Length == 0) throw new ArgumentException("Invalid private key");

            if (memo.StartsWith(MemoPrefix))
            {
                memo = memo.Substring(MemoPrefix.Length);
            }

            var bytes = Encoding.ASCII.GetBytes(memo);



            return "";
        }

        private string Decode(string memo, string privateKey)
        {
            if (memo == null || memo.Length == 0) throw new ArgumentException("Invalid memo");
            if (privateKey == null || privateKey.Length == 0) throw new ArgumentException("Invalid private key");
            return "";
        }
    }
}
