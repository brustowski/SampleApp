using System;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Entities.Vessel
{
    /// <summary>
    /// Defines model for vessels list representation
    /// </summary>
    public class VesselImportReadModel : InboundReadModelOld
    {
        /// <summary>
        /// Gets or sets the Importer code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets the Supplier code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Gets or sets the Vessel name
        /// </summary>
        public string Vessel { get; set; }
        /// <summary>
        /// Gets or sets the State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the FIRMs Code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets the Classification
        /// </summary>
        public string Classification { get; set; }
        /// <summary>
        /// Gets or sets the Classification
        /// </summary>
        public string ProductDescription { get; set; }
        /// <summary>
        /// Gets or sets the ETA
        /// </summary>
        public DateTime Eta { get; set; }
        /// <summary>
        /// Gets or sets the Filer ID
        /// </summary>
        public string FilerId { get; set; }
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
        /// Gets or sets the Country of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets the Owner Ref
        /// </summary>
        public string OwnerRef { get; set; }
        /// <summary>
        /// Gets or sets the model Mapping Status title
        /// </summary>
        public string MappingStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets the model Filing status title
        /// </summary>
        public string FilingStatusTitle { get; set; }
        /// <summary>
        /// Gets or sets whether this record has port rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasPortRule { get; set; }
        /// <summary>
        /// Gets or sets whether this record has product rule. 1 - rule set, 0 - rule not set
        /// </summary>
        public int HasProductRule { get; set; }
        /// <summary>
        /// Determines whether initial record data can be edited
        /// </summary>
        public virtual bool CanEditInitialRecord() => 
            !FilingHeaderId.HasValue || (MappingStatus == Parts.Common.Domain.Enums.MappingStatus.Open && FilingStatus == Parts.Common.Domain.Enums.FilingStatus.Open);
    }
}
