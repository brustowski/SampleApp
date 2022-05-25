using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Services.Truck
{
    /// <summary>
    /// Service describing File Procedure
    /// </summary>
    public class TruckFilingService : ITruckFilingService
    {
        private readonly IFilingWorkflow<TruckFilingHeader, TruckDefValueManual> _filingWorkflow;
        private readonly ISpecificationBuilder _specificationBuilder;
        private readonly ITruckInboundRepository _inboundRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckFilingService" /> class
        /// </summary>
        /// <param name="inboundRepository">The Truck inbound repository</param>
        /// <param name="specificationBuilder">Specification builder for search requests</param>
        /// <param name="filingWorkflow">Filing Workflow</param>
        public TruckFilingService(
            ITruckInboundRepository inboundRepository,
            ISpecificationBuilder specificationBuilder,
            IFilingWorkflow<TruckFilingHeader, TruckDefValueManual> filingWorkflow
            )
        {
            _filingWorkflow = filingWorkflow;
            _specificationBuilder = specificationBuilder;
            _inboundRepository = inboundRepository;
        }

        /// <summary>
        /// Creates single-filing filing headers by specified record ids
        /// </summary>
        /// <param name="inboundIds">Inbound record ids</param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(IEnumerable<int> inboundIds, string userAccount = null) =>
            _filingWorkflow.StartSingleFiling(CreateSingleFilingHeaders(inboundIds, userAccount));

        /// <summary>
        /// Creates single-filing filing headers by specified filter set and excluded records id 
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(FiltersSet filtersSet, string userAccount = null) =>
            _filingWorkflow.StartSingleFiling(CreateSingleFilingHeaders(FiltersetToIds(filtersSet), userAccount));

        internal IEnumerable<int> FiltersetToIds(FiltersSet filtersSet)
        {
            ISpecification specification = _specificationBuilder.Build<TruckInbound>(filtersSet);
            IEnumerable<int> ids = _inboundRepository.GetAll<EntityDto>(specification, 100000, false).Select(x => x.Id);
            return ids;
        }

        /// <summary>
        /// Calls File procedure for the specified filing header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">The filing identifiers</param>
        public void File(params int[] filingHeaderIds) => _filingWorkflow.File(filingHeaderIds);

        /// <summary>
        /// Sets the status of Filing Header to In Review by filing header identifiers
        /// </summary>
        /// <param name="filingHeaderIds">The filing header identifiers</param>
        public void SetInReview(params int[] filingHeaderIds) => _filingWorkflow.SetInReview(filingHeaderIds);

        /// <summary>
        /// Cancel filing process for the specified filing headers
        /// </summary>
        /// <param name="filingHeaderIds">The filing headers</param>
        public void CancelFilingProcess(params int[] filingHeaderIds) => _filingWorkflow.CancelFilingProcess(filingHeaderIds);

        /// <summary>
        /// Refile specified inbound records and runs Filing or Updating process for it. If records are not specified then update all available records.
        /// </summary>
        /// <param name="filingHeaderIds">A collection of the filing header ids to update</param>
        /// <param name="user">The user who started the process</param>
        public OperationResultWithValue<int[]> Refile(IEnumerable<int> filingHeaderIds, string user) =>
            _filingWorkflow.Refile(filingHeaderIds, user);

        internal IEnumerable<TruckFilingHeader> CreateSingleFilingHeaders(IEnumerable<int> inboundIds, string user)
        {
            IEnumerable<TruckInbound> inbounds = _inboundRepository.GetList(inboundIds);

            foreach (TruckInbound inbound in inbounds)
            {
                TruckFilingHeader header = _filingWorkflow.CreateHeader(user);
                header.AddTruckInbounds(new[] { inbound });

                yield return header;
            }
        }
    }
}
