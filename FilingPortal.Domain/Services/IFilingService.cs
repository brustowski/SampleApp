using System.Collections.Generic;
using FilingPortal.Domain.Common.OperationResult;
using Framework.Domain;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Interface describing File Procedure
    /// </summary>
    public interface IFilingService<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Creates single-filing filing headers by specified record ids
        /// </summary>
        /// <param name="inboundIds">Inbound record ids</param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(IEnumerable<int> inboundIds, string userAccount = null);

        /// <summary>
        /// Creates single-filing filing headers by specified filter set and excluded records id 
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(FiltersSet filtersSet, string userAccount = null);

        /// <summary>
        /// Calls File procedure for the specified filing header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">The filing identifiers</param>
        void File(params int[] filingHeaderIds);

        /// <summary>
        /// Sets the status of Filing Header to In Review by filing header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">The filing header identifiers</param>
        void SetInReview(params int[] filingHeaderIds);

        /// <summary>
        /// Cancel filing process for specified filing headers
        /// </summary>
        /// <param name="filingHeaderIds">The filing headers</param>
        void CancelFilingProcess(params int[] filingHeaderIds);

        /// <summary>
        /// Refile specified inbound records and runs Filing or Updating process for it. If records are not specified then update all available records.
        /// </summary>
        /// <param name="filingHeaderIds">A collection of the filing header ids to update</param>
        /// <param name="user">The user who started the process</param>
        OperationResultWithValue<int[]> Refile(IEnumerable<int> filingHeaderIds, string user);
    }
}