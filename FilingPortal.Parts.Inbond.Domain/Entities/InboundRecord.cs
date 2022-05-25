using System;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Inbond.Domain.Entities
{
    /// <summary>
    /// Inbond import record 
    /// </summary>
    public class InboundRecord : InboundEntity<FilingHeader>
    {
        /// <summary>
        /// Gets or sets the FIRMs Code Id
        /// </summary>
        public int FirmsCodeId { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public virtual CargowiseFirmsCodes FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets Importer ID
        /// </summary>
        public Guid ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public virtual Client Importer { get; set; }
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
        public DateTime EntryDate { get; set; } = DateTime.Now;
        /// <summary>
        /// Gets or sets conveyance
        /// </summary>
        public string ExportConveyance { get; set; }
        /// <summary>
        /// Gets or sets Consignee ID
        /// </summary>
        public Guid ConsigneeId { get; set; }
        /// <summary>
        /// Gets or sets the Consignee
        /// </summary>
        public virtual Client Consignee { get; set; }
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
    }
}
