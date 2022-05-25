using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Rail
{
    /// <summary>
    /// Class describing the configuration for the Inbound Records grid
    /// </summary>
    public class InboundRecordsGridConfig : GridConfiguration<InboundRecordItemViewModel>
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
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(100);
            AddColumn(x => x.Supplier).DisplayName("Supplier").MinWidth(100);
            AddColumn(x => x.TrainNumber).DisplayName("Train #").DefaultSorted().MinWidth(100);
            AddColumn(x => x.BOLNumber).DisplayName("BoL #").MinWidth(100);
            AddColumn(x => x.IssuerCode).DisplayName("Issuer Code").FixWidth(110);
            AddColumn(x => x.ContainerNumber).DisplayName("Container #").MinWidth(100);
            AddColumn(x => x.PortCode).DisplayName("Port Code").FixWidth(95);
            AddColumn(x => x.HTS).DisplayName("HTS").MinWidth(100);
            AddColumn(x => x.Destination);
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").MinWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Importer).Title("Importer");
            TextFilterFor(x => x.TrainNumber).Title("Train Number");
            TextFilterFor(x => x.BOLNumber).Title("BoL Number");
            TextFilterFor(x => x.ContainerNumber).Title("Container Number");
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ImportEntryStatusFilterDataProvider>().NotSearch();
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains).Advanced();
            TextFilterFor(x => x.IssuerCode).Title("Issuer Code").SetMaxLength(5).SetOperand(FilterOperands.Contains).Advanced();
            SelectFilterFor(x => x.Status).Title("Manifest Status").SetOperand(FilterOperands.Custom).DataSourceFrom<RailInboundStatusFilterDataProvider>()
                .NotSearch().Advanced();
            NumberFilterFor(x => x.PortCode).Title("Port Code").SetMaxLength(4).SetOperand(FilterOperands.Contains).Advanced();
            TextFilterFor(x => x.Supplier).Title("Supplier").Advanced();
            TextFilterFor(x => x.HTS).Title("HTS").Advanced();
            SelectFilterFor(x => x.HasAllRequiredRules).Title("Rule status").DataSourceFrom<ErrorStatusDataProvider>()
                .NotSearch();
        }
    }
}