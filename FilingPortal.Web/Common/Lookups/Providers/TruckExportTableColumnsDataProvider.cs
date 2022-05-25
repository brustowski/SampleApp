using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class TruckExportTableColumnsDataProvider : BaseTableColumnsDataProvider<TruckExportTable>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.TruckExportTableColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportTableColumnsDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public TruckExportTableColumnsDataProvider(ITablesRepository<TruckExportTable> repository) : base(repository)
        {
        }
    }
}