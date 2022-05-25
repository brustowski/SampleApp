using System;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Repositories;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Services
{
    /// <summary>
    /// File Procedure Service
    /// </summary>
    public class FilingService : IFilingService<InboundRecord>, IAutofilingService<InboundRecord>
    {
        /// <summary>
        /// File Procedure Service
        /// </summary>
        private readonly IFilingWorkflow<FilingHeader, DefValueManual> _filingWorkflow;

        private readonly IFilingHeadersRepository _filingHeadersRepository;

        /// <summary>
        /// Specification builder
        /// </summary>
        private readonly ISpecificationBuilder _specificationBuilder;

        /// <summary>
        /// Inbound records repository
        /// </summary>
        private readonly IInboundRecordsRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilingService" /> class
        /// </summary>
        /// <param name="repository">The inbound records repository</param>
        /// <param name="specificationBuilder">Specification builder for search requests</param>
        /// <param name="filingWorkflow">Filing Workflow</param>
        /// <param name="filingHeadersRepository">Filing Headers repository</param>
        public FilingService(
            IInboundRecordsRepository repository,
            ISpecificationBuilder specificationBuilder,
            IFilingWorkflow<FilingHeader, DefValueManual> filingWorkflow,
            IFilingHeadersRepository filingHeadersRepository
            )
        {
            _filingWorkflow = filingWorkflow;
            _filingHeadersRepository = filingHeadersRepository;
            _specificationBuilder = specificationBuilder;
            _repository = repository;
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
            ISpecification specification = _specificationBuilder.Build<InboundRecord>(filtersSet);
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
        /// Creates filing headers for specified records
        /// </summary>
        /// <param name="ids">Records id</param>
        /// <param name="user">User account</param>
        private IEnumerable<FilingHeader> CreateSingleFilingHeaders(IEnumerable<int> ids, string user)
        {
            IEnumerable<InboundRecord> records = _repository.GetList(ids);

            foreach (InboundRecord record in records)
            {
                FilingHeader header = _filingWorkflow.CreateHeader(user);
                header.AddInboundRecords(new[] { record });

                yield return header;
            }
        }

        /// <summary>
        /// Update inbound records specified by filter set.
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="user">The user who started the process</param>
        public OperationResultWithValue<int[]> Update(FiltersSet filtersSet, AppUsersModel user)
        {
            ISpecification specification = _specificationBuilder.Build<InboundReadModel>(filtersSet);
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
            var filingHeaders = _filingHeadersRepository.FindByInboundRecordIds(updatedIds)
                .Where(x => x.JobStatus != JobStatus.Open).ToList();

            UpdateFilingHeaders(filingHeaders, user.Id);

            filingHeaders = ExcludeFilingHeadersWithErrors(filingHeaders, updatedIds);

            OperationResultWithValue<int[]> result = Refile(filingHeaders.Select(x => x.Id), user.Id);

            var idsToProcess = (
                from id in updatedIds
                let currentHeader = filingHeaders.FirstOrDefault(x =>
                    x.InboundRecords.Any(y => y.Id == id))
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
        /// Refile specified inbound records and runs Filing or Updating process for it. If records are not specified then update all available records.
        /// </summary>
        /// <param name="filingHeaderIds">A collection of the filing header ids to update</param>
        /// <param name="user">The user who started the process</param>
        public OperationResultWithValue<int[]> Refile(IEnumerable<int> filingHeaderIds, string user) =>
            _filingWorkflow.Refile(filingHeaderIds, user);

        /// <summary>
        /// Calls File procedure and set Autofiled flag
        /// </summary>
        /// <param name="filingHeaderIds">The filing identifiers</param>
        /// <param name="user">The user who started the process</param>
        public void AutoFile(int[] filingHeaderIds, string user)
        {
            IEnumerable<FilingHeader> filingHeaders = _filingWorkflow.File(filingHeaderIds);
            UpdateFilingHeaders(filingHeaders, user, true);
        }

        /// <summary>
        /// Updates filing headers
        /// </summary>
        /// <param name="filingHeaders">Collection of filing headers</param>
        /// <param name="user">User account</param>
        /// <param name="isAuto">Auto filing indicator</param>
        private void UpdateFilingHeaders(IEnumerable<FilingHeader> filingHeaders, string user, bool isAuto = false)
        {
            DateTime lastModifiedDate = DateTime.Now;
            foreach (FilingHeader filingHeader in filingHeaders)
            {
                filingHeader.LastModifiedDate = lastModifiedDate;
                filingHeader.LastModifiedUser = user;
                if (!isAuto)
                {
                    filingHeader.JobStatus = JobStatus.WaitingUpdate;
                }

                _filingHeadersRepository.Update(filingHeader);
            }

            _filingHeadersRepository.Save();
        }

        /// <summary>
        /// Exclude from update filing headers that contain records with errors
        /// </summary>
        /// <param name="filingHeaders">The list of the filing header</param>
        /// <param name="updatedRecordIds">The list of the valid record ids</param>
        private List<FilingHeader> ExcludeFilingHeadersWithErrors(IEnumerable<FilingHeader> filingHeaders, IEnumerable<int> updatedRecordIds)
        {
            return filingHeaders.Where(filingHeader =>
                filingHeader.InboundRecords.Any(inboundRecord =>
                    updatedRecordIds.Any(id => id == inboundRecord.Id))).ToList();
        }
    }
}
