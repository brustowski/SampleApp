using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Parts.Inbond.Domain.Services
{
    /// <summary>
    /// Describes service for importing Inbond Import records from Excel file
    /// </summary>
    public interface IInbondImportExcelFileImportService : IFileImportService<FileProcessingResult>
    {
    }
}
