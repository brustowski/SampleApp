using System.Collections.Generic;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Describes File Procedure Service
    /// </summary>
    public interface IFilingWorkflow<TFilingHeader, TDefValuesManual>
        where TFilingHeader : BaseFilingHeader
        where TDefValuesManual : BaseDefValuesManual
    {
        /// <summary>
        /// Creates suitable Filing Header
        /// </summary>
        TFilingHeader CreateHeader(string userAccount);
        /// <summary>
        /// Creates initial data for single filing
        /// </summary>
        /// <param name="headers">Selected headers</param>
        OperationResultWithValue<int[]> StartSingleFiling(IEnumerable<TFilingHeader> headers);
        /// <summary>
        /// Starts filing on selected headers
        /// </summary>
        /// <param name="filingHeaderIds">Selected headers ids</param>
        IEnumerable<TFilingHeader> File(params int[] filingHeaderIds);
        /// <summary>
        /// Moves filing process to In Review status
        /// </summary>
        /// <param name="filingHeaderIds">Selected headers ids</param>
        void SetInReview(params int[] filingHeaderIds);
        /// <summary>
        /// Cancel filing process and moves record to status Open
        /// </summary>
        /// <param name="filingHeaderIds">Selected filing headers ids</param>
        void CancelFilingProcess(params int[] filingHeaderIds);
        /// <summary>
        /// Starts refiling on selected headers
        /// </summary>
        /// <param name="headerIds">Filing Header Ids collection</param>
        /// <param name="userAccount">User account to get information from</param>
        OperationResultWithValue<int[]> Refile(IEnumerable<int> headerIds, string userAccount = null);
    }
}