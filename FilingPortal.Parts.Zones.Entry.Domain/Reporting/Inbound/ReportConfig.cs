using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Zones.Entry.Domain.Config;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using System;

namespace FilingPortal.Parts.Zones.Entry.Domain.Reporting.Inbound
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
        public string FileName => "ZonesEntry.xlsx";

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle => "ZonesEntry";

        /// <summary>
        /// Gets or sets the value indicating whether to show filter settings
        /// </summary>
        public bool IsFilterSettingsVisible => true;

        /// <summary>
        /// Underlying model type
        /// </summary>
        public Type ModelType => typeof(InboundReportModel);

        /// <summary>
        /// The Data source model type
        /// </summary>
        public Type ModelDataType => typeof(InboundReadModel);
    }
}
