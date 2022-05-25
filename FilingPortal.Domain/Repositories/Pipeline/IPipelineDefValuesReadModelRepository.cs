using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Pipeline
{
    /// <summary>
    /// Interface for repository of <see cref="PipelineDefValueReadModel"/>
    /// </summary>
    public interface IPipelineDefValuesReadModelRepository : IAgileConfiguration<PipelineDefValueReadModel>
        , IRepositoryWithTypeId<PipelineDefValueReadModel, int>
        , ISearchRepository
    {
    }
}