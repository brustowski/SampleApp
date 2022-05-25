using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.PluginEngine.DataProviders;
using DataProviderNames = FilingPortal.Parts.Isf.Web.Configs.DataProviderNames;

namespace FilingPortal.Parts.Isf.Web.Common.Providers
{
    /// <summary>
    /// Provider for tables name
    /// </summary>
    public class TableNamesDataProvider : BaseTableNamesDataProvider<Tables>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.TableNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public TableNamesDataProvider(ITablesRepository<Tables> repository) : base(repository)
        {
        }
    }
}