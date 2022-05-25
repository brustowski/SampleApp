using System.Net.Http;
using System.Web.Http;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.Web.Controllers.VesselExport
{
    /// <summary>
    /// Controller provides actions for download documents 
    /// </summary>
    [RoutePrefix("api/export/vessel/documents")]
    public class VesselExportDownloadDocumentsController : ApiControllerBase
    {
        /// <summary>
        /// The Vessel Export Document repository
        /// </summary>
        private readonly IDocumentRepository<VesselExportDocument> _documentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDownloadDocumentsController"/> class
        /// </summary>
        /// <param name="documentRepository">Repository for documents operations</param>
        public VesselExportDownloadDocumentsController(
            IDocumentRepository<VesselExportDocument> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Downloads document file by document identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        [HttpGet]
        [Route("{id:int}")]
        [PermissionRequired(Permissions.VesselViewExportRecord)]
        public HttpResponseMessage DownloadDocument(int id)
        {
            var docFileContent = _documentRepository.Get(id);
            return GetFileResponse(docFileContent.Content, docFileContent.FileName);
        }
    }
}
