using System;
using FilingPortal.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Defines model for Inbond Import list representation
    /// </summary>
    public class InboundReadModel : InboundReadModelOld
    {
        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets Port of Arrival
        /// </summary>
        public string PortOfArrival { get; set; }
        /// <summary>
        /// Gets or sets the Port Of Destination
        /// </summary>
        public string PortOfDestination { get; set; }
        /// <summary>
        /// Gets or sets Entry Date
        /// </summary>
        public DateTime EntryDate { get; set; }
        /// <summary>
        /// Gets or sets conveyance
        /// </summary>
        public string ExportConveyance { get; set; }
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public string ConsigneeCode { get; set; }
        /// <summary>
        /// Gets or sets the Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets value
        /// </summary>
        public decimal? Value { get; set; }
        /// <summary>
        /// Gets or sets manifest quantity
        /// </summary>
        public decimal? ManifestQty { get; set; }
        /// <summary>
        /// Gets or sets Manifest quantity unit
        /// </summary>
        public string ManifestQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets Weight
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// Gets or sets the CreatedUser
        /// </summary>
        public string CreatedUser { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the record has a corresponding Entry rule 
        /// </summary>
        public bool HasEntryRule { get; set; }
    }
}
