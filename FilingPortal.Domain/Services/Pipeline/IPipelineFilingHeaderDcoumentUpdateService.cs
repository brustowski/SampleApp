using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Parts.Common.Domain.DTOs;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Describes service that updates pipeline filing header documents
    /// </summary>
    public interface IPipelineFilingHeaderDocumentUpdateService : IFilingHeaderDocumentUpdateService<PipelineDocumentDto>
    {
        /// <summary>
        /// Updates existing API Calculator file
        /// </summary>
        /// <param name="parameters">The filing parameters</param>
        void UpdateApiCalculator(InboundRecordFilingParameters parameters);

        /// <summary>
        /// Generates API Calculator file
        /// </summary>
        /// <param name="parameters">The filing parameters</param>
        BinaryFileModel GenerateApiCalculator(InboundRecordFilingParameters parameters);
    }
}
