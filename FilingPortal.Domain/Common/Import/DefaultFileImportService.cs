using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Domain.Common.Import
{
    /// <summary>
    /// Default import service, returning <see cref="FileProcessingResult"/>
    /// </summary>
    public abstract class DefaultFileImportService : FileImportService<FileProcessingResult>
    {

    }
}