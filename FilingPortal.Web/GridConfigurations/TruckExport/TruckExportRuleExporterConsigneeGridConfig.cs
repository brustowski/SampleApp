using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.TruckExport;

namespace FilingPortal.Web.GridConfigurations.TruckExport
{
    /// <summary>
    /// Class describing the configuration for the Truck Export Consignee Rule grid
    /// </summary>
    public class TruckExportRuleExporterConsigneeGridConfig : RuleGridConfigurationWithUniqueFields<TruckExportRuleExporterConsigneeViewModel, TruckExportRuleExporterConsignee>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public TruckExportRuleExporterConsigneeGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.TruckExportRuleExporterConsignee;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Exporter).DisplayName("USPPI").DefaultSorted().EditableLookup().DataSourceFrom<SupplierCodeDataProvider>().Searchable();
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee ").EditableLookup().DataSourceFrom<ImporterCodeDataProvider>().Searchable();
            AddColumn(x => x.Address).DisplayName("USPPI Address").MinWidth(300)
                .EditableLookup().DataSourceFrom<ClientAddressCodeDataProvider>()
                .DependsOn<TruckExportRuleExporterConsigneeViewModel>(x => x.Exporter);
            AddColumn(x => x.Contact).DisplayName("Contact").MinWidth(200)
                .EditableLookup().DataSourceFrom<ClientContactCodeDataProvider>()
                .DependsOn<TruckExportRuleExporterConsigneeViewModel>(x => x.Exporter);
            AddColumn(x => x.Phone).DisplayName("Phone").EditableText();
            AddColumn(x => x.TranRelated).DisplayName("Tran Related").EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Exporter).Title("USPPI");
            TextFilterFor(x => x.ConsigneeCode).Title("Consignee");
            TextFilterFor(x => x.Address).Title("USPPI Address");
            TextFilterFor(x => x.Contact).Title("Contact");
            TextFilterFor(x => x.Phone).Title("Phone");
            SelectFilterFor(x => x.TranRelated).Title("Tran Related").DataSourceFrom<YesNoTextFilterDataProvider>();
        }
    }
}