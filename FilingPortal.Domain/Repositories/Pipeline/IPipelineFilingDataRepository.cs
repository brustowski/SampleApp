using FilingPortal.Domain.Entities.Pipeline;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Pipeline
{
    /// <summary>
    /// Describes repository of <see cref="PipelineFilingData"/>
    /// </summary>
    public interface IPipelineFilingDataRepository : IFilingDataRepository<PipelineFilingData>
    {
    }
}