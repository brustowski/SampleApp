using FilingPortal.Parts.Zones.Entry.Domain.Config;
using FilingPortal.Parts.Zones.Entry.Web.Models;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Zones.Entry.Web.GridConfigs
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
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(200).DefaultSorted();
            AddColumn(x => x.Ein).DisplayName("EIN");
            AddColumn(x => x.EntryPort).DisplayName("Entry Port");
            AddColumn(x => x.ArrivalDate).DisplayName("Arrival Date");
            AddColumn(x => x.OwnerRef).DisplayName("Owner Ref");
            AddColumn(x => x.FirmsCode).DisplayName("FIRMs Code");
            AddColumn(x => x.EntryNo).DisplayName("Entry #");
            AddColumn(x => x.VesselName).DisplayName("Vessel Name");

          
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.ModifiedDate).DisplayName("Modified Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            SelectFilterFor(x => x.Importer).Title("Importer Name").DataSourceFrom<ClientCodeWithEinDataProvider>().SetOperand(FilterOperands.Equal);
            TextFilterFor(x => x.EntryNo).Title("Entry #").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.OwnerRef).Title("Owner Ref").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.VesselName).Title("Vessel Name").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.FilingNumber).Title("Job #").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Record Created Date");
            DateRangeFilterFor(x => x.ModifiedDate).Title("Record Modified Date");
            SelectFilterFor(x => x.ModifiedUser).Title("Record Modified User").DataSourceFrom<UpdatedByUserTruckExportFilterDataProvide>().NotSearch();
            DateRangeFilterFor(x => x.EntryCreatedDate).Title("Entry Created Date");
            DateRangeFilterFor(x => x.EntryModifiedDate).Title("Entry Modified Date");
            SelectFilterFor(x => x.IsAuto).Title("Filing Method").DataSourceFrom<FileUploadMethodFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.ValidationPassed).Title("Validation Passed").DataSourceFrom<ValidationPassedFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.JobStatus).Title("Job Status").DataSourceFrom<JobStatusFilterDataProvider>().NotSearch().IsMultiSelect();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ExportEntryStatusFilterDataProvider>().NotSearch();
        }
    }
}
