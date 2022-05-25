using System;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Inbond.Domain.Config;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.Domain.Reporting.Inbound
{
    internal class ReportConfig : IReportConfig
    {
        /// <summary>
        /// Grid name
        /// </summary>
        public string Name => GridNames.InbondRecords;

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName => "Inbond.xlsx";

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle => "Inbond";

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
