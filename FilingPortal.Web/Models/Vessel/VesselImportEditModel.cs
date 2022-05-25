using System;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Web.Models.Vessel
{
    /// <summary>
    /// Defines the Vessel Inbound record item Edit Model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true)]
    public class VesselImportEditModel
    {
        /// <summary>
        /// Vessel ID
        /// </summary>
        public int Id { get; set; } = 0;
        /// <summary>
        /// Gets or sets the Importer id
        /// </summary>
        public string ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Supplier id
        /// </summary>
        public string SupplierId { get; set; }
        /// <summary>
        /// Gets or sets the Vessel Id
        /// </summary>
        public int VesselId { get; set; }
        /// <summary>
        /// Gets or sets the State Id
        /// </summary>
        public int? StateId { get; set; }

        /// <summary>
        /// Gets or sets the Firms Code id
        /// </summary>
        public int FirmsCodeId { get; set; }
        /// <summary>
        /// Gets or sets the Classification ID
        /// </summary>
        public int ClassificationId { get; set; }
        /// <summary>
        /// Gets or sets the Classification ID
        /// </summary>
        public int ProductDescriptionId { get; set; }
        /// <summary>
        /// Gets or sets the ETA
        /// </summary>
        public virtual string Eta { get; set; }
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
        /// Gets or sets the Owner Ref
        /// </summary>
        public string OwnerRef { get; set; }
        /// <summary>
        /// Gets or sets Country of Origin
        /// </summary>
        public int CountryOfOriginId { get; set; }
    }
}