using System.Net.Http;
using System.Web.Http;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.Web.Controllers.Pipeline
{
    /// <summary>
    /// Controller provides actions for download documents 
    /// </summary>
    [RoutePrefix("api/inbound/pipeline/documents")]
    public class PipelineDownloadDocumentsController : ApiControllerBase
    {
        /// <summary>
        /// The Pipeline Document repository
        /// </summary>
        private readonly IDocumentRepository<PipelineDocument> _documentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineDownloadDocumentsController"/> class
        /// </summary>
        /// <param name="documentRepository">Repository for documents operations</param>
        public PipelineDownloadDocumentsController(
            IDocumentRepository<PipelineDocument> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Downloads document file by document identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        [HttpGet]
        [Route("{id:int}")]
        [PermissionRequired(Permissions.PipelineViewInboundRecord)]
        public HttpResponseMessage DownloadDocument(int id)
        {
            var docFileContent = _documentRepository.Get(id);
            return GetFileResponse(docFileContent.Content, docFileContent.FileName);
        }
    }
}
