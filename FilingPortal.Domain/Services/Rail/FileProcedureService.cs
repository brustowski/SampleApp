using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Services.Rail
{
    /// <summary>
    /// Service describing File Procedure
    /// </summary>
    public class FileProcedureService : IFileProcedureService
    {
        /// <summary>
        /// The RailBdParsed entity repository
        /// </summary>
        private readonly IBdParsedRepository _parsedRepository;

        /// <summary>
        /// The manifest PDF generator
        /// </summary>
        private readonly IFileGenerator<RailFilingHeader> _manifestPdfGenerator;

        /// <summary>
        /// The rail document factory
        /// </summary>
        private readonly IRailDocumentFactory _railDocumentFactory;

        /// <summary>
        /// The read model repository
        /// </summary>
        private readonly IRailInboundReadModelRepository _readModelRepository;
        
        /// <summary>
        /// Search request specification builder
        /// </summary>
        private readonly ISpecificationBuilder _specificationBuilder;
        
        /// <summary>
        /// Consolidated Filing workflow
        /// </summary>
        private readonly IConsolidatedFilingWorkflow<RailFilingHeader, RailDefValuesManual> _filingWorkflow;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileProcedureService" /> class
        /// </summary>
        /// <param name="parsedRepository">The parsed repository</param>
        /// <param name="manifestPdfGenerator">The manifest PDF generator</param>
        /// <param name="railDocumentFactory">The rail document factory</param>
        /// <param name="readModelRepository">Rail read model repository</param>
        /// <param name="specificationBuilder">The specification builder</param>
        /// <param name="filingWorkflow">Filing workflow</param>
        public FileProcedureService(
            IBdParsedRepository parsedRepository,
            IFileGenerator<RailFilingHeader> manifestPdfGenerator,
            IRailDocumentFactory railDocumentFactory,
            IRailInboundReadModelRepository readModelRepository,
            ISpecificationBuilder specificationBuilder,
            IConsolidatedFilingWorkflow<RailFilingHeader, RailDefValuesManual> filingWorkflow)
        {
            _parsedRepository = parsedRepository;
            _manifestPdfGenerator = manifestPdfGenerator;
            _railDocumentFactory = railDocumentFactory;
            _readModelRepository = readModelRepository;
            _specificationBuilder = specificationBuilder;
            _filingWorkflow = filingWorkflow;
        }

        /// <summary>
        /// Creates single-filing filing headers by specified record ids
        /// </summary>
        /// <param name="inboundIds">Inbound record ids</param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(IEnumerable<int> inboundIds, string userAccount = null) =>
            _filingWorkflow.StartSingleFiling(CreateSingleFilingHeaders(inboundIds, userAccount));

        /// <summary>
        /// Creates single-filing filing headers by specified record ids
        /// </summary>
        /// <param name="filtersSet">Filters for finding ids</param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int[]> CreateSingleFilingFilingHeaders(FiltersSet filtersSet, string userAccount = null) =>
            _filingWorkflow.StartSingleFiling(CreateSingleFilingHeaders(FiltersetToIds(filtersSet), userAccount));

        /// <summary>
        /// Creates the initial filing header by specified bd parsed record ids
        /// </summary>
        /// <param name="inboundIds">The record ids</param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int> CreateUnitTrainFilingHeader(IEnumerable<int> inboundIds, string userAccount = null) =>
            _filingWorkflow.StartUnitTradeFiling(CreateFilingHeader(inboundIds, userAccount));

        /// <summary>
        /// Creates the initial filing header by specified bd parsed record ids
        /// </summary>
        /// <param name="filtersSet">Filters for finding ids</param>
        /// <param name="userAccount">User whose profile will be used for data mapping</param>
        public OperationResultWithValue<int> CreateUnitTrainFilingHeader(FiltersSet filtersSet, string userAccount = null) =>
            CreateUnitTrainFilingHeader(FiltersetToIds(filtersSet), userAccount);

        internal IEnumerable<int> FiltersetToIds(FiltersSet filtersSet)
        {
            ISpecification specification = _specificationBuilder.Build<RailInboundReadModel>(filtersSet);
            IEnumerable<int> ids = _readModelRepository.GetAll<EntityDto>(specification, 100000, false).Select(x => x.Id);
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
        /// Cancel filing process for the specified filing header
        /// </summary>
        /// <param name="filingHeaderIds">The filing header</param>
        public void CancelFilingProcess(params int[] filingHeaderIds) => _filingWorkflow.CancelFilingProcess(filingHeaderIds);

        /// <summary>
        /// Refile specified inbound records and runs Filing or Updating process for it. If records are not specified then update all available records.
        /// </summary>
        /// <param name="filingHeaderIds">A collection of the filing header ids to update</param>
        /// <param name="user">The user who started the process</param>
        public OperationResultWithValue<int[]> Refile(IEnumerable<int> filingHeaderIds, string user) =>
            _filingWorkflow.Refile(filingHeaderIds, user);

        /// <summary>
        /// Adds the manifest document to specified rail filing header
        /// </summary>
        /// <param name="header">The rail filing header</param>
        internal void AddManifestDocument(RailFilingHeader header)
        {
            BinaryFileModel manifest = _manifestPdfGenerator.Generate(header);
            RailDocument railDocument = _railDocumentFactory.CreateManifest(manifest, null);

            header.AddDocuments(new[] { railDocument });
        }

        internal IEnumerable<RailFilingHeader> CreateSingleFilingHeaders(IEnumerable<int> inboundIds, string user)
        {
            IEnumerable<RailBdParsed> inbounds = _parsedRepository.GetList(inboundIds);

            foreach (RailBdParsed inbound in inbounds)
            {
                RailFilingHeader header = _filingWorkflow.CreateHeader(user);
                header.AddRailBdParseds(new[] { inbound });
                AddManifestDocument(header);

                yield return header;
            }
        }
        internal RailFilingHeader CreateFilingHeader(IEnumerable<int> inboundIds, string user)
        {
            IEnumerable<RailBdParsed> inbounds = _parsedRepository.GetList(inboundIds);

            RailFilingHeader header = _filingWorkflow.CreateHeader(user);
            header.AddRailBdParseds(inbounds);
            AddManifestDocument(header);

            return header;
        }
    }
}
