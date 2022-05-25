using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using DataProviderNames = FilingPortal.Parts.CanadaTruckImport.Web.Configs.DataProviderNames;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Common.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class TableColumnsDataProvider : BaseTableColumnsDataProvider<Tables>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.TableColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableColumnsDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public TableColumnsDataProvider(ITablesRepository<Tables> repository) : base(repository)
        {
        }
    }
}