using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Import.Models;

namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// Describes service for importing Truck Inbound records from Excel file
    /// </summary>
    public interface ITruckExportExcelFileImportService : IFileImportService<FileProcessingDetailedResult>
    {
    }
}
