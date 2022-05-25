using FilingPortal.Parts.Zones.Ftz214.Domain.Config;
using FilingPortal.Parts.Zones.Ftz214.Web.Models;
using FilingPortal.PluginEngine.DataProviders.FilterProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Zones.Ftz214.Web.GridConfigs
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
            AddColumn(x => x.Applicant).DisplayName("Applicant").MinWidth(200).DefaultSorted();
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(200);
            AddColumn(x => x.Ein).DisplayName("EIN");
            AddColumn(x => x.FtzOperator).DisplayName("FTZ Operator");
            AddColumn(x => x.AdmissionNo).DisplayName("Admission Number").FixWidth(160);
            AddColumn(x => x.AdmissionYear).DisplayName("Admission Year").FixWidth(160);
            AddColumn(x => x.ZoneId).DisplayName("Zone Id");
            AddColumn(x => x.AdmissionType).DisplayName("Admission Type").FixWidth(160);

            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.ModifiedDate).DisplayName("Modified Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            SelectFilterFor(x => x.Applicant).Title("Applicant Name").DataSourceFrom<ClientCodeWithEinDataProvider>().SetOperand(FilterOperands.Equal);
            SelectFilterFor(x => x.FtzOperator).Title("Operator Name").DataSourceFrom<ClientCodeWithEinDataProvider>().SetOperand(FilterOperands.Equal);
            TextFilterFor(x => x.ZoneId).Title("Zone Id").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.AdmissionType).Title("Admission Type").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.FilingNumber).Title("Job #").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Record Created Date");
            DateRangeFilterFor(x => x.ModifiedDate).Title("Record Modified Date");
            SelectFilterFor(x => x.ModifiedUser).Title("Record Modified User").DataSourceFrom<UpdatedByUserTruckExportFilterDataProvide>().NotSearch();
            DateRangeFilterFor(x => x.EntryCreatedDate).Title("Entry Created Date");
            DateRangeFilterFor(x => x.EntryModifiedDate).Title("Entry Modified Date");
            SelectFilterFor(x => x.IsAuto).Title("Filing Method").DataSourceFrom<FileUploadMethodFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.ValidationPassed).Title("Validation Passed").DataSourceFrom<ValidationPassedFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.JobStatus).Title("Job Status").DataSourceFrom<JobStatusFilterDataProvider>().NotSearch().IsMultiSelect();
            SelectFilterFor(x => x.EntryStatus).Title("Status").DataSourceFrom<ExportEntryStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.Applicant).Title("Importer").DataSourceFrom<ClientCodeWithEinDataProvider>().SetOperand(FilterOperands.Equal);
        }
    }
}
