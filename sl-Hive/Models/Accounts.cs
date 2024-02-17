using System.Text.Json.Serialization;

namespace sl_Hive.Models
{
    public class Accounts
    {
        public long Id { get; set; } = -1;
        public string Name { get; set; }
        public KeyCollection? Owner { get; set; }
        public KeyCollection? Active { get; set; }

        [JsonPropertyName("posting")]
        public KeyCollection? Posting { get; set; }

        [JsonPropertyName("memo_key")]
        public string MemoKey { get; set; }

        [JsonPropertyName("json_metadata")]
        public string JsonMetadata { get; set; }

        public string Posting_Json_Metadata { get; set; }
        public string Proxy { get; set; }
        public DateTime? Previous_Owner_Update { get; set; }
        public DateTime? Last_Owner_Update { get; set; }
        public DateTime? Last_Account_Update { get; set; }
        public DateTime? Created { get; set; }
        public bool Mined { get; set; }
        public string Recovery_Account { get; set; }
        public DateTime? Last_Account_Recovery { get; set; }
        public string? Reset_Account { get; set; }
        public int Comment_Count { get; set; }
        public int Lifetime_Vote_Count { get; set; }
        public int Post_Count { get; set; }
        public bool Can_Vote { get; set; }
        public ManaBar? Voting_Manabar { get; set; }
        public ManaBar? Downvote_Manabar { get; set; }
        public int Voting_Power { get; set; }
        public string Balance { get; set; }
        public string Savings_Balance { get; set; }
        public string Hbd_Balance { get; set; }
        public string Hbd_Seconds { get; set; }
        public DateTime? Hhbd_Seconds_Last_Update { get; set; }
        public DateTime? Hbd_Last_Interest_Payment { get; set; }
        public string savings_hbd_balance { get; set; }
        public string savings_hbd_seconds { get; set; }
        public DateTime? Savings_Hbd_Seconds_Last_Update { get; set; }
        public DateTime? Savings_Hbd_Last_Interest_Payment { get; set; }
        public int Savings_Withdraw_Requests { get; set; }
        public string Reward_Hbd_Balance { get; set; }
        public string Reward_Hive_Balance { get; set; }
        public string Reward_Vesting_BBalance { get; set; }
        public string Reward_Vesting_Hive { get; set; }
        public string Vesting_Shares { get; set; }
        public string Delegated_Vesting_Shares { get; set; }
        public string Received_Vesting_Shares { get; set; }
        public string Vesting_Withdraw_Rate { get; set; }
        public string post_voting_power { get; set; }
        public DateTime? Next_Vesting_Withdrawal { get; set; }
        public long Withdrawn { get; set; }
        public long To_Withdraw { get; set; }
        public int Withdraw_Routes { get; set; }
        public int Pending_Transfers { get; set; }
        public int Curation_Rewards { get; set; }
        public int Posting_Rewards { get; set; }
        public IReadOnlyList<int> Proxied_Vsf_Votes { get; set; }
        public int Witnesses_Voted_For { get; set; }
        public DateTime? Last_Post { get; set; }
        public DateTime? Last_Root_Post { get; set; }
        public DateTime? Last_Vote_Time { get; set; }
        public int Post_Bandwidth { get; set; }
        public int Pending_Claimed_Accounts { get; set; }
        public DateTime? Governance_Vote_Expiration_Ts { get; set; }
        public IReadOnlyList<DelayedVote> Delayed_Votes { get; set; } = Array.Empty<DelayedVote>();
        public int Open_Recurrent_Transfers { get; set; }
        public string Vesting_Balance { get; set; }
        public int Reputation { get; set; }
        public IReadOnlyList<string> Transfer_History { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Market_History { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Post_History { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Vote_History { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Other_History { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Witness_Votes { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Tags_Usage { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Guest_Bloggers { get; set; } = Array.Empty<string>();
    }
}
