using System;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Parts.Recon.Domain.Config;
using FilingPortal.Parts.Recon.Domain.Entities;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseClientValue
{
    /// <summary>
    /// Represents the CargoWise Client Value Report configuration
    /// </summary>
    internal class ReportConfig : IReportConfig
    {
        /// <summary>
        /// Report name
        /// </summary>
        public string Name => ReportNames.CargoWiseClientValue;

        /// <summary>
        /// File Name
        /// </summary>
        public string FileName => "Recon.xlsx";

        /// <summary>
        /// Document title
        /// </summary>
        public string DocumentTitle => "CargoWise Client";

        /// <summary>
        /// Gets or sets the value indicating whether to show filter settings
        /// </summary>
        public bool IsFilterSettingsVisible => false;

        /// <summary>
        /// Underlying model type
        /// </summary>
        public Type ModelType => typeof(Model);

        /// <summary>
        /// The Data source model type
        /// </summary>
        public Type ModelDataType => typeof(InboundRecordReadModel);
    }
}
