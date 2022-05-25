using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.Pipeline;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Pipeline
{
    /// <summary>
    /// Class describing the configuration for the Pipeline Inbound records grid
    /// </summary>
    public class PipelineInboundRecordsGridConfig : GridConfiguration<PipelineViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.PipelineInboundRecords;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "pipeline_import_template.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(200).DefaultSorted();
            AddColumn(x => x.Batch).DisplayName("Batch");
            AddColumn(x => x.TicketNumber).DisplayName("Ticket #");
            AddColumn(x => x.Facility).DisplayName("Facility");
            AddColumn(x => x.Quantity).DisplayName("Quantity");
            AddColumn(x => x.API).DisplayName("API");
            AddColumn(x => x.EntryNumber).DisplayName("Entry Number");
            AddColumn(x => x.ExportDate).DisplayName("Export Date").FixWidth(120);
            AddColumn(x => x.ImportDate).DisplayName("Import Date").FixWidth(120);
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Importer).Title("Importer");
            TextFilterFor(x => x.Batch).Title("Batch").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.TicketNumber).Title("Ticket Number").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.Facility).Title("Facility").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ImportEntryStatusFilterDataProvider>().NotSearch();
            FloatNumberFilterFor(x => x.Quantity).Title("Quantity").Advanced();
            FloatNumberFilterFor(x => x.API).Title("API").Advanced();
            TextFilterFor(x => x.EntryNumber).Title("Entry Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.HasAllRequiredRules).Title("Rule status").DataSourceFrom<ErrorStatusDataProvider>()
                .NotSearch();
        }
    }
}