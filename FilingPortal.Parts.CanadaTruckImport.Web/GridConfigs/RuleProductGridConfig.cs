using FilingPortal.Domain.Services;
using FilingPortal.Parts.CanadaTruckImport.Domain.Config;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Common.Providers;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleProduct;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.DataProviders.Cargowise;
using FilingPortal.PluginEngine.GridConfigurations;

namespace FilingPortal.Parts.CanadaTruckImport.Web.GridConfigs
{
    /// <summary>
    /// Class describing the configuration for the Product Rule grid
    /// </summary>
    public class RuleProductGridConfig : RuleGridConfigurationWithUniqueFields<RuleProductViewModel, RuleProduct>
    {
        /// <summary>
        /// Creates a new instance of <see cref="GridConfigurationWithUniqueFields{TModel, TEntity}"/>
        /// <param name="keyFieldsService">Key Fields Service</param>
        /// </summary>
        public RuleProductGridConfig(IKeyFieldsService keyFieldsService) : base(keyFieldsService)
        {
        }

        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.RuleProduct;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureEditableColumns()
        {
            AddColumn(x => x.ProductCode).DisplayName("Product").DefaultSorted().IsKeyField()
                .EditableLookup().DataSourceFrom<ProductCodeDataProvider>().KeyField(x => x.ProductCodeId);
            AddColumn(x => x.GrossWeightUnit).DisplayName("Gross Weight Unit")
                .EditableLookup().DataSourceFrom<UOMDatProvider>();
            AddColumn(x => x.PackagesUnit).DisplayName("Packages Unit")
                .EditableLookup().DataSourceFrom<PacksUnitOfMeasureDataProvider>();
            AddColumn(x => x.InvoiceUQ).DisplayName("Invoice UQ")
                .EditableLookup().DataSourceFrom<UnitsDataProvider>();
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            TextFilterFor(x => x.ProductCode).Title("Product");
            TextFilterFor(x => x.GrossWeightUnit).Title("Gross Weight Unit");
            TextFilterFor(x => x.PackagesUnit).Title("Packages Unit");
            TextFilterFor(x => x.InvoiceUQ).Title("Invoice UQ");
            DateRangeFilterFor(x => x.CreatedDate).Title("Created Date");
        }
    }
}