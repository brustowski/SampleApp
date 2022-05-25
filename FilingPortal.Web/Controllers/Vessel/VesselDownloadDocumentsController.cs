using System.Net.Http;
using System.Web.Http;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.Web.Controllers.Vessel
{
    /// <summary>
    /// Controller provides actions for download documents 
    /// </summary>
    [RoutePrefix("api/inbound/vessel/documents")]
    public class VesselDownloadDocumentsController : ApiControllerBase
    {
        /// <summary>
        /// The Truck Export Document repository
        /// </summary>
        private readonly IDocumentRepository<VesselImportDocument> _documentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselDownloadDocumentsController"/> class
        /// </summary>
        /// <param name="documentRepository">Repository for documents operations</param>
        public VesselDownloadDocumentsController(
            IDocumentRepository<VesselImportDocument> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Downloads document file by document identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        [HttpGet]
        [Route("{id:int}")]
        [PermissionRequired(Permissions.VesselViewImportRecord)]
        public HttpResponseMessage DownloadDocument(int id)
        {
            var docFileContent = _documentRepository.Get(id);
            return GetFileResponse(docFileContent.Content, docFileContent.FileName);
        }
    }
}
