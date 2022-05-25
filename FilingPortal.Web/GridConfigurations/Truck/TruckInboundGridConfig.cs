using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.Truck;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Truck
{
    /// <summary>
    /// Class describing the configuration for the Truck Inbound records grid
    /// </summary>
    public class TruckInboundGridConfig : GridConfiguration<TruckInboundViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.TruckInboundRecords;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "truck_import_template.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Importer).MinWidth(200).DefaultSorted();
            AddColumn(x => x.Supplier).MinWidth(200).DefaultSorted();
            AddColumn(x => x.PAPs).DisplayName("PAPs");
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Importer);
            TextFilterFor(x => x.Supplier);
            TextFilterFor(x => x.PAPs).Title("PAPs").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ImportEntryStatusFilterDataProvider>().NotSearch();
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            SelectFilterFor(x => x.HasAllRequiredRules).Title("Rule status").DataSourceFrom<ErrorStatusDataProvider>()
                .NotSearch();
        }
    }
}
