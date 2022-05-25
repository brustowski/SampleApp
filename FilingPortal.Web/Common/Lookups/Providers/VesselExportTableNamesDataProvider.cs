using FilingPortal.Domain.Repositories.VesselExport;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.DataLayer.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class VesselExportTableNamesDataProvider : BaseTableNamesDataProvider<VesselExportTable>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.VesselExportTableNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportTableNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public VesselExportTableNamesDataProvider(ITablesRepository<VesselExportTable> repository) : base(repository)
        {
        }
    }
}