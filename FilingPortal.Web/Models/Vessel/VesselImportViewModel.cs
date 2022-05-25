using System.Collections.Generic;
using FilingPortal.PluginEngine.Models;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Web.Models.Vessel
{
    /// <summary>
    /// Defines the Vessel Inbound record item View Model
    /// </summary>
    [TsInterface(IncludeNamespace = false, AutoI = false, FlattenHierarchy = true)]
    public class VesselImportViewModel : FilingRecordModelWithActionsOld, IModelWithStringValidation
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
        /// Gets or sets the discharge terminal
        /// </summary>
        public string ProductDescription { get; set; }
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
        public string CustomsQty { get; set; }
        /// <summary>
        /// Gets or sets the Unit Price
        /// </summary>
        public string UnitPrice { get; set; }
        /// <summary>
        /// Gets or sets the Owner Ref
        /// </summary>
        public string OwnerRef { get; set; }
        /// <summary>
        /// Gets or sets Country of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// Validation errors
        /// </summary>
        public List<string> Errors { get; set; }
    }
}