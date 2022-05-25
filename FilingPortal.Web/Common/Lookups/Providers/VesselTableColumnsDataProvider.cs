using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.DataLayer.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class VesselTableColumnsDataProvider : BaseTableColumnsDataProvider<VesselImportTable>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.VesselTableColumns;


        /// <summary>
        /// Initializes a new instance of the <see cref="VesselTableColumnsDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public VesselTableColumnsDataProvider(ITablesRepository<VesselImportTable> repository) : base(repository)
        {
        }
    }
}