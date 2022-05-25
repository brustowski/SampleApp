using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class TruckExportTableNamesDataProvider : BaseTableNamesDataProvider<TruckExportTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportTableNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public TruckExportTableNamesDataProvider(ITablesRepository<TruckExportTable> repository) : base(repository)
        {
        }

        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.TruckExportTableNames;
    }
}