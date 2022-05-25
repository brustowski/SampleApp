﻿using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories.VesselExport;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;

namespace FilingPortal.Domain.Services.VesselExport
{
    /// <summary>
    /// File Procedure Service
    /// </summary>
    public class VesselExportFilingService : IVesselExportFilingService
    {
        /// <summary>
        /// File Procedure Service
        /// </summary>
        private readonly IFilingWorkflow<VesselExportFilingHeader, VesselExportDefValueManual> _filingWorkflow;

        /// <summary>
        /// Specification builder
        /// </summary>
        private readonly ISpecificationBuilder _specificationBuilder;

        /// <summary>
        /// Vessel export repository
        /// </summary>
        private readonly IVesselExportRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportFilingService" /> class
        /// </summary>
        /// <param name="repository">The Vessel Export repository</param>
        /// <param name="specificationBuilder">Specification builder for search requests</param>
        /// <param name="filingWorkflow">Filing Workflow</param>
        public VesselExportFilingService(
            IVesselExportRepository repository,
            ISpecificationBuilder specificationBuilder,
            IFilingWorkflow<VesselExportFilingHeader, VesselExportDefValueManual> filingWorkflow
            )
        {
            _filingWorkflow = filingWorkflow;
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
            ISpecification specification = _specificationBuilder.Build<VesselExportRecord>(filtersSet);
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
        /// Refile specified inbound records and runs Filing or Updating process for it. If records are not specified then update all available records.
        /// </summary>
        /// <param name="filingHeaderIds">A collection of the filing header ids to update</param>
        /// <param name="user">The user who started the process</param>
        public OperationResultWithValue<int[]> Refile(IEnumerable<int> filingHeaderIds, string user) =>
            _filingWorkflow.Refile(filingHeaderIds, user);

        /// <summary>
        /// Creates filing headers for specified records
        /// </summary>
        /// <param name="ids">Records id</param>
        /// <param name="user">User account</param>
        internal IEnumerable<VesselExportFilingHeader> CreateSingleFilingHeaders(IEnumerable<int> ids, string user)
        {
            IEnumerable<VesselExportRecord> records = _repository.GetList(ids);

            foreach (VesselExportRecord record in records)
            {
                VesselExportFilingHeader header = _filingWorkflow.CreateHeader(user);
                header.AddRecords(new[] { record });

                yield return header;
            }
        }
    }
}
