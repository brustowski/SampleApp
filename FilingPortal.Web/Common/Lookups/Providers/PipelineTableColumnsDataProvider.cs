using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;

namespace FilingPortal.Web.Common.Lookups.Providers
{
    /// <summary>
    /// Represents provider for tables name
    /// </summary>
    public class PipelineTableColumnsDataProvider : BaseTableColumnsDataProvider<PipelineTable>
    {
        /// <summary>
        /// Gets the name of the data provider
        /// </summary>
        public override string Name => DataProviderNames.PipelineTableColumns;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineTableColumnsDataProvider"/> class
        /// </summary>
        /// <param name="repository">The tables repository</param>
        public PipelineTableColumnsDataProvider(ITablesRepository<PipelineTable> repository) : base(repository)
        {
        }
    }
}