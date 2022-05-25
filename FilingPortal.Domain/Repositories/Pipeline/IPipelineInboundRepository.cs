using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain.Repositories;

namespace FilingPortal.Domain.Repositories.Pipeline
{
    /// <summary>
    /// Interface for repository of <see cref="PipelineInbound"/>
    /// </summary>
    public interface IPipelineInboundRepository : IInboundRecordsRepository<PipelineInbound>
    {
    }
}
