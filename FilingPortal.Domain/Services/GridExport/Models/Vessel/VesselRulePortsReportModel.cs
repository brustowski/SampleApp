using System;

namespace FilingPortal.Domain.Services.GridExport.Models.Vessel
{
    /// <summary>
    /// Class describing Vessel port rule record model for reports
    /// </summary>
    public class VesselRulePortsReportModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Discharge Name
        /// </summary>
        public string DischargeName { get; set; }

        /// <summary>
        /// Gets or sets the Entry Port
        /// </summary>
        public string EntryPort { get; set; }

        /// <summary>
        /// Gets or sets the Discharge Port
        /// </summary>
        public string DischargePort { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public string FirmsCode { get; set; }

        /// <summary>
        /// Gets or sets the HMF
        /// </summary>
        public string HMF { get; set; }
        /// <summary>
        /// Gets or sets the Destination Code
        /// </summary>
        public string DestinationCode { get; set; }

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
