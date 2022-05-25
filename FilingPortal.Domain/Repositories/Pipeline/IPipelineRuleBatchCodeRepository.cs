using FilingPortal.Domain.Entities.Pipeline;

namespace FilingPortal.Domain.Repositories.Pipeline
{
    /// <summary>
    /// Describes Pipeline batch code rules repository
    /// </summary>
    public interface IPipelineRuleBatchCodeRepository
    {
        /// <summary>
        /// Gets Batch rule by code
        /// </summary>
        /// <param name="code">Batch code</param>
        PipelineRuleBatchCode GetByBatchCode(string code);
    }
}
