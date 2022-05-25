using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.DataLayer.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class VesselExportTableColumnsDataProvider : BaseTableColumnsDataProvider<VesselExportTable>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.VesselExportTableColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportTableColumnsDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public VesselExportTableColumnsDataProvider(ITablesRepository<VesselExportTable> repository) : base(repository)
        {
        }
    }
}