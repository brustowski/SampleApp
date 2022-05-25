using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Models.Vessel;

namespace FilingPortal.Web.GridConfigurations.Vessel
{
    /// <summary>
    /// Provides the configuration for the Vessel Product Rule grid
    /// </summary>
    public class VesselRuleProductGridConfiguration : RuleGridConfigurationWithUniqueFields<VesselRuleProductViewModel, VesselRuleProduct>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public VesselRuleProductGridConfiguration(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.VesselRuleProduct;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Tariff).DisplayName("Tariff").MinWidth(150).DefaultSorted().EditableText();
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").MinWidth(200).EditableText();
            AddColumn(x => x.CustomsAttribute1).DisplayName("Customs Attribute 1").MinWidth(200).EditableText();
            AddColumn(x => x.CustomsAttribute2).DisplayName("Customs Attribute 2").MinWidth(150).EditableText();
            AddColumn(x => x.InvoiceUQ).DisplayName("Invoice UQ").MinWidth(150).EditableText();
            AddColumn(x => x.TSCARequirement).DisplayName("TSCA Requirement").MinWidth(150).EditableText();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Tariff).Title("Tariff");
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").Advanced();
            TextFilterFor(x => x.CustomsAttribute1).Title("Customs Attribute 1").Advanced();
            TextFilterFor(x => x.CustomsAttribute2).Title("Customs Attribute 2").Advanced();
            TextFilterFor(x => x.InvoiceUQ).Title("Invoice UQ").Advanced();
            TextFilterFor(x => x.TSCARequirement).Title("TSCA Requirement").Advanced();
        }
    }
}
