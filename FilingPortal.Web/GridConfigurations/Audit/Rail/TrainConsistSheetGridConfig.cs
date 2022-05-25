using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.Audit.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Audit.Rail
{
    /// <summary>
    /// Class describing the configuration for the Audit Rail Train Consist Sheet grid
    /// </summary>
    public class TrainConsistSheetGridConfig : GridConfiguration<TrainConsistSheetViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.AuditRailTrainConsistSheet;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "consist_sheet_import_template.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.EntryNumber).DisplayName("Entry number");
            AddColumn(x => x.BillNumber).DisplayName("Bill number");
            AddColumn(x => x.Status);
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date");
            AddColumn(x => x.CreatedUser).DisplayName("Created By");
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.EntryNumber).Title("Entry Number").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.BillNumber).Title("Bill Number").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Creation Date");
            SelectFilterFor(x => x.Status).DataSourceFrom<AuditRailTrainConsistSheetStatusDataProvider>().NotSearch();
        }
    }
}