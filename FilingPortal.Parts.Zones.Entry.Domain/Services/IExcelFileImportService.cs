using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Parts.Zones.Entry.Domain.Services
{
    /// <summary>
    /// Describes service for importing Entry 06 Import records from Excel file
    /// </summary>
    public interface IExcelFileImportService : IFileImportService<FileProcessingResult>
    {
    }
}
