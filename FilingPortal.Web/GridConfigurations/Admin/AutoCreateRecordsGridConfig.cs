using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Models.Admin;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Admin
{
    /// <summary>
    /// Class describing the grid configuration for the Auto-create records grid
    /// </summary>
    public class AutoCreateRecordsGridConfig : RuleGridConfigurationWithUniqueFields<AutoCreateRecordViewModel, AutoCreateRecord>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.AutoCreateRecords;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "admin_auto_create_template.xlsx";

        /// <summary>
        /// Configures editable rule columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.ShipmentType).DisplayName("Shipment Type").MinWidth(200).EditableLookup().DataSourceFrom<ShipmentTypeDataProvider>().DefaultSorted();
            AddColumn(x => x.EntryType).DisplayName("Entry Type").EditableLookup().DataSourceFrom<EntryTypeDataProvider>();
            AddColumn(x => x.TransportMode).DisplayName("Transport Mode").EditableLookup().DataSourceFrom<TransportModeCodesDataProvider>();
            AddColumn(x => x.ImporterExporter).DisplayName("Importer/Exporter").EditableLookup().DataSourceFrom<ClientCodeDataProvider>();
            AddColumn(x => x.AutoCreate).DisplayName("Auto-Create").EditableBoolean();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            SelectFilterFor(x => x.ShipmentType).Title("Shipment Type").DataSourceFrom<ShipmentTypeDataProvider>()
                .NotSearch();
            TextFilterFor(x => x.EntryType).Title("Entry Type").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.TransportMode).Title("Transport Mode")
                .DataSourceFrom<TransportModeCodesDataProvider>().NotSearch();
            TextFilterFor(x => x.ImporterExporter).Title("Importer/Exporter").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.AutoCreate).Title("Auto-Create").DataSourceFrom<YesNoFilterDataProvider>().NotSearch();
        }

        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public AutoCreateRecordsGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }
    }
}