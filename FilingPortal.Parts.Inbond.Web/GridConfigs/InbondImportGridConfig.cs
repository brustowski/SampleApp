using FilingPortal.Parts.Inbond.Domain.Config;
using FilingPortal.Parts.Inbond.Web.Common.Providers;
using FilingPortal.Parts.Inbond.Web.Models;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.Inbond.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the Inbond import records grid
    /// </summary>
    public class InbondImportGridConfig : GridConfiguration<InbondViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.InbondRecords;

        /// <summary>
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "inbond_import_template.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.FirmsCode).DisplayName("FIRMS Code");
            AddColumn(x => x.ImporterCode).DisplayName("Importer").MinWidth(120);
            AddColumn(x => x.PortOfArrival).DisplayName("Port of Arrival").MinWidth(120);
            AddColumn(x => x.PortOfDestination).DisplayName("Port of Destination").MinWidth(120);
            AddColumn(x => x.ExportConveyance).DisplayName("Conveyance").MinWidth(120);
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee").MinWidth(120);
            AddColumn(x => x.Carrier).DisplayName("In-Bond Carrier").MinWidth(120);
            AddColumn(x => x.Value).DisplayName("Value").MinWidth(120);
            AddColumn(x => x.ManifestQty).DisplayName("Manifest Quantity").MinWidth(120);
            AddColumn(x => x.ManifestQtyUnit).DisplayName("Manifest Quantity Unit").MinWidth(120);
            AddColumn(x => x.Weight).DisplayName("Weight").MinWidth(120);
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.FirmsCode).Title("FIRMS Code").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.ImporterCode).Title("Importer").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.PortOfArrival).Title("Port of Arrival").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.PortOfDestination).Title("Port of Destination").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.ExportConveyance).Title("Conveyance").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.ConsigneeCode).Title("Consignee").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.Carrier).Title("Carrier").SetOperand(FilterOperands.Contains);
            NumberFilterFor(x => x.Value).Title("Value");
            NumberFilterFor(x => x.ManifestQty).Title("Manifest Quantity");
            TextFilterFor(x => x.ManifestQtyUnit).Title("Manifest Quantity Unit");
            NumberFilterFor(x => x.Weight).Title("Weight");
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<InBondEntryStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.HasAllRequiredRules).Title("Rule status").DataSourceFrom<ErrorStatusDataProvider>()
                .NotSearch();
        }
    }
}
