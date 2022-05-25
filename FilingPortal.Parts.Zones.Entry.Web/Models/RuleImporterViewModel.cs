using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.Zones.Entry.Web.Models
{
    /// <summary>
    /// Represents Importer rule view model
    /// </summary>
    public class RuleImporterViewModel : RuleViewModelWithActions
    {
        public string ImporterId { get; set; }
        /// <summary>
        /// Gets or sets the Importer
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets RLF
        /// </summary>
        public string Rlf { get; set; } //Declaration tab
        /// <summary>
        /// Gets or sets 3461 box 29
        /// </summary>
        public string F3461Box29 { get; set; } // Declaration tab
        
        /// <summary>
        /// Gets or sets Consignee
        /// </summary>
        public string Consignee { get; set; } // Declaration and Invoice header tab
        /// <summary>
        /// Gets or sets Manufacturer
        /// </summary>
        public string Manufacturer { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Supplier
        /// </summary>
        public string Supplier { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Seller
        /// </summary>
        public string Seller { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Sold to Party
        /// </summary>
        public string SoldToParty { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Ship to party
        /// </summary>
        public string ShipToParty { get; set; } // Invoice header tab
        /// <summary>
        /// Gets or sets Goods Description
        /// </summary>
        public string GoodsDescription { get; set; } // Invoice lines tab
        /// <summary>
        /// Gets or sets Recon Issue
        /// </summary>
        public string ReconIssue { get; set; } // Misc tab
    }
}
