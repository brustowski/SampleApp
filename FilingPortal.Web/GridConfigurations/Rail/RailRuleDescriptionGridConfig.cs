using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Rail;

namespace FilingPortal.Web.GridConfigurations.Rail
{
    /// <summary>
    /// Class describing the configuration for the Rail Description Rule grid
    /// </summary>
    public class RailRuleDescriptionGridConfig : RuleGridConfigurationWithUniqueFields<RailRuleDescriptionViewModel, RailRuleDescription>
    {

        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RailRuleDescriptionGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RailRuleDescription;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Description1).DisplayName("Description 1").MinWidth(500).EditableText().DefaultSorted();
            AddColumn(x => x.Importer).DisplayName("Importer Name").MinWidth(200)
                .EditableLookup().DataSourceFrom<RailImportersFromRuleDataProvider>();
            AddColumn(x => x.Supplier).DisplayName("Supplier Name").MinWidth(200)
                .EditableLookup().DataSourceFrom<RailSuppliersFromRuleDataProvider>();
            AddColumn(x => x.Port).MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<PortDataProvider>();
            AddColumn(x => x.Destination).MinWidth(130).Searchable()
                .EditableLookup().DataSourceFrom<StatesDataProvider>();
            AddColumn(x => x.ProductID).DisplayName("Product ID").MinWidth(150).EditableText();
            AddColumn(x => x.Attribute1).DisplayName("Attribute 1").MinWidth(200).EditableText();
            AddColumn(x => x.Attribute2).DisplayName("Attribute 2").MinWidth(200).EditableText();
            AddColumn(x => x.Tariff).MinWidth(150).Searchable()
                .EditableLookup().DataSourceFrom<TariffDataProvider>();
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description").MinWidth(200).EditableText();
            AddColumn(x => x.InvoiceUOM).DisplayName("Invoice UOM").Searchable()
                .EditableLookup().DataSourceFrom<UnitsDataProvider>();
            AddColumn(x => x.TemplateHTSQuantity).DisplayName("Template HTS Quantity").EditableFloatNumber();
            AddColumn(x => x.TemplateInvoiceQuantity).DisplayName("Template Invoice Quantity").EditableFloatNumber();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Description1).Title("Description 1");
            TextFilterFor(x => x.Importer).Title("Importer");
            TextFilterFor(x => x.Supplier).Title("Supplier");
            TextFilterFor(x => x.Port).Title("Port");
            TextFilterFor(x => x.Destination);
            TextFilterFor(x => x.ProductID).Title("Product ID").Advanced();
            TextFilterFor(x => x.Attribute1).Title("Attribute 1").Advanced();
            TextFilterFor(x => x.Attribute2).Title("Attribute 2").Advanced();
            TextFilterFor(x => x.Tariff).Title("Tariff").Advanced();
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").Advanced();
            TextFilterFor(x => x.InvoiceUOM).Title("Invoice UOM").Advanced();
            FloatNumberFilterFor(x => x.TemplateHTSQuantity).Title("Template HTS Quantity").Advanced();
            FloatNumberFilterFor(x => x.TemplateInvoiceQuantity).Title("Template Invoice Quantity").Advanced();
        }
    }
}