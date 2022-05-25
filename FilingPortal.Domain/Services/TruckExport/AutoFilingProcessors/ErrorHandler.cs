using FilingPortal.Domain.Entities.TruckExport;

namespace FilingPortal.Domain.Services.TruckExport.AutoFilingProcessors
{
    /// <summary>
    /// Handles error in records processor
    /// </summary>
    /// <param name="record"></param>
    /// <param name="message"></param>
    public delegate void ErrorHandler(TruckExportRecord record, string message);
}