using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.TruckExport;
using System.Collections.Generic;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Services.TruckExport
{

    /// <summary>
    /// Defines the Filing service for Truck Export records
    /// </summary>
    public interface ITruckExportFilingService : IFilingService<TruckExportRecord>
    {
        /// <summary>
        /// Update inbound records specified by ids.
        /// </summary>
        /// <param name="recordIds">A collection of the inbound record ids to update</param>
        /// <param name="user">The user who started the process</param>
        OperationResultWithValue<int[]> Update(IEnumerable<int> recordIds, AppUsersModel user);

        /// <summary>
        /// Update inbound records specified by filter set.
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="user">The user who started the process</param>
        OperationResultWithValue<int[]> Update(FiltersSet filtersSet, AppUsersModel user);

        /// <summary>
        /// Refile specified inbound records and runs Filing or Updating process for it. If records are not specified then update all available records.
        /// </summary>
        /// <param name="filingHeaderIds">A collection of the filing header ids to update</param>
        /// <param name="user">The user who started the process</param>
        OperationResultWithValue<int[]> Refile(IEnumerable<int> filingHeaderIds, string user);
        /// <summary>
        /// Calls File procedure and set Autofiled flag
        /// </summary>
        /// <param name="filingHeaderIds">The filing identifiers</param>
        /// <param name="user">The user who started the process</param>
        void AutoFile(int[] filingHeaderIds, string user);
    }
}
