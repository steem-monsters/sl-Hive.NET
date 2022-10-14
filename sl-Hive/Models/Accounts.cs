using Newtonsoft.Json.Linq;

namespace sl_Hive.Models
{
    public class Accounts
    {
        public Int64 Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public KeyCollection? Owner { get; set; } = null;
        public KeyCollection? Active { get; set; } = null;
        public KeyCollection? Posting { get; set; } = null;
        public string Memo_Key { get; set; } = string.Empty;
        public string Json_Metadata { get; set; } = string.Empty;
        public string Posting_Json_Metadata { get; set; } = string.Empty;
        public string Proxy { get; set; } = string.Empty;
        public DateTime? Previous_Owner_Update { get; set; } = null;
        public DateTime? Last_Owner_Update { get; set; } = null;
        public DateTime? Last_Account_Update { get; set; } = null;
        public DateTime? Created { get; set; } = null;
        public bool Mined { get; set; } = false;
        public string Recovery_Account { get; set; } = string.Empty;
        public DateTime? Last_Account_Recovery { get; set; } = null;
        public string? Reset_Account { get; set; } = null;
        public int Comment_Count { get; set; } = 0;
        public int Lifetime_Vote_Count { get; set; } = 0;
        public int Post_Count { get; set; } = 0;
        public bool Can_Vote { get; set; } = false;
        public ManaBar? Voting_Manabar { get; set; } = null;
        public ManaBar? Downvote_Manabar { get; set; } = null;
        public int Voting_Power { get; set; } = 0;
        public string Balance { get; set; } = string.Empty;
        public string Savings_Balance { get; set; } = string.Empty;
        public string Hbd_Balance { get; set; } = string.Empty;
        public string Hbd_Seconds { get; set; } = string.Empty;
        public DateTime? Hhbd_Seconds_Last_Update { get; set; } = null;
        public DateTime? Hbd_Last_Interest_Payment { get; set; } = null;
        public string savings_hbd_balance { get; set; } = string.Empty;
        public string savings_hbd_seconds { get; set; } = string.Empty;
        public DateTime? Savings_Hbd_Seconds_Last_Update { get; set; } = null;
        public DateTime? Savings_Hbd_Last_Interest_Payment { get; set; } = null;
        public int Savings_Withdraw_Requests { get; set; } = 0;
        public string Reward_Hbd_Balance { get; set; } = string.Empty;
        public string Reward_Hive_Balance { get; set; } = string.Empty;
        public string Reward_Vesting_BBalance { get; set; } = string.Empty;
        public string Reward_Vesting_Hive { get; set; } = string.Empty;
        public string Vesting_Shares { get; set; } = string.Empty;
        public string Delegated_Vesting_Shares { get; set; } = string.Empty;
        public string Received_Vesting_Shares { get; set; } = string.Empty;
        public string Vesting_Withdraw_Rate { get; set; } = string.Empty;
        public string post_voting_power { get; set; } = string.Empty;
        public DateTime? Next_Vesting_Withdrawal { get; set; } = null;
        public int Withdrawn { get; set; } = 0;
        public int To_Withdraw { get; set; } = 0;
        public int Withdraw_Routes { get; set; } = 0;
        public int Pending_Transfers { get; set; } = 0;
        public int Curation_Rewards { get; set; } = 0;
        public int Posting_Rewards { get; set; } = 0;
        public List<int> Proxied_Vsf_Votes { get; set; } = new List<int>();
        public int Witnesses_Voted_For { get; set; } = 0;
        public DateTime? Last_Post { get; set; } = null;
        public DateTime? Last_Root_Post { get; set; } = null;
        public DateTime? Last_Vote_Time { get; set; } = null;
        public int Post_Bandwidth { get; set; } = 0;
        public int Pending_Claimed_Accounts { get; set; } = 0;
        public DateTime? Governance_Vote_Expiration_Ts { get; set; } = null;
        public List<DelayedVote> Delayed_Votes { get; set; } = new List<DelayedVote>();
        public int Open_Recurrent_Transfers { get; set; } = 0;
        public string Vesting_Balance { get; set; } = string.Empty;
        public int Reputation { get; set; } = 0;
        public List<string> Transfer_History { get; set; } = new List<string>();
        public List<string> Market_History {get;set;} = new List<string>();
        public List<string> Post_History { get; set; } = new List<string>();
        public List<string> Vote_History { get; set; } = new List<string>();
        public List<string> Other_History { get; set; } = new List<string>();
        public List<string> Witness_Votes { get; set; } = new List<string>();
        public List<string> Tags_Usage { get; set; } = new List<string>();
        public List<string> Guest_Bloggers { get; set; } = new List<string>();
    }
}
