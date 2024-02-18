using sl_Hive.Attributes;

namespace sl_Hive.Splinterlands_Ops
{
    public class Plot
    {
        public string plotId { get; set; } = string.Empty;
    }
    [SMOperationAttribute(id: "sm_survey_land")]
    public class SurveyLand : SMOperation
    {
        public Plot[] plotsToSurvey { get; set; } = [];
    }
}
