using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.PluginEngine.DataProviders;
using DataProviderNames = FilingPortal.Parts.Inbond.Web.Configs.DataProviderNames;

namespace FilingPortal.Parts.Inbond.Web.Common.Providers
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