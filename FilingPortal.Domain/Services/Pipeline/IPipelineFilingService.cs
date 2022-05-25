using FilingPortal.Domain.Entities.Pipeline;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Describes Pipeline Inbound Filing Service
    /// </summary>
    public interface IPipelineFilingService: IFilingService<PipelineInbound> { }
}