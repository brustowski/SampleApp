using System.Net.Http;
using System.Web.Http;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.Web.Controllers.Truck
{
    /// <summary>
    /// Controller provides actions for download documents 
    /// </summary>
    [RoutePrefix("api/inbound/truck/documents")]
    public class TruckDownloadDocumentsController : ApiControllerBase
    {
        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IRailFilingHeadersRepository _filingHeadersRepository;
        /// <summary>
        /// The Truck Document repository
        /// </summary>
        private readonly IDocumentRepository<TruckDocument> _documentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDownloadDocumentsController"/> class
        /// </summary>
        /// <param name="filingHeadersRepository">The filing headers repository</param>
        /// <param name="documentRepository">Repository for documents operations</param>
        public TruckDownloadDocumentsController(
            IRailFilingHeadersRepository filingHeadersRepository, 
            IDocumentRepository<TruckDocument> documentRepository)
        {
            _filingHeadersRepository = filingHeadersRepository;
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Downloads document file by document identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        [HttpGet]
        [Route("{id:int}")]
        [PermissionRequired(Permissions.TruckViewInboundRecord)]
        public HttpResponseMessage DownloadDocument(int id)
        {
            var docFileContent = _documentRepository.Get(id);
            return GetFileResponse(docFileContent.Content, docFileContent.FileName);
        }
    }
}
