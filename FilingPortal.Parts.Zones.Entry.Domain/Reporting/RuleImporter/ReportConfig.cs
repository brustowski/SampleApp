using System;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Zones.Entry.Domain.Config;

namespace FilingPortal.Parts.Zones.Entry.Domain.Reporting.RuleImporter
{
    /// <summary>
    /// Importer rule report configuration
    /// </summary>
    internal class ReportConfig : IReportConfig
    {
        /// <summary>
        /// Grid name
        /// </summary>
        public string Name => GridNames.ImporterRuleGrid;

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName => "ZonesEntryImporterRules.xlsx";

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle => "ZonesEntryImporterRules";

        /// <summary>
        /// Gets or sets the value indicating whether to show filter settings
        /// </summary>
        public bool IsFilterSettingsVisible => true;

        /// <summary>
        /// Underlying model type
        /// </summary>
        public Type ModelType => typeof(RuleImporterReportModel);

        /// <summary>
        /// The Data source model type
        /// </summary>
        public Type ModelDataType => typeof(RuleImporterReportModel);
    }
}
