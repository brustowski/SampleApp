using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Entities.Vessel;
using Framework.Domain;
using System;
using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Domain.Entities.VesselExport
{
    /// <summary>
    /// Describes the Vessel Export Entity
    /// </summary>
    public class VesselExportRecord : InboundEntity<VesselExportFilingHeader>
    {
        /// <summary>
        /// Gets or sets USPPI Id
        /// </summary>
        public Guid UsppiId { get; set; }
        /// <summary>
        /// Gets or sets USPPI
        /// </summary>
        public virtual Client Usppi { get; set; }
        /// <summary>
        /// Gets or sets the address id
        /// </summary>
        public Guid? AddressId { get; set; }
        /// <summary>
        /// Gets or sets the address
        /// </summary>
        public virtual ClientAddress Address { get; set; }
        /// <summary>
        /// Gets or sets the contact id
        /// </summary>
        public Guid? ContactId { get; set; }
        /// <summary>
        /// Gets or sets Contact
        /// </summary>
        public virtual ClientContact Contact { get; set; }
        /// <summary>
        /// Gets or sets the phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Gets or sets Importer ID
        /// </summary>
        public Guid ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public virtual Client Importer { get; set; }
        /// <summary>
        /// Gets or sets the Vessel id
        /// </summary>
        public int VesselId { get; set; }
        /// <summary>
        /// Gets or sets the Vessel name
        /// </summary>
        public virtual VesselHandbookRecord Vessel { get; set; }
        /// <summary>
        /// Gets or sets the Export Date
        /// </summary>
        public DateTime ExportDate { get; set; }
        /// <summary>
        /// Gets or sets the Load Port
        /// </summary>
        public string LoadPort { get; set; }
        /// <summary>
        /// Gets or sets the Discharge Port
        /// </summary>
        public string DischargePort { get; set; }
        /// <summary>
        /// Gets or sets the Country of Destination Identifier
        /// </summary>
        public int CountryOfDestinationId { get; set; }
        /// <summary>
        /// Gets or sets the Country of Destination
        /// </summary>
        public virtual Country CountryOfDestination { get; set; }
        /// <summary>
        /// Gets or sets the Tariff Type
        /// </summary>
        public string TariffType { get; set; }
        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string Tariff { get; set; }
        /// <summary>
        /// Gets or sets the Goods Description
        /// </summary>
        public string GoodsDescription { get; set; }
        /// <summary>
        /// Gets or sets the Origin Indicator
        /// </summary>
        public string OriginIndicator { get; set; }
        /// <summary>
        /// Gets or sets the Quantity
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Gets or sets the Weight
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Gets or sets the Transport Ref
        /// </summary>
        public string TransportRef { get; set; }
        /// <summary>
        /// Gets or sets the Container
        /// </summary>
        public string Container { get; set; }
        /// <summary>
        /// Gets or sets the In-Bond
        /// </summary>
        public string InBond { get; set; }
        /// <summary>
        /// Gets or sets the Sold En Route
        /// </summary>
        public string SoldEnRoute { get; set; }
        /// <summary>
        /// Gets or sets the Export Adjustment Value
        /// </summary>
        public string ExportAdjustmentValue { get; set; }
        /// <summary>
        /// Gets or sets the Original ITN
        /// </summary>
        public string OriginalItn { get; set; }
        /// <summary>
        /// Gets or sets the Routed Transaction
        /// </summary>
        public string RoutedTransaction { get; set; }
        /// <summary>
        /// Gets or sets the reference number
        /// </summary>
        public string ReferenceNumber { get; set; }
        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Documents
        /// </summary>
        public virtual ICollection<VesselExportDocument> Documents { get; set; }
    }
}
