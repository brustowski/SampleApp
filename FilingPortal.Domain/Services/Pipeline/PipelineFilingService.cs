using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Service describing File Procedure
    /// </summary>
    public class PipelineFilingService : IPipelineFilingService
    {
        private readonly IFilingWorkflow<PipelineFilingHeader, PipelineDefValueManual> _filingWorkflow;
        private readonly ISpecificationBuilder _specificationBuilder;
        private readonly IPipelineInboundRepository _inboundRepository;
        /// <summary>
        /// The api calculator file generator
        /// </summary>
        private readonly IFileGenerator<PipelineInbound> _fileGenerator;
        /// <summary>
        /// The document factory
        /// </summary>
        private readonly IPipelineDocumentFactory _fileFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilingService" /> class
        /// </summary>
        /// <param name="inboundRepository">The inbounds repository</param>
        /// <param name="specificationBuilder">Specification builder for search requests</param>
        /// <param name="filingWorkflow">Filing Workflow</param>
        /// <param name="fileGenerator">The api calculator file generator</param>
        /// <param name="fileFactory">The file creation factory</param>
        public PipelineFilingService(
            IPipelineInboundRepository inboundRepository,
            ISpecificationBuilder specificationBuilder,
            IFilingWorkflow<PipelineFilingHeader, PipelineDefValueManual> filingWorkflow, 
            IFileGenerator<PipelineInbound> fileGenerator, 
            IPipelineDocumentFactory fileFactory)
        {
            _filingWorkflow = filingWorkflow;
            _fileGenerator = fileGenerator;
            _fileFactory = fileFactory;
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
        /// Creates single-filing filing headers by specified filter set and exluded records id 
        /// </summary>
        /// <param name="filtersSet">List of the filters <see cref="FiltersSet"/></param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(FiltersSet filtersSet, string userAccount = null) =>
            _filingWorkflow.StartSingleFiling(CreateSingleFilingHeaders(FiltersetToIds(filtersSet), userAccount));

        internal IEnumerable<int> FiltersetToIds(FiltersSet filtersSet)
        {
            ISpecification specification = _specificationBuilder.Build<PipelineInbound>(filtersSet);
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

        internal IEnumerable<PipelineFilingHeader> CreateSingleFilingHeaders(IEnumerable<int> inboundIds, string user)
        {
            IEnumerable<PipelineInbound> inbounds = _inboundRepository.GetList(inboundIds);

            foreach (PipelineInbound inbound in inbounds)
            {
                PipelineFilingHeader header = _filingWorkflow.CreateHeader(user);
                header.AddPipelineInbounds(new[] { inbound });
                PipelineDocument document = CreateDocument(inbound, user);
                header.AddDocuments(new [] {document});

                yield return header;
            }
        }
        
        /// <summary>
        /// Creates the API calculator document from inbound record
        /// </summary>
        /// <param name="inbound">The inbound record</param>
        internal PipelineDocument CreateDocument(PipelineInbound inbound, string user)
        {
            BinaryFileModel file = _fileGenerator.Generate(inbound);
            PipelineDocument document = _fileFactory.CreateApiCalculator(file, user);

            return document;
        }
    }
}
