using FilingPortal.Domain.Services;
using FilingPortal.Parts.Rail.Export.Domain.Config;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Web.Models;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.GridConfigurations.FilterProviders;

namespace FilingPortal.Parts.Rail.Export.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the Consignee Rule grid
    /// </summary>
    public class RuleExporterConsigneeGridConfig : RuleGridConfigurationWithUniqueFields<RuleExporterConsigneeViewModel, RuleExporterConsignee>
    {
        /// <summary>
        /// Creates a new instance of <see cref="RuleExporterConsigneeGridConfig"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RuleExporterConsigneeGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RuleExporterConsignee;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Exporter).DisplayName("USPPI").DefaultSorted().EditableLookup().DataSourceFrom<SupplierCodeDataProvider>().Searchable();
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee ").EditableLookup().DataSourceFrom<ImporterCodeDataProvider>().Searchable();
            AddColumn(x => x.Contact).DisplayName("Contact").MinWidth(200).EditableText();
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
            TextFilterFor(x => x.Contact).Title("Contact");
            TextFilterFor(x => x.Phone).Title("Phone");
            SelectFilterFor(x => x.TranRelated).Title("Tran Related").DataSourceFrom<YesNoTextFilterDataProvider>();
        }
    }
}