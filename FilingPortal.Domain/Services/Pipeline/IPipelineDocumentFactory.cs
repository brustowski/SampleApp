using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Describes Pipeline document factory
    /// </summary>
    public interface IPipelineDocumentFactory : IDocumentFactory<PipelineDocument>
    {
        /// <summary>
        /// Creates the pipeline api calculator document from specified file model and with specified creator name
        /// </summary>
        /// <param name="file">The file model</param>
        /// <param name="creatorName">The creator name</param>
        PipelineDocument CreateApiCalculator(BinaryFileModel file, string creatorName);
    }
}