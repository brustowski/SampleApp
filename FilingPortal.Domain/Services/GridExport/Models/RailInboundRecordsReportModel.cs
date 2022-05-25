using FilingPortal.Domain.Enums;
using System;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.Services.GridExport.Models
{
    /// <summary>
    /// Class describing Rail Inbound record model for reports
    /// </summary>
    public class RailInboundRecordsReportModel
    {
        /// <summary>
        /// Record identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Supplier
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Train number
        /// </summary>
        public string TrainNumber { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Bill of Lading number
        /// </summary>
        public string BOLNumber { get; set; }
        /// <summary>
        /// Gets or sets the Issuer Code
        /// </summary>
        public string IssuerCode { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Container Number
        /// </summary>
        public string ContainerNumber { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Port code
        /// </summary>
        public string PortCode { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record HTS
        /// </summary>
        public string HTS { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Creation date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Filing Number
        /// </summary>
        public string FilingNumber { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Mapping status
        /// </summary>
        public MappingStatus? MappingStatus { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record Filing Status
        /// </summary>
        public FilingStatus? FilingStatus { get; set; }
        /// <summary>
        /// Gets or sets the Rail Inbound record deleted status
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
