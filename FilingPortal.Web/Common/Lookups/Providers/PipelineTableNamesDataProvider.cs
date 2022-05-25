using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents provider for tables name
    /// </summary>
    public class PipelineTableNamesDataProvider : BaseTableNamesDataProvider<PipelineTable>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.PipelineTableNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineTableNamesDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public PipelineTableNamesDataProvider(ITablesRepository<PipelineTable> repository) : base(repository)
        {
        }
    }
}