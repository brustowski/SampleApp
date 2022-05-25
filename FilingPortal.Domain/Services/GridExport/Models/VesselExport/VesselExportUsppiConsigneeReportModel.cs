using System;

namespace FilingPortal.Domain.Services.GridExport.Models.VesselExport
{
    /// <summary>
    /// Class describing Vessel USPPI-Consignee Rule record model for reports
    /// </summary>
    public class VesselExportUsppiConsigneeReportModel
    {
        /// <summary>
        /// Rule Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the USPPI
        /// </summary>
        public string Usppi { get; set; }
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public string Consignee { get; set; }
        /// <summary>
        /// Gets or sets the transactions related
        /// </summary>
        public string TransactionRelated { get; set; }

        /// <summary>
        /// Get or sets Ultimate consignee type
        /// </summary>
        public string UltimateConsigneeType { get; set; }
        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public string ConsigneeAddress { get; set; }
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
