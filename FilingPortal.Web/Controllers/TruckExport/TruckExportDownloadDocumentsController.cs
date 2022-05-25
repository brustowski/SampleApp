using System.Net.Http;
using System.Web.Http;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.Web.Controllers.TruckExport
{
    /// <summary>
    /// Controller provides actions for download documents 
    /// </summary>
    [RoutePrefix("api/export/truck/documents")]
    public class TruckExportDownloadDocumentsController : ApiControllerBase
    {
        /// <summary>
        /// The Truck Export Document repository
        /// </summary>
        private readonly IDocumentRepository<TruckExportDocument> _documentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDownloadDocumentsController"/> class
        /// </summary>
        /// <param name="documentRepository">Repository for documents operations</param>
        public TruckExportDownloadDocumentsController(
            IDocumentRepository<TruckExportDocument> documentRepository)
        {
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Downloads document file by document identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        [HttpGet]
        [Route("{id:int}")]
        [PermissionRequired(Permissions.TruckViewExportRecord)]
        public HttpResponseMessage DownloadDocument(int id)
        {
            var docFileContent = _documentRepository.Get(id);
            return GetFileResponse(docFileContent.Content, docFileContent.FileName);
        }
    }
}
