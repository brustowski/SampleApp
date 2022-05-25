using FilingPortal.Domain.Entities.Pipeline;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Service for single filing grid operations
    /// </summary>
    public interface IPipelineSingleFilingGridService: ISingleFilingGridService<PipelineInbound>
    {
    }
}