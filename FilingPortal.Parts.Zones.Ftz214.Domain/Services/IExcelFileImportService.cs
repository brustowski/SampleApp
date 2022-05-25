using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Services
{
    /// <summary>
    /// Describes service for importing Ftz214 06 Import records from Excel file
    /// </summary>
    public interface IExcelFileImportService : IFileImportService<FileProcessingResult>
    {
    }
}
