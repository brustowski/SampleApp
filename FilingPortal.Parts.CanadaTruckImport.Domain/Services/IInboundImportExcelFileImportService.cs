using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Services
{
    /// <summary>
    /// Describes service for importing Canada Truck Import records from Excel file
    /// </summary>
    public interface IInboundImportExcelFileImportService : IFileImportService<FileProcessingResult>
    {
    }
}
