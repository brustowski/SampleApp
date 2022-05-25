using FilingPortal.Domain.Services;
using FilingPortal.Parts.Rail.Export.Domain.Config;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Web.Models;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;

namespace FilingPortal.Parts.Rail.Export.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the Consignee Rule grid
    /// </summary>
    public class RuleConsigneeGridConfig : RuleGridConfigurationWithUniqueFields<RuleConsigneeViewModel, RuleConsignee>
    {
        /// <summary>
        /// Creates a new instance of <see cref="RuleConsigneeGridConfig"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RuleConsigneeGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RuleConsignee;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee").MinWidth(200).DefaultSorted().EditableLookup().DataSourceFrom<ImporterCodeDataProvider>().KeyField(x => x.ConsigneeCode).Searchable();
            AddColumn(x => x.Country).DisplayName("Country of Destination").MinWidth(150).EditableLookup().DataSourceFrom<CountryCodeDataProvider>().Searchable();
            AddColumn(x => x.Destination).DisplayName("Destination").MinWidth(150).EditableText();
            AddColumn(x => x.UltimateConsigneeType).DisplayName("Ultimate Consignee Type").EditableLookup().DataSourceFrom<ConsigneeTypeLookupDataProvider>().MinWidth(200);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ConsigneeCode).Title("Consignee");
            TextFilterFor(x => x.Country).Title("Country of Destination");
            TextFilterFor(x => x.Destination).Title("Destination");
            TextFilterFor(x => x.UltimateConsigneeType).Title("Ultimate Consignee Type");
        }
    }
}