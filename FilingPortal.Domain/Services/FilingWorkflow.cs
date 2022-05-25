using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using Framework.Infrastructure;
using Framework.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Services
{
    /// <summary>
    /// Service describing File Procedure
    /// </summary>
    /// <typeparam name="TFilingHeader">Filing header type</typeparam>
    /// <typeparam name="TDefValuesManual">DefValues_Manual type</typeparam>
    public class FilingWorkflow<TFilingHeader, TDefValuesManual> : IFilingWorkflow<TFilingHeader, TDefValuesManual>
        where TFilingHeader : BaseFilingHeader, new()
        where TDefValuesManual : BaseDefValuesManual
    {
        /// <summary>
        /// Defines the locker for multi-threading critical section
        /// </summary>
        protected static readonly object Locker = new object();

        /// <summary>
        /// Defines the defValues_Manual repository
        /// </summary>
        protected readonly IDefValuesManualRepository<TDefValuesManual> DefValuesRepository;

        /// <summary>
        /// Defines the filing headers repository
        /// </summary>
        protected readonly IFilingHeaderRepository<TFilingHeader> FilingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilingWorkflow{TFilingHeader, TDefValuesManual}"/> class.
        /// </summary>
        /// <param name="repository">The filing headers repository</param>
        /// <param name="defValuesRepository">The defValues_Manual repository</param>
        public FilingWorkflow(
            IFilingHeaderRepository<TFilingHeader> repository,
            IDefValuesManualRepository<TDefValuesManual> defValuesRepository)
        {
            FilingRepository = repository;
            DefValuesRepository = defValuesRepository;
        }

        /// <summary>
        /// Returns mapping status to Open
        /// </summary>
        /// <param name="filingHeaderIds">Filing Header ids to Cancel filing process</param>
        public void CancelFilingProcess(params int[] filingHeaderIds)
        {
            foreach (var filingHeaderId in filingHeaderIds)
            {
                FilingRepository.CancelFilingProcess(filingHeaderId);
            }
        }

        /// <summary>
        /// Creates a new filing header
        /// </summary>
        public TFilingHeader CreateHeader(string userAccount) => new TFilingHeader {CreatedUser = userAccount};

        /// <summary>
        /// Starts filing on selected headers
        /// </summary>
        /// <param name="filingHeaderIds">Selected headers ids</param>
        public IEnumerable<TFilingHeader> File(params int[] filingHeaderIds)
        {
            var result = new List<TFilingHeader>();

            foreach (var filingHeaderId in filingHeaderIds)
            {
                FilingRepository.FileRecordsWithHeader(filingHeaderId);
                TFilingHeader filingHeader = FilingRepository.Get(filingHeaderId);
                filingHeader.SetInProgressForMappingStatus();
                FilingRepository.Update(filingHeader);
                result.Add(filingHeader);
            }
            FilingRepository.Save();

            return result;
        }

        /// <summary>
        /// Sets the status of Filing Header to In Review by filing header identifier
        /// </summary>
        /// <param name="filingHeaderIds">The filing header ids</param>
        public void SetInReview(params int[] filingHeaderIds)
        {
            foreach (var filingHeaderId in filingHeaderIds)
            {
                TFilingHeader filingHeader = FilingRepository.Get(filingHeaderId);
                filingHeader.SetInReviewForMappingStatus();
                FilingRepository.Update(filingHeader);
            }
            FilingRepository.Save();
        }

        /// <summary>
        /// Creates the initial filing headers for each inbound record
        /// </summary>
        /// <param name="headers">The headers list</param>
        public OperationResultWithValue<int[]> StartSingleFiling(IEnumerable<TFilingHeader> headers)
        {
            var tasks = new List<Task>();
            headers = headers.ToList(); // Explicitly get all headers
            var results = new List<OperationResultWithValue<int>>(headers.Count());
            headers.ForEach(x => FilingRepository.Add(x));
            FilingRepository.Save();

            foreach (TFilingHeader header in headers)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    results.Add(RunFilingProcedure(header.Id, header.CreatedUser));
                }));
            }

            Task.WaitAll(tasks.ToArray());

            OperationResultWithValue<int[]> result = CombineResults(results);
            return result;
        }

        /// <summary>
        /// Runs initial filing procedure for selected filing header
        /// </summary>
        /// <param name="headerId">Filing header id</param>
        /// <param name="userAccount">User account</param>
        protected OperationResultWithValue<int> RunFilingProcedure(int headerId, string userAccount)
        {
            var result = new OperationResultWithValue<int>();
            try
            {
                lock (Locker)
                {
                    FilingRepository.FillDataForFilingHeader(headerId, userAccount);
                }
                result.Value = headerId;
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "An error occurred during preparing set for filing.");
                result.AddErrorMessage(ErrorMessages.CreateInitialFilingHeaderError);
            }
            return result;
        }

        /// <summary>
        /// Combines list of operation results into one operation result
        /// </summary>
        /// <param name="results">List of operation results </param>
        private OperationResultWithValue<int[]> CombineResults(List<OperationResultWithValue<int>> results)
        {
            var result = new OperationResultWithValue<int[]>();
            var values = new int[results.Count];
            var i = 0;
            foreach (OperationResultWithValue<int> res in results)
            {
                values[i++] = res.Value;
                foreach (var err in res.Errors)
                {
                    result.Errors.Add(err);
                }
            }
            result.Value = values;
            return result;
        }

        /// <summary>
        /// Starts refiling on selected headers
        /// </summary>
        /// <param name="headerIds">Filing Header Ids collection<</param>
        /// <param name="userAccount">User account to get information from</param>
        public OperationResultWithValue<int[]> Refile(IEnumerable<int> headerIds, string userAccount = null)
        {
            var results = new List<OperationResultWithValue<int>>();
            headerIds = headerIds.ToList();

            Task.WaitAll(headerIds.Select(id => Task.Factory.StartNew(() =>
            {
                results.Add(RunRefilingProcedure(id, userAccount));
            })).ToArray());

            OperationResultWithValue<int[]> result = CombineResults(results);
            return result;
        }

        /// <summary>
        /// Runs initial filing procedure for selected filing header
        /// </summary>
        /// <param name="headerId">Filing header id</param>
        /// <param name="userAccount">User account</param>
        protected OperationResultWithValue<int> RunRefilingProcedure(int headerId, string userAccount)
        {
            var result = new OperationResultWithValue<int>();
            try
            {
                lock (Locker)
                {
                    FilingRepository.RefillDataForFilingHeader(headerId, userAccount);
                }
                result.Value = headerId;
            }
            catch (Exception ex)
            {
                AppLogger.Error(ex, "An error occurred during preparing set for filing.");
                result.AddErrorMessage(ErrorMessages.CreateInitialFilingHeaderError);
            }
            return result;
        }
    }
}
