using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Zones.Ftz214.Domain.Config;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using System;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Reporting.Inbound
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
        public string FileName => "ZonesFtz214.xlsx";

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle => "ZonesFtz214";

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
