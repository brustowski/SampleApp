using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Enums;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.Domain.Services.TruckExport
{
    /// <summary>
    /// File Procedure Service
    /// </summary>
    public class TruckExportFilingService : ITruckExportFilingService
    {
        /// <summary>
        /// File Procedure Service
        /// </summary>
        private readonly IFilingWorkflow<TruckExportFilingHeader, TruckExportDefValueManual> _filingWorkflow;

        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly ITruckExportFilingHeadersRepository _filingHeaderRepository;

        /// <summary>
        /// Specification builder
        /// </summary>
        private readonly ISpecificationBuilder _specificationBuilder;

        /// <summary>
        /// Truck export repository
        /// </summary>
        private readonly ITruckExportRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportFilingService" /> class
        /// </summary>
        /// <param name="repository">The Truck Export repository</param>
        /// <param name="filingHeaderRepository">The filing headers repository</param>
        /// <param name="specificationBuilder">Specification builder for search requests</param>
        /// <param name="filingWorkflow">Filing Workflow</param>
        public TruckExportFilingService(
            ITruckExportRepository repository,
            ITruckExportFilingHeadersRepository filingHeaderRepository,
            ISpecificationBuilder specificationBuilder,
            IFilingWorkflow<TruckExportFilingHeader, TruckExportDefValueManual> filingWorkflow)
        {
            _filingWorkflow = filingWorkflow;
            _specificationBuilder = specificationBuilder;
            _repository = repository;
            _filingHeaderRepository = filingHeaderRepository;
        }

        /// <summary>
        /// Creates single-filing filing headers by specified record ids
        /// </summary>
        /// <param name="ids">Record ids</param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(IEnumerable<int> ids, string userAccount = null) =>
            _filingWorkflow.StartSingleFiling(CreateSingleFilingHeaders(ids, userAccount));

        /// <summary>
        /// Creates single-filing filing headers by specified filter set and excluded records id 
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(FiltersSet filtersSet, string userAccount = null) =>
            _filingWorkflow.StartSingleFiling(CreateSingleFilingHeaders(SetFilterToIds(filtersSet), userAccount));

        /// <summary>
        /// Set filters to filter by provided ids
        /// </summary>
        /// <param name="filtersSet">Filters to set</param>
        internal IEnumerable<int> SetFilterToIds(FiltersSet filtersSet)
        {
            ISpecification specification = _specificationBuilder.Build<TruckExportRecord>(filtersSet);
            IEnumerable<int> ids = _repository.GetAll<EntityDto>(specification, 100000, false).Select(x => x.Id);
            return ids;
        }

        /// <summary>
        /// Calls File procedure for the specified filing header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">The filing header identifiers</param>
        public void File(params int[] filingHeaderIds) => _filingWorkflow.File(filingHeaderIds);

        /// <summary>
        /// Sets the status of Filing Header to In Review by filing header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">The filing header identifier identifiers</param>
        public void SetInReview(params int[] filingHeaderIds) => _filingWorkflow.SetInReview(filingHeaderIds);

        /// <summary>
        /// Cancel filing process for the specified filing headers
        /// </summary>
        /// <param name="filingHeaderIds">The filing header identifiers</param>
        public void CancelFilingProcess(params int[] filingHeaderIds) => _filingWorkflow.CancelFilingProcess(filingHeaderIds);

        /// <summary>
        /// Update inbound records specified by filter set.
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="user">The user who started the process</param>
        public OperationResultWithValue<int[]> Update(FiltersSet filtersSet, AppUsersModel user)
        {
            ISpecification specification = _specificationBuilder.Build<TruckExportReadModel>(filtersSet);
            IEnumerable<int> ids = _repository.GetAll<EntityDto>(specification, 100000, false).Select(x => x.Id);

            return Update(ids, user);
        }

        /// <summary>
        /// Updates specified inbound records and runs Filing or Updating process for it. If records are not specified then update all available records.
        /// </summary>
        /// <param name="ids">A collection of the inbound record ids to update</param>
        /// <param name="user">The user who started the process</param>
        public OperationResultWithValue<int[]> Update(IEnumerable<int> ids, AppUsersModel user)
        {
            int[] updatedIds = ids as int[] ?? ids.ToArray();

            // We will pick only that filing headers that may contain user changes
            var filingHeaders = _filingHeaderRepository.FindByInboundRecordIds(updatedIds)
                .Where(x => x.JobStatus != JobStatus.Open).ToList();

            UpdateFilingHeaders(filingHeaders, user.Id);

            filingHeaders = ExcludeFilingHeadersWithErrors(filingHeaders, updatedIds);

            OperationResultWithValue<int[]> result = Refile(filingHeaders.Select(x => x.Id), user.Id);

            var idsToProcess = (
                from id in updatedIds
                let currentHeader = filingHeaders.FirstOrDefault(x =>
                    x.TruckExports.Any(y => y.Id == id))
                where currentHeader == null
                select id).ToList();

            OperationResultWithValue<int[]> fileResult = _filingWorkflow.StartSingleFiling(CreateSingleFilingHeaders(idsToProcess, user.Id));

            foreach (string error in fileResult.Errors)
            {
                result.Errors.Add(error);
            }

            result.Value = result.Value.Union(fileResult.Value).ToArray();
            return result;
        }

        /// <summary>
        /// Updates filing headers
        /// </summary>
        /// <param name="filingHeaders">Collection of filing headers</param>
        /// <param name="user">User account</param>
        /// <param name="isAuto">Auto filing indicator</param>
        private void UpdateFilingHeaders(IEnumerable<TruckExportFilingHeader> filingHeaders, string user, bool isAuto = false)
        {
            DateTime lastModifiedDate = DateTime.Now;
            foreach (TruckExportFilingHeader filingHeader in filingHeaders)
            {
                filingHeader.LastModifiedDate = lastModifiedDate;
                filingHeader.LastModifiedUser = user;
                filingHeader.IsUpdated = true;
                if (!isAuto)
                {
                    filingHeader.JobStatus = JobStatus.WaitingUpdate;
                }

                _filingHeaderRepository.Update(filingHeader);
            }

            _filingHeaderRepository.Save();
        }

        /// <summary>
        /// Exclude from update filing headers that contain records with errors
        /// </summary>
        /// <param name="filingHeaders">The list of the filing header</param>
        /// <param name="updatedRecordIds">The list of the valid record ids</param>
        private List<TruckExportFilingHeader> ExcludeFilingHeadersWithErrors(IEnumerable<TruckExportFilingHeader> filingHeaders, IEnumerable<int> updatedRecordIds)
        {
            return filingHeaders.Where(filingHeader =>
                filingHeader.TruckExports.Any(inboundRecord =>
                    updatedRecordIds.Any(id => id == inboundRecord.Id))).ToList();
        }

        /// <summary>
        /// Calls File procedure and set Autofiled flag
        /// </summary>
        /// <param name="filingHeaderIds">The filing identifiers</param>
        /// <param name="user">The user who started the process</param>
        public void AutoFile(int[] filingHeaderIds, string user)
        {
            IEnumerable<TruckExportFilingHeader> filingHeaders = _filingWorkflow.File(filingHeaderIds);
            UpdateFilingHeaders(filingHeaders, user, true);
        }

        /// <summary>
        /// Creates filing headers for specified records
        /// </summary>
        /// <param name="ids">Records id</param>
        /// <param name="user">User account</param>
        internal IEnumerable<TruckExportFilingHeader> CreateSingleFilingHeaders(IEnumerable<int> ids, string user)
        {
            IEnumerable<TruckExportRecord> records = _repository.GetList(ids);

            foreach (TruckExportRecord record in records)
            {
                TruckExportFilingHeader header = _filingWorkflow.CreateHeader(user);
                header.AddRecords(new[] { record });

                yield return header;
            }
        }

        /// <summary>
        /// Refile specified inbound records and runs Filing or Updating process for it. If records are not specified then update all available records.
        /// </summary>
        /// <param name="filingHeaderIds">A collection of the filing header ids to update</param>
        /// <param name="user">The user who started the process</param>
        public OperationResultWithValue<int[]> Refile(IEnumerable<int> filingHeaderIds, string user) => _filingWorkflow.Refile(filingHeaderIds, user);
    }
}
