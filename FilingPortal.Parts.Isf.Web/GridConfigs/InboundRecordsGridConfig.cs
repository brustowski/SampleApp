using FilingPortal.Parts.Isf.Domain.Config;
using FilingPortal.Parts.Isf.Web.Common.Providers;
using FilingPortal.Parts.Isf.Web.Models.Inbound;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Isf.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the import records grid
    /// </summary>
    public class InboundRecordsGridConfig : GridConfiguration<InboundRecordViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.InboundRecords;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.ImporterCode).DisplayName("Importer").MinWidth(120).DefaultSorted();
            AddColumn(x => x.BuyerCode).DisplayName("Buyer").MinWidth(120);
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee").MinWidth(120);
            AddColumn(x => x.MblScacCode).DisplayName("MBL SCAC Code");
            AddColumn(x => x.Eta).DisplayName("ETA").MinWidth(120);
            AddColumn(x => x.Etd).DisplayName("ETD").MinWidth(120);
            AddColumn(x => x.OwnerRef).DisplayName("Owner Ref").MinWidth(120);
            AddColumn(x => x.SellerCode).DisplayName("Seller").MinWidth(120);
            AddColumn(x => x.ShipToCode).DisplayName("Ship To").MinWidth(120);
            AddColumn(x => x.ContainerStuffingLocationCode).DisplayName("Container stuffing location").MinWidth(120);
            AddColumn(x => x.ConsolidatorCode).DisplayName("Consolidator/Forwarder:").MinWidth(120);
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.CreatedUser).DisplayName("Created User").MinWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ImporterCode).Title("Importer").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<IsfEntryStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.CreatedUser).Title("Created User").DataSourceFrom<CreatedUsersDataProvider>().NotSearch();
        }
    }
}
