using System;

namespace FilingPortal.Domain.Services.GridExport.Models.Pipeline
{
    /// <summary>
    /// Class describing Pipeline Rule Price model for reporting
    /// </summary>
    public class PipelineRulePriceReportModel
    {
        /// <summary>
        /// Rule Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Client Code
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets the Crude Type
        /// </summary>
        public string CrudeType { get; set; }
        /// <summary>
        /// Gets or sets the Facility
        /// </summary>
        public string Facility { get; set; }
        /// <summary>
        /// Gets or sets the pricing
        /// </summary>
        public decimal Pricing { get; set; }
        /// <summary>
        /// Gets of sets the Freight
        /// </summary>
        public decimal Freight { get; set; }
        /// <summary>
        /// Gets or sets the Rule Creation Date
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets the Rule Creator
        /// </summary>
        public string CreatedUser { get; set; }
    }
}
