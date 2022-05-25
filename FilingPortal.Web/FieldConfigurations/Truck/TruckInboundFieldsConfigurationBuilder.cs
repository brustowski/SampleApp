using System.Linq;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Truck;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;

namespace FilingPortal.Web.FieldConfigurations.Truck
{
    /// <summary>
    /// Class for building field configuration for Truck Inbound Record filing process
    /// </summary>
    public class TruckInboundFieldsConfigurationBuilder: ITruckInboundFieldsConfigurationBuilder
    {
        /// <summary>
        /// The repository of <see cref="TruckDefValueManualReadModel"/> entities
        /// </summary>
        private readonly ITruckDefValuesManualReadModelRepository _repository;
        /// <summary>
        /// The factory for field configuration creation
        /// </summary>
        private readonly ITruckInboundFieldFactory _factory;
        /// <summary>
        /// The Truck Document repository
        /// </summary>
        private readonly IDocumentRepository<TruckDocument> _documentRepository;

        /// <summary>
        /// Inbound records repository
        /// </summary>
        private readonly IInboundRecordsRepository<TruckInbound> _inboundRecordsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundFieldsConfigurationBuilder" /> class
        /// </summary>
        /// <param name="repository">The repository of <see cref="TruckDefValueManualReadModel" /> entities</param>
        /// <param name="factory">The factory for parameter configuration creation</param>
        /// <param name="documentRepository">The Truck Document repository</param>
        /// <param name="inboundRecordsRepository">Inbound Records repository</param>
        public TruckInboundFieldsConfigurationBuilder(
            ITruckDefValuesManualReadModelRepository repository,
            ITruckInboundFieldFactory factory,
            IDocumentRepository<TruckDocument> documentRepository,
            IInboundRecordsRepository<TruckInbound> inboundRecordsRepository)
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
            var railDefValues = _repository.GetAdditionalParametersByFilingHeader(filingHeaderId);
            var commonDataValues = _repository.GetCommonDataByFilingHeader(filingHeaderId);

            var inboundRecordsIds = _inboundRecordsRepository.GetByFilingId(filingHeaderId).Select(x=>x.Id);

            var documents = _documentRepository.GetListByFilingHeader(filingHeaderId, inboundRecordsIds);

            var configuration = new InboundRecordFieldConfiguration
            {
                AdditionalParameters = railDefValues.Select(railDefValue => _factory.CreateFrom(railDefValue)).ToList(),
                CommonData = _factory.CreateSectionsFrom(commonDataValues).ToList(),
                Documents = documents.Map<TruckDocument, InboundRecordDocumentViewModel>().ToList()
            };

            return configuration;
        }
    }
}
