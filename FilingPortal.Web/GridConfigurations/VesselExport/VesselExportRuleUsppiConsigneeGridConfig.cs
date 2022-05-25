using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.VesselExport;

namespace FilingPortal.Web.GridConfigurations.VesselExport
{
    /// <summary>
    /// Class describing the configuration for the Vessel Export Consignee Rule grid
    /// </summary>
    public class VesselExportRuleUsppiConsigneeGridConfig : RuleGridConfigurationWithUniqueFields<VesselExportRuleUsppiConsigneeViewModel, VesselExportRuleUsppiConsignee>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public VesselExportRuleUsppiConsigneeGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.VesselExportRuleUsppiConsignee;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Usppi).DisplayName("USPPI").DefaultSorted()
                .EditableLookup().DataSourceFrom<SupplierDataProvider>().KeyField(x => x.UsppiId);
            AddColumn(x => x.Consignee).DisplayName("Consignee")
                .EditableLookup().DataSourceFrom<ImporterDataProvider>().KeyField(x => x.ConsigneeId);
            AddColumn(x => x.TransactionRelated).DisplayName("Transaction Related")
                .EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>().MinWidth(300);
            AddColumn(x => x.UltimateConsigneeType).DisplayName("Ultimate Consignee Type")
                .EditableLookup().DataSourceFrom<ConsigneeTypeLookupDataProvider>().MinWidth(200);
            AddColumn(x => x.ConsigneeAddress).DisplayName("Consignee Address")
                .EditableLookup().DataSourceFrom<ClientAddressDataProvider>()
                .DependsOn<VesselExportRuleUsppiConsigneeViewModel>(x => x.Consignee)
                .KeyField(x => x.ConsigneeAddressId);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Usppi).Title("USPPI");
            TextFilterFor(x => x.Consignee).Title("Consignee");
            TextFilterFor(x => x.TransactionRelated).Title("Transaction Related");
            TextFilterFor(x => x.UltimateConsigneeType).Title("Ultimate Consignee Type");
            TextFilterFor(x => x.ConsigneeAddress).Title("Consignee Address");
        }
    }
}