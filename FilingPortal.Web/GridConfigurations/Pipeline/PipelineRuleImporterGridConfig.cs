using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Pipeline;

namespace FilingPortal.Web.GridConfigurations.Pipeline
{
    /// <summary>
    /// Class describing the configuration for the Pipeline Importer Rule grid
    /// </summary>
    public class PipelineRuleImporterGridConfig : RuleGridConfigurationWithUniqueFields<PipelineRuleImporterViewModel, PipelineRuleImporter>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public PipelineRuleImporterGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.PipelineRuleImporter;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.Importer).DisplayName("Importer").MinWidth(150).DefaultSorted().EditableLookup().DataSourceFrom<ImporterCodeDataProvider>().Searchable();
            AddColumn(x => x.Supplier).DisplayName("Supplier/Seller").MinWidth(150).EditableLookup().DataSourceFrom<SupplierCodeDataProvider>().Searchable();
            AddColumn(x => x.Manufacturer).DisplayName("Manufacturer").MinWidth(150).EditableText();
            AddColumn(x => x.Consignee).DisplayName("Consignee/Sold to Party/Ship to Party").MinWidth(150).EditableText();
            AddColumn(x => x.ReconIssue).DisplayName("Recon. Issue").MinWidth(150).EditableLookup().DataSourceFrom<ValueReconeDataProvider>();
            AddColumn(x => x.FTARecon).DisplayName("FTA Recon").MinWidth(150).EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.SPI).DisplayName("SPI").MinWidth(150).EditableText();
            AddColumn(x => x.TransactionRelated).DisplayName("Transaction Related").EditableLookup().DataSourceFrom<YesNoLookUpDataProvider>();
            AddColumn(x => x.CountryOfExport).DisplayName("Country of Export").MinWidth(150).EditableLookup().DataSourceFrom<CountryCodeDataProvider>().Searchable();
            AddColumn(x => x.Origin).DisplayName("Country of Origin").MinWidth(150).EditableLookup().DataSourceFrom<CountryCodeDataProvider>().Searchable();
            AddColumn(x => x.MID).DisplayName("MID").MinWidth(150).EditableText();
            AddColumn(x => x.Seller).DisplayName("Seller").MinWidth(150).EditableText();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.Importer).Title("Importer");
            TextFilterFor(x => x.Supplier).Title("Supplier/Seller");
            TextFilterFor(x => x.Manufacturer).Title("Manufacturer").Advanced();
            TextFilterFor(x => x.Consignee).Title("Consignee").Advanced();
            TextFilterFor(x => x.ReconIssue).Title("Recon. Issue").Advanced();
            TextFilterFor(x => x.FTARecon).Title("FTA Recon").Advanced();
            TextFilterFor(x => x.SPI).Title("SPI").Advanced();
            TextFilterFor(x => x.TransactionRelated).Title("Transaction Related").Advanced();
            TextFilterFor(x => x.CountryOfExport).Title("Country of Export").Advanced();
            TextFilterFor(x => x.Origin).Title("Country of Origin").Advanced();
            TextFilterFor(x => x.MID).Title("MID").Advanced();
            TextFilterFor(x => x.Seller).Title("Seller").Advanced();
        }
    }
}