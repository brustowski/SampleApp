using System.Net.Http;
using System.Web.Http;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Controllers
{
    /// <summary>
    /// Controller provides actions for download documents 
    /// </summary>
    [RoutePrefix("api/canada-imp-truck/documents")]
    public class CanadaTruckImportDownloadDocumentsController : ApiControllerBase
    {
        /// <summary>
        /// The Document repository
        /// </summary>
        private readonly IDocumentRepository<Document> _documentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanadaTruckImportDownloadDocumentsController"/> class
        /// </summary>
        /// <param name="documentRepository">Repository for documents operations</param>
        public CanadaTruckImportDownloadDocumentsController(
            IDocumentRepository<Document> documentRepository)
        {
            _documentRepository = documentRepository;
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
            var docFileContent = _documentRepository.Get(id);
            return GetFileResponse(docFileContent.Content, docFileContent.FileName);
        }
    }
}
