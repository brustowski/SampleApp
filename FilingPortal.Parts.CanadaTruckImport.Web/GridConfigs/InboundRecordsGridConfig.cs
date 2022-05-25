using FilingPortal.Parts.CanadaTruckImport.Domain.Config;
using FilingPortal.Parts.CanadaTruckImport.Web.Common.Providers;
using FilingPortal.Parts.CanadaTruckImport.Web.Models;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using Framework.Domain.Paging;

namespace FilingPortal.Parts.CanadaTruckImport.Web.GridConfigs
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
        /// Gets the name of the corresponding template file name
        /// </summary>
        public override string TemplateFileName => "canada_truck_import_template.xlsx";

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.Vendor).DisplayName("Vendor").MinWidth(120);
            AddColumn(x => x.Port).MinWidth(120);
            AddColumn(x => x.ParsNumber).DisplayName("PARS#").MinWidth(120);
            AddColumn(x => x.Eta).DisplayName("ETA").MinWidth(120);
            AddColumn(x => x.OwnersReference).DisplayName("Owners Reference").MinWidth(120);
            AddColumn(x => x.GrossWeight).DisplayName("Gross Weight").MinWidth(120);
            AddColumn(x => x.DirectShipDate).DisplayName("Direct Ship Date").MinWidth(120);
            AddColumn(x => x.Consignee).DisplayName("Consignee").MinWidth(120);
            AddColumn(x => x.Product).DisplayName("Product").MinWidth(120);
            AddColumn(x => x.InvoiceQty).DisplayName("Invoice Quantity").MinWidth(120);
            AddColumn(x => x.LinePrice).DisplayName("Line Price").MinWidth(120);
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Vendor).Title("Vendor").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.Port).SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.ParsNumber).Title("PARS#").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.Eta).Title("ETA");
            TextFilterFor(x => x.OwnersReference).Title("Owners Reference").SetOperand(FilterOperands.Contains);
            FloatNumberFilterFor(x => x.GrossWeight).Title("Gross Weight").SetOperand(FilterOperands.Equal);
            DateRangeFilterFor(x => x.DirectShipDate).Title("Direct Ship Date");
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            TextFilterFor(x => x.Consignee).Title("Consignee").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.Product).Title("Product").SetOperand(FilterOperands.Contains);
            FloatNumberFilterFor(x => x.InvoiceQty).Title("Invoice Qty").SetOperand(FilterOperands.Equal);
            FloatNumberFilterFor(x => x.LinePrice).Title("Line Price").SetOperand(FilterOperands.Equal);
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ImportTruckCanadaEntryStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.HasAllRequiredRules).Title("Rule status").DataSourceFrom<ErrorStatusDataProvider>()
                .NotSearch();
        }
    }
}
