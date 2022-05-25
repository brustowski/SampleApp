using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Vessel
{
    /// <summary>
    /// Defines the Vessel Rule Product View model
    /// </summary>
    public class VesselRuleProductViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets the Tariff
        /// </summary>
        public string Tariff { get; set; }

        /// <summary>
        /// Gets or sets the Goods Description
        /// </summary>
        public string GoodsDescription { get; set; }

        /// <summary>
        /// Gets or sets the Customs Attribute 1
        /// </summary>
        public string CustomsAttribute1 { get; set; }

        /// <summary>
        /// Gets or sets the Customs Attirbute 2
        /// </summary>
        public string CustomsAttribute2 { get; set; }

        /// <summary>
        /// Gets or sets the Invoice UQ
        /// </summary>
        public string InvoiceUQ { get; set; }

        /// <summary>
        /// Gets or sets the TSCA Requirement
        /// </summary>
        public string TSCARequirement { get; set; }
    }
}