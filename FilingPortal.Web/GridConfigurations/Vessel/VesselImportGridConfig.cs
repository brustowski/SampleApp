using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.Vessel;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Vessel
{
    /// <summary>
    /// Class describing the configuration for the Vessel Import records grid
    /// </summary>
    public class VesselImportGridConfig : GridConfiguration<VesselImportViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.VesselImportRecords;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.ImporterCode).DisplayName("Importer").MinWidth(150).DefaultSorted();
            AddColumn(x => x.SupplierCode).DisplayName("Supplier").MinWidth(150);
            AddColumn(x => x.Vessel).MinWidth(200);
            AddColumn(x => x.CustomsQty).DisplayName("Customs Qty");
            AddColumn(x => x.UnitPrice).DisplayName("Unit Price");
            AddColumn(x => x.CountryOfOrigin).DisplayName("Country of Origin");
            AddColumn(x => x.OwnerRef).DisplayName("Owner Ref");
            AddColumn(x => x.State);
            AddColumn(x => x.FirmsCode).MinWidth(155).DisplayName("FIRMs Code");
            AddColumn(x => x.Classification);
            AddColumn(x => x.ProductDescription).MinWidth(160).DisplayName("Product Description");
            AddColumn(x => x.Eta).DisplayName("ETA").FixWidth(120);
            AddColumn(x => x.FilerId).DisplayName("Filer ID").NotSortable();
            AddColumn(x => x.Container);
            AddColumn(x => x.EntryType).DisplayName("Entry Type");
            AddColumn(x => x.CreatedDate).DisplayName("Creation Date").FixWidth(120);
            AddColumn(x => x.FilingNumber).DisplayName("Job #").FixWidth(175);
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ImporterCode).Title("Importer");
            TextFilterFor(x => x.SupplierCode).Title("Supplier");
            TextFilterFor(x => x.Vessel).Title("Vessel").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
            TextFilterFor(x => x.FilingNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            SelectFilterFor(x => x.MappingStatus).Title("Mapping Status").DataSourceFrom<MappingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.FilingStatus).Title("Job Status").DataSourceFrom<FilingStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.EntryStatus).Title("Entry Status").DataSourceFrom<ImportEntryStatusFilterDataProvider>().NotSearch();
            SelectFilterFor(x => x.HasAllRequiredRules).Title("Rule status").DataSourceFrom<ErrorStatusDataProvider>()
                .NotSearch();
        }
    }
}
