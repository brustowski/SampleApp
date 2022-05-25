using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Mapping;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Rail;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Common.Grids;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.Models.InboundRecordModels;

namespace FilingPortal.Web.FieldConfigurations.InboundRecordParameters
{
    /// <summary>
    /// Class for building field configuration for Inbound Record filing process
    /// </summary>
    public class InboundRecordConfigurationBuilder : IInboundRecordConfigurationBuilder
    {
        /// <summary>
        /// The repository of <see cref="RailDefValuesManualReadModel"/> entities
        /// </summary>
        private readonly IDefValuesManualReadModelRepository<RailDefValuesManualReadModel> _repository;
        /// <summary>
        /// The factory for field configuration creation
        /// </summary>
        private readonly IInboundRecordFieldFactory _factory;
        /// <summary>
        /// The Rail Document repository
        /// </summary>
        private readonly IDocumentRepository<RailDocument> _documentRepository;
        private readonly IGridConfigRegistry _registry;
        /// <summary>
        /// Inbound records repository
        /// </summary>
        private readonly IInboundRecordsRepository<RailBdParsed> _inboundRecordsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordConfigurationBuilder" /> class
        /// </summary>
        /// <param name="repository">The repository of <see cref="RailDefValuesManualReadModel" /> entities</param>
        /// <param name="factory">The factory for parameter configuration creation</param>
        /// <param name="documentRepository">The Rail Document repository</param>
        /// <param name="registry">The configuration registry</param>
        /// <param name="inboundRecordsRepository">Inbound records repository</param>
        public InboundRecordConfigurationBuilder(IDefValuesManualReadModelRepository<RailDefValuesManualReadModel> repository, 
            IInboundRecordFieldFactory factory, 
            IDocumentRepository<RailDocument> documentRepository, 
            IGridConfigRegistry registry,
            IInboundRecordsRepository<RailBdParsed> inboundRecordsRepository)
        {
            _repository = repository;
            _factory = factory;
            _documentRepository = documentRepository;
            _registry = registry;
            _inboundRecordsRepository = inboundRecordsRepository;
        }

        /// <summary>
        /// Builds configuration using the specified filing header identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        public InboundRecordFieldConfiguration Build(int filingHeaderId)
        {
            IEnumerable<RailDefValuesManualReadModel> railDefValues = _repository.GetAdditionalParametersByFilingHeader(filingHeaderId);
            IEnumerable<RailDefValuesManualReadModel> commonDataValues = _repository.GetCommonDataByFilingHeader(filingHeaderId);

            var inboundRecordIds = _inboundRecordsRepository.GetByFilingId(filingHeaderId).Select(x => x.Id);

            IEnumerable<RailDocument> documents = _documentRepository.GetListByFilingHeader(filingHeaderId, inboundRecordIds);

            var configuration = new InboundRecordFieldConfiguration
            {
                AdditionalParameters = _factory.CreateFrom(railDefValues).ToList(),
                CommonData = _factory.CreateSectionsFrom(commonDataValues).ToList(),
                Documents = documents.Map<RailDocument, InboundRecordDocumentViewModel>().OrderBy(x => x.IsManifest).ToList()
            };

            return configuration;
        }

        /// <summary>
        /// Builds configuration for single-filing
        /// </summary>
        /// <param name="filingHeaderIds">Requested ids</param>
        public InboundRecordFieldConfiguration BuildSingleFiling(int[] filingHeaderIds)
        {
            IGridConfiguration gridConfig = _registry.GetGridConfig(GridNames.RailSingleFilingGrid);
            gridConfig.Configure();
            IEnumerable<string> columnNames = gridConfig.GetColumns().Select(x => x.DisplayName);

            IEnumerable<RailDefValuesManualReadModel> singleFilingData = _repository.GetSingleFilingData(filingHeaderIds, columnNames);

            var configuration = new InboundRecordFieldConfiguration
            {
                AdditionalParameters = _factory.CreateFrom(singleFilingData).ToList()
            };

            return configuration;
        }
    }
}
