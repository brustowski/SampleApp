using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Inbond.Domain.Config;
using System;

namespace FilingPortal.Parts.Inbond.Domain.Reporting.RuleEntry
{
    internal class ReportConfig : IReportConfig
    {
        /// <summary>
        /// Grid name
        /// </summary>
        public string Name => GridNames.RuleEntry;

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName => "ZonesInBondEntryRule.xlsx";

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle => "ZonesInBondEntryRule";

        /// <summary>
        /// Gets or sets the value indicating whether to show filter settings
        /// </summary>
        public bool IsFilterSettingsVisible => true;

        /// <summary>
        /// Underlying model type
        /// </summary>
        public Type ModelType => typeof(RuleEntryReportModel);

        /// <summary>
        /// The Data source model type
        /// </summary>
        public Type ModelDataType => ModelType;
    }
}
