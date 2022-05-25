
namespace FilingPortal.Domain.Services.Pipeline
{
    using FilingPortal.Domain.Common.Import;
    using FilingPortal.Domain.Common.Import.Models;

    /// <summary>
    /// Describes service for importing Pipeline Inbound records from Excel file
    /// </summary>
    public interface IPipelineInboundExcelFileImportService : IFileImportService<FileProcessingResult>
    {
    }
}
