using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleProduct
{
    /// <summary>
    /// Describes Product Rule View Model
    /// </summary>
    public class RuleProductViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets Product Code Id
        /// </summary>
        public string ProductCodeId { get; set; }
        /// <summary>
        /// Gets or sets Product Code
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Gets or sets Gross weight unit
        /// </summary>
        public string GrossWeightUnit { get; set; }
        /// <summary>
        /// Gets or sets Packages Unit
        /// </summary>
        public string PackagesUnit { get; set; }
        /// <summary>
        /// Gets or sets the Invoice UQ
        /// </summary>
        public string InvoiceUQ { get; set; }
    }
}