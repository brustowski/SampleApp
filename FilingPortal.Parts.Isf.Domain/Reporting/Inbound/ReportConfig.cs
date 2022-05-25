using System;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Isf.Domain.Config;
using FilingPortal.Parts.Isf.Domain.Entities;

namespace FilingPortal.Parts.Isf.Domain.Reporting.Inbound
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
        public string FileName => "ISF.xlsx";

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle => "ISF";

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
