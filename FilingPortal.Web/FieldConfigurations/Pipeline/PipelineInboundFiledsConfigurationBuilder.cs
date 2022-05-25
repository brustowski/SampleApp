using System.Linq;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.Pipeline
{
    /// <summary>
    /// Class for building field configuration for Pipeline Inbound Record filing process
    /// </summary>
    public class PipelineInboundFieldsConfigurationBuilder: IPipelineInboundFieldsConfigurationBuilder
    {
        /// <summary>
        /// The repository of <see cref="PipelineDefValueManualReadModel"/> entities
        /// </summary>
        private readonly IPipelineDefValuesManualReadModelRepository _repository;
        /// <summary>
        /// The factory for field configuration creation
        /// </summary>
        private readonly IPipelineInboundFieldFactory _factory;
        /// <summary>
        /// The Pipeline Document repository
        /// </summary>
        private readonly IDocumentRepository<PipelineDocument> _documentRepository;

        /// <summary>
        /// Inbound records repository
        /// </summary>
        private readonly IInboundRecordsRepository<PipelineInbound> _inboundRecordsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundFieldsConfigurationBuilder" /> class
        /// </summary>
        /// <param name="repository">The repository of <see cref="PipelineDefValueManualReadModel" /> entities</param>
        /// <param name="factory">The factory for parameter configuration creation</param>
        /// <param name="documentRepository">The Pipeline Document repository</param>
        /// <param name="inboundRecordsRepository">Inbound Records repository</param>
        public PipelineInboundFieldsConfigurationBuilder(
            IPipelineDefValuesManualReadModelRepository repository,
            IPipelineInboundFieldFactory factory,
            IDocumentRepository<PipelineDocument> documentRepository,
            IInboundRecordsRepository<PipelineInbound> inboundRecordsRepository)
        {
            _repository = repository;
            _factory = factory;
            _documentRepository = documentRepository;
            _inboundRecordsRepository = inboundRecordsRepository;
        }

        /// <summary>
        /// Builds configuration using the specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public InboundRecordFieldConfiguration Build(int filingHeaderId)
        {
            var defValues = _repository.GetAdditionalParametersByFilingHeader(filingHeaderId);
            var commonDataValues = _repository.GetCommonDataByFilingHeader(filingHeaderId);

            var inboundRecords = _inboundRecordsRepository.GetByFilingId(filingHeaderId).Select(x=>x.Id);

            var documents = _documentRepository.GetListByFilingHeader(filingHeaderId, inboundRecords);

            var configuration = new InboundRecordFieldConfiguration
            {
                AdditionalParameters = defValues.Select(defValue => _factory.CreateFrom(defValue)).ToList(),
                CommonData = _factory.CreateSectionsFrom(commonDataValues).ToList(),
                Documents = documents.Map<PipelineDocument, InboundRecordDocumentViewModel>().ToList()
            };

            return configuration;
        }
    }
}
