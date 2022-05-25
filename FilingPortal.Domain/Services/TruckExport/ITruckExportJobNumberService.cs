namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// Describes methods to work with Job Numbers
    /// </summary>
    public interface ITruckExportJobNumberService
    {
        /// <summary>
        /// Ensure Job Number for specified records
        /// </summary>
        /// <param name="recordIds">Array of the records to validate</param>
        /// <param name="userId">The user id</param>
        int EnsureJobNumberForRecords(int[] recordIds, string userId);
    }
}
