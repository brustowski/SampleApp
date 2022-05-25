using Reinforced.Typings.Attributes;

namespace FilingPortal.Web.PageConfigs.Common
{
    /// <summary>
    /// Class describing page configuration names
    /// </summary>
    [TsClass(IncludeNamespace = false)]
    internal class PageConfigNames
    {
        /// <summary>
        /// The single inbound record configuration name
        /// </summary>
        public const string SingleInboundRecordConfigName = "RailBdParsedActionsConfig";

        /// <summary>
        /// The inbound record list configuration name
        /// </summary>
        public const string InboundRecordListConfigName = "InboundRecordListActionsConfig";

        /// <summary>
        /// The Filtered Rail records actions provider name
        /// </summary>
        public const string FilteredRailRecordsActionsConfig = "FilteredRailRecordsActionsConfig";

        /// <summary>
        /// The inbound rail rule record configuration name
        /// </summary>
        public const string RailRuleConfigName = "RailRuleActionsConfiguration";

        /// <summary>
        /// The Rail Rules page actions configuration name
        /// </summary>
        [TsProperty]
        public const string RailRulesPageActions = "RailRulesPageActions";

        /// <summary>
        /// The Rail page actions configuration name
        /// </summary>
        [TsProperty]
        public const string RailViewPageActions = "RailViewPageActions";

        /// <summary>
        /// The single truck inbound record configuration name
        /// </summary>
        public const string TruckInboundActions = "TruckInboundActionsConfiguration";

        /// <summary>
        /// The truck inbound record list configuration name
        /// </summary>
        public const string TruckListInboundActions = "TruckInboundListActionsConfiguration";

        /// <summary>
        /// The inbound truck rule record configuration name
        /// </summary>
        [TsProperty]
        public const string TruckRuleConfigName = "TruckRuleActionsConfiguration";

        /// <summary>
        /// The Truck Rules page actions configuration name
        /// </summary>
        [TsProperty]
        public const string TruckRulesPageActions = "TruckRulesPageActions";

        /// <summary>
        /// The Truck View page actions configuration name
        /// </summary>
        [TsProperty]
        public const string TruckViewPageActions = "TruckViewPageActions";

        /// <summary>
        /// The Configuration record  actions configuration name
        /// </summary>
        public const string ConfigurationActionsConfigName = "ConfigurationActionsConfiguration";

        /// <summary>
        /// The Configuration page actions configuration name
        /// </summary>
        [TsProperty]
        public const string ConfigurationPageActions = "ConfigurationPageActions";

        /// <summary>
        /// The Pipeline View page actions configuration name
        /// </summary>
        [TsProperty]
        public const string PipelineViewPageActions = "PipelineViewPageActions";

        /// <summary>
        /// The Pipeline rule record configuration name
        /// </summary>
        [TsProperty]
        public const string PipelineRuleConfigName = "PipelineRuleActionsConfiguration";
        /// <summary>
        /// The Pipeline Rules page actions configuration name
        /// </summary>
        [TsProperty]
        public const string PipelineRulesPageActions = "PipelineRulesPageActions";
        /// <summary>
        /// The single pipeline inbound record configuration name
        /// </summary>
        [TsProperty]
        public const string PipelineInboundActions = "PipelineInboundActionsConfiguration";

        /// <summary>
        /// The Pipeline inbound record list configuration name
        /// </summary>
        [TsProperty]
        public const string PipelineListInboundActions = "PipelineInboundListActionsConfiguration";

        /// <summary>
        /// The Vessel Rule record Configuration name
        /// </summary>
        [TsProperty]
        public const string VesselRuleConfigName = "VesselRuleActionsConfiguration";

        /// <summary>
        /// The Vessel Rule Page ActionConfiguration name
        /// </summary>
        [TsProperty]
        public const string VesselRulesPageActions = "VesselRulePageAction";

        /// <summary>
        /// The single vessel import record configuration name
        /// </summary>
        [TsProperty]
        public const string VesselImportActions = "VesselImportActionsConfiguration";

        /// <summary>
        /// The vessel inbound record list configuration name
        /// </summary>
        [TsProperty]
        public const string VesselListImportActions = "VesselImportListActionsConfiguration";
        /// <summary>
        /// The Vessel View page actions configuration name
        /// </summary>
        [TsProperty]
        public const string VesselViewPageActions = "VesselViewPageActions";

        /// <summary>
        /// The Truck Export Actions Configuration name
        /// </summary>
        [TsProperty]
        public const string TruckExportActions = "TruckExportActionsConfiguration";

        /// <summary>
        /// The Truck Exports View page actions configuration name
        /// </summary>
        [TsProperty]
        public const string TruckExportViewPageActions = "TruckExportViewPageActions";
        /// <summary>
        /// The truck export rule record configuration name
        /// </summary>
        public const string TruckExportRuleConfigName = "TruckExportRuleActionsConfiguration";

        /// <summary>
        /// The Truck Export Rules page actions configuration name
        /// </summary>
        [TsProperty]
        public const string TruckExportRulesPageActions = "TruckExportRulesPageActions";

        /// <summary>
        /// The Default value records configuration name
        /// </summary>
        [TsProperty]
        public const string DefValueActionsConfigName = "DefValueActionsConfiguration";
        /// <summary>
        /// The Vessel Export Actions Configuration name
        /// </summary>
        [TsProperty]
        public const string VesselExportActions = "VesselExportActionsConfiguration";
        /// <summary>
        /// The Vessel Export records configuration name
        /// </summary>
        [TsProperty]
        public const string VesselExportListActions = "VesselExportListActionsConfiguration";
        /// <summary>
        /// The Vessel Export View page actions configuration name
        /// </summary>
        [TsProperty]
        public const string VesselExportViewPageActions = "VesselExportViewPageActions";

        /// <summary>
        /// The Vessel Export Rule record Configuration name
        /// </summary>
        [TsProperty]
        public const string VesselExportRuleConfigName = "VesselExportRuleActionsConfiguration";

        /// <summary>
        /// The Vessel Export Rule Page Action Configuration name
        /// </summary>
        [TsProperty]
        public const string VesselExportRulesPageActions = "VesselExportRulePageAction";
        #region Audit
        /// <summary>
        /// The Audit Rail Train Consist Sheet Action Configuration name
        /// </summary>
        [TsProperty]
        public const string AuditRailTrainConsistSheetPageActions = "AuditRailTrainConsistSheetPageActions";
        /// <summary>
        /// The inbound rail rule record configuration name
        /// </summary>
        public const string AuditRailDailyAuditRulesConfigName = "AuditRailDailyAuditRulesConfigName";
        /// <summary>
        /// The rail daily audit page action configuration name
        /// </summary>
        [TsProperty]
        public const string AuditRailDailyAuditRulesPageActions = "AuditRailDailyAuditRulesPageActions";

        #endregion
        #region Admin
        /// <summary>
        /// The Admin Rules Page Action Configuration name
        /// </summary>
        [TsProperty]
        public const string AdminRulesPageActions = "AdminRulesPageActions";
        #endregion
    }
}