using System;
using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Domain.Entities.Vessel
{
    /// <summary>
    /// Defines the Vessel Import Inbound record item
    /// </summary>
    public class VesselImportRecord : InboundEntity<VesselImportFilingHeader>
    {
        /// <summary>
        /// Gets or sets Importer ID
        /// </summary>
        public Guid ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public virtual Client Importer { get; set; }
        /// <summary>
        /// Gets or sets Supplier ID
        /// </summary>
        public Guid? SupplierId { get; set; }
        /// <summary>
        /// Gets or sets the Supplier
        /// </summary>
        public virtual Client Supplier { get; set; }
        /// <summary>
        /// Gets or sets the Vessel id
        /// </summary>
        public int VesselId { get; set; }
        /// <summary>
        /// Gets or sets the Vessel name
        /// </summary>
        public virtual VesselHandbookRecord Vessel { get; set; }
        /// <summary>
        /// Gets or sets State ID
        /// </summary>
        public int? StateId { get; set; }
        /// <summary>
        /// Gets or sets the State
        /// </summary>
        public virtual USStates State { get; set; }
        /// <summary>
        /// Gets or sets the FIRMs code id
        /// </summary>
        public int FirmsCodeId { get; set; }
        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public CargowiseFirmsCodes FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets Classification ID
        /// </summary>
        public int ClassificationId { get; set; }
        /// <summary>
        /// Gets or sets the Classification
        /// </summary>
        public virtual HtsTariff Classification { get; set; }
        /// <summary>
        /// Gets or sets Product Description ID
        /// </summary>
        public int ProductDescriptionId { get; set; }
        /// <summary>
        /// Gets or sets the Product Description
        /// </summary>
        public virtual ProductDescriptionHandbookRecord ProductDescription { get; set; }
        /// <summary>
        /// Gets or sets the ETA
        /// </summary>
        public DateTime Eta { get; set; }
        /// <summary>
        /// Gets or sets filer id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the Filer ID
        /// </summary>
        public virtual AppUsersModel Filer { get; set; }
        /// <summary>
        /// Gets or sets the container
        /// </summary>
        public string Container { get; set; }
        /// <summary>
        /// Gets or sets the Entry Type
        /// </summary>
        public string EntryType { get; set; }
        /// <summary>
        /// Gets or sets the Customs Quantity
        /// </summary>
        public decimal CustomsQty { get; set; }
        /// <summary>
        /// Gets or sets the Unit Price
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// Gets or sets the Country of Origin Id
        /// </summary>
        public int? CountryOfOriginId { get; set; }
        /// <summary>
        /// Gets or sets the Country of Origin
        /// </summary>
        public virtual Country CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets the Owner Ref
        /// </summary>
        public string OwnerRef { get; set; }
        /// <summary>
        /// Gets or sets the collection of documents
        /// </summary>
        public virtual ICollection<VesselImportDocument> Documents { get; set; }
    }
}
