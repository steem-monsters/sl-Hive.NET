using sl_Hive.Attributes;

namespace sl_Hive.Models
{
    [RPCMethod("database_api", "get_dynamic_global_properties")]
    public class HiveDynamicGlobalProperties
    {
        public Int64 Id { get; set; } = -1;
        public Int64 Head_Block_Number { get; set; } = -1;
        public string Head_Block_Id { get; set; } = string.Empty;
        public DateTime Time { get; set; } = DateTime.Now;
        public string Current_Witness { get; set; } = string.Empty;
        public Int64 Total_Pow { get; set; } = -1;
        public Int64 Num_Pow_Witnesses { get; set; } = -1;
        public TokenSupply? Virtual_Supply { get; set; } = null;
        public TokenSupply? Current_Supply { get; set; } = null;
        public TokenSupply? Init_Hbd_Supply { get; set; } = null;
        public TokenSupply? Current_Hbd_Supply { get; set; } = null;
        public TokenSupply? Total_Vesting_Fund_Hive { get; set; } = null;
        public TokenSupply? Total_Vesting_Shares { get; set; } = null;
        public TokenSupply? Total_Reward_Fund_Hive { get; set; } = null;
        public string Total_Reward_Shares2 { get; set; } = string.Empty;
        public TokenSupply? Pending_Rewarded_Vesting_Shares { get; set; } = null;
        public TokenSupply? Pending_Rewarded_Vesting_Hive { get; set; } = null;
        public int Hbd_Iinterest_Rate { get; set; } = 0;
        public int Hbd_Print_Rate { get; set; } = 0;
        public int Maximum_Block_Size { get; set; } = 0;
        public int Required_Actions_Partition_Percent { get; set; } = 0;
        public Int64 Current_Aslot { get; set; } = -1;
        public string Recent_Slots_Filled { get; set; } = string.Empty;
        public int Participation_Count { get; set; } = 0;
        public Int64 Last_Irreversible_Block_Num { get; set; } = -1;
        public int Vote_Power_Reserve_Rate { get; set; } = 0;
        public int Delegation_Return_Period { get; set; } = 0;
        public int Reverse_Auction_Seconds { get; set; } = 0;
        public int Available_Account_Subsidies { get; set; } = 0;
        public int Hbd_Stop_Percent { get; set; } = 0;
        public int Hbd_Start_Percent { get; set; } = 0;
        public DateTime? Next_Maintenance_Time { get; set; } = null;
        public DateTime? Last_Budget_Time { get; set; } = null;
        public DateTime? Next_Daily_Maintenance_Time { get; set; } = null;
        public int Content_Reward_Percent {get;set;} = 0;
        public int Vesting_Reward_Percent { get; set; } = 0;
        public int Proposal_Fund_Percent { get; set; } = 0;
        public TokenSupply? Dhf_Interval_Ledger { get; set; } = null;
        public int Downvote_Pool_Percent { get; set; } = 0;
        public int Current_Remove_Threshold { get; set; } = 0;
        public int Early_Voting_Seconds { get; set; } = 0;
        public int Mid_Voting_Seconds { get; set; } = 0;
        public int Max_Consecutive_Recurrent_Transfer_Failures { get; set; } = 0;
        public int Max_Recurrent_Transfer_End_Date { get; set; } = 0;
        public int Min_Recurrent_Transfers_Recurrence { get; set; } = 0;
        public int Max_Open_Recurrent_Transfers { get; set; } = 0;
    }
}
