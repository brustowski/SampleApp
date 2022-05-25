using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Parts.Rail.Export.Domain.Services
{
    /// <summary>
    /// Describes service for importing records from Excel file
    /// </summary>
    public interface IInboundImportExcelFileImportService : IFileImportService<FileProcessingResult>
    {
    }
}
