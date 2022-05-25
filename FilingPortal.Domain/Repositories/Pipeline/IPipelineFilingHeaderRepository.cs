using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Pipeline
{
    /// <summary>
    /// Describes the repository of the Pipeline Filing Header
    /// </summary>
    public interface IPipelineFilingHeaderRepository : IFilingHeaderRepository<PipelineFilingHeader>, IFilingSectionRepository { }
}