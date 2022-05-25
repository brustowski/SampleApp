using System;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Rail.Export.Domain.Config;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.Domain.Reporting.Inbound
{
    internal class ReportConfig : IReportConfig
    {
        /// <summary>
        /// Grid name
        /// </summary>
        public string Name => GridNames.InboundRecords;

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName => "USExportRail.xlsx";

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle => "USExportRail";

        /// <summary>
        /// Gets or sets the value indicating whether to show filter settings
        /// </summary>
        public bool IsFilterSettingsVisible => true;

        /// <summary>
        /// Underlying model type
        /// </summary>
        public Type ModelType => typeof(InboundReadModel);

        /// <summary>
        /// The Data source model type
        /// </summary>
        public Type ModelDataType => ModelType;
    }
}
