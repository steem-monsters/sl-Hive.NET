using sl_Hive.Requests;
using System.Text.Json.Serialization;

namespace sl_Hive.Splinterlands_Ops
{
    [JsonDerivedType(typeof(UpdateWorksite))]
    [JsonDerivedType(typeof(BuildResearchHut))]
    [JsonDerivedType(typeof(BuildSPSMine))]
    [JsonDerivedType(typeof(BuildGrainFarm))]
    [JsonDerivedType(typeof(DoResearch))]
    [JsonDerivedType(typeof(MinePlot))]
    [JsonDerivedType(typeof(HarvestPlot))]
    [JsonDerivedType(typeof(TransferResources))]
    [JsonDerivedType(typeof(DecPowerdownRegion))]
    [JsonDerivedType(typeof(DecPowerupRegion))]
    [JsonDerivedType(typeof(SurveyLand))]
    [JsonDerivedType(typeof(CombineTotemFragments))]
    [JsonDerivedType(typeof(RedeemTotem))]
    [JsonDerivedType(typeof(StakeTokens))]
    public abstract class SMOperation
    {
        public string app { get; set; } = "sl-hive";
        public string n { get; set; } = string.Empty;
    }
}
