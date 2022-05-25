using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Enums;
using FilingPortal.Parts.Zones.Ftz214.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using System.Net.Http;
using System.Web.Http;

namespace FilingPortal.Parts.Zones.Ftz214.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for download documents
    /// </summary>
    [RoutePrefix("api/zones/ftz-214/documents")]
    public class ZonesFtz214DownloadDocumentsController : ApiControllerBase
    {
        /// <summary>
        /// The Document repository
        /// </summary>
        private readonly IDocumentRepository<Document> _documentRepository;

        private readonly IInboundRecordsRepository _inboundRecordsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZonesFtz214DownloadDocumentsController"/> class
        /// </summary>
        /// <param name="documentRepository">Repository for documents operations</param>
        /// <param name="inboundRecordsRepository">Inbound Records Repository</param>
        public ZonesFtz214DownloadDocumentsController(
            IDocumentRepository<Document> documentRepository,
            IInboundRecordsRepository inboundRecordsRepository)
        {
            _documentRepository = documentRepository;
            _inboundRecordsRepository = inboundRecordsRepository;
        }

        /// <summary>
        /// Downloads document file by document identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        [HttpGet]
        [Route("{id:int}")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public HttpResponseMessage DownloadDocument(int id)
        {
            Document docFileContent = _documentRepository.Get(id);
            return GetFileResponse(docFileContent.Content, docFileContent.FileName);
        }

        /// <summary>
        /// Downloads document file by document identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        [HttpGet]
        [Route("inbound/{id:int}")]
        [PermissionRequired(Permissions.ViewInboundRecord)]
        public HttpResponseMessage DownloadInboundDocument(int id)
        {
            InboundRecord inboundRecord = _inboundRecordsRepository.Get(id);
            return GetFileResponse(inboundRecord.XmlFile, inboundRecord.XmlFileName);
        }
    }
}
