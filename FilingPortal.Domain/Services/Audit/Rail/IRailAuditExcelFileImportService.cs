using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Domain.Services.Audit.Rail
{
    /// <summary>
    /// Describes service for importing Rail Audit records from Excel file
    /// </summary>
    public interface IRailAuditExcelFileImportService : IFileImportService<FileProcessingResult>
    {
    }
}
