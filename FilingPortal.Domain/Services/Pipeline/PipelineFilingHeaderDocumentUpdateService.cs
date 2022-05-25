using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Pipeline;
using Framework.Domain.Repositories;
using Framework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Services.Pipeline
{
    /// <summary>
    /// Service that updates filing header documents
    /// </summary>
    public class PipelineFilingHeaderDocumentUpdateService : FilingHeaderDocumentUpdateService<PipelineDocumentDto, PipelineDocument>, IPipelineFilingHeaderDocumentUpdateService
    {
        /// <summary>
        /// Pipeline inbound records repository
        /// </summary>
        private readonly ISearchRepository<PipelineInbound> _pipelineRepository;

        /// <summary>
        /// The api calculator file generator
        /// </summary>
        private readonly IFileGenerator<PipelineInbound> _fileGenerator;

        /// <summary>
        /// The def value repository
        /// </summary>
        private readonly IPipelineDefValuesReadModelRepository _defValuesRepository;

        /// <summary>
        /// Document types that may be uploaded only to inbound record with corresponding batch code
        /// </summary>
        private readonly string[] _processableDocumentTypes = { "DKT", "COR" };

        /// <summary>
        /// Creates new instance of <see cref="PipelineFilingHeaderDocumentUpdateService"/>
        /// </summary>
        /// <param name="pipelineRepository">Pipeline inbound records repository</param>
        /// <param name="documentRepository">Pipeline documents repository</param>
        /// <param name="documentFactory">Pipeline documents factory</param>
        /// <param name="fileGenerator">The api calculator file generator</param>
        /// <param name="defValuesRepository">The def value repository</param>
        public PipelineFilingHeaderDocumentUpdateService(
            ISearchRepository<PipelineInbound> pipelineRepository,
            IDocumentRepository<PipelineDocument> documentRepository,
            IPipelineDocumentFactory documentFactory,
            IFileGenerator<PipelineInbound> fileGenerator,
            IPipelineDefValuesReadModelRepository defValuesRepository) : base(documentRepository, documentFactory)
        {
            _pipelineRepository = pipelineRepository;
            _fileGenerator = fileGenerator;
            _defValuesRepository = defValuesRepository;
        }

        /// <summary>
        /// Mass upload documents to inbound records
        /// </summary>
        /// <param name="inboundRecordIds">Inbound Records ids, where this document should be added</param>
        /// <param name="dtos">Document DTO collection</param>
        public override void UploadDocumentsToInboundRecords(int[] inboundRecordIds, IEnumerable<PipelineDocumentDto> dtos)
        {
            var pipelineList = _pipelineRepository.GetList(inboundRecordIds).ToList();

            var documentsDtoList = dtos.ToList();
            foreach (PipelineInbound inboundRecord in pipelineList)
            {
                var inboundDocsList = DocumentRepository.GetListByInboundRecord(inboundRecord.Id).Select(x =>
                    new
                    {
                        x.Id,
                        x.DocumentType
                    }).ToList();

                foreach (PipelineDocumentDto dto in documentsDtoList.Where(x => x.Status == InboundRecordDocumentStatus.New))
                {
                    if (FileMatches(dto, inboundRecord))
                    {
                        PipelineDocument newDocument = DocumentFactory.CreateFromDto(dto, "sa");
                        newDocument.InboundRecordId = inboundRecord.Id;
                        DocumentRepository.AddOrUpdate(newDocument);

                        var sameDocType = inboundDocsList.FirstOrDefault(x => x.DocumentType == dto.DocumentType);
                        if (sameDocType != null)
                        {
                            DocumentRepository.DeleteById(sameDocType.Id);
                        }
                    }
                }
            }

            DocumentRepository.Save();
        }

        /// <summary>
        /// Updates existing API Calculator file
        /// </summary>
        /// <param name="parameters">The filing parameters</param>
        public void UpdateApiCalculator(InboundRecordFilingParameters parameters)
        {
            var apiCalc = GenerateApiCalculator(parameters);

            IEnumerable<PipelineDocument> documents = DocumentRepository.GetListByFilingHeader(parameters.FilingHeaderId, Enumerable.Empty<int>());
            PipelineDocument document = documents.FirstOrDefault(x => x.DocumentType == "CAL");

            if (document == null)
            {
                document = ((IPipelineDocumentFactory)DocumentFactory).CreateApiCalculator(apiCalc, "sa");
                document.FilingHeaderId = parameters.FilingHeaderId;
            }
            else
            {
                document.Content = apiCalc.Content;
            }

            DocumentRepository.AddOrUpdate(document);
            DocumentRepository.Save();
        }

        /// <summary>
        /// Generates API Calculator file
        /// </summary>
        /// <param name="parameters">The filing parameters</param>
        public BinaryFileModel GenerateApiCalculator(InboundRecordFilingParameters parameters)
        {
            IEnumerable<PipelineDefValueReadModel> config = _defValuesRepository.GetAll().ToList();
            PipelineDefValueReadModel apiConfig = config.FirstOrDefault(x => x.ColumnName.Equals("attribute2", StringComparison.InvariantCultureIgnoreCase));
            InboundRecordParameter api = parameters.Parameters.FirstOrDefault(x => x.Id == apiConfig?.Id);
            PipelineDefValueReadModel invoiceQtyConfig = config.FirstOrDefault(x => x.ColumnName.Equals("invoice_qty", StringComparison.CurrentCultureIgnoreCase));
            InboundRecordParameter invoiceQty = parameters.Parameters.FirstOrDefault(x => x.Id == invoiceQtyConfig?.Id);

            if (api == null)
            {
                AppLogger.Warning("API calculator can't be updated because API value is missed.");
                return null;
            }

            if (invoiceQty == null)
            {
                AppLogger.Warning("API calculator can't be updated because invoice quantity value is missed.");
                return null;
            }

            var inbound = new PipelineInbound
            {
                API = Convert.ToDecimal(api.Value, CultureInfo.InvariantCulture),
                Quantity = Convert.ToDecimal(invoiceQty.Value, CultureInfo.InvariantCulture)
            };

            return _fileGenerator.Generate(inbound);
        }

        /// <summary>
        /// Tests if file may be uploaded to inbound record
        /// </summary>
        /// <param name="file">File to save in database</param>
        /// <param name="inbound">Inbound record</param>
        private bool FileMatches(PipelineDocumentDto file, PipelineInbound inbound)
        {
            if (string.IsNullOrWhiteSpace(inbound.Batch) ||
                !_processableDocumentTypes.Contains(file.DocumentType, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (file.FileName.ToLower().Contains(inbound.Batch.ToLower()))
            {
                return true;
            }

            return false;
        }
    }
}
