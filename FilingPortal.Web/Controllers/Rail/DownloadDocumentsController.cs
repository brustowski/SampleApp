using System.Net.Http;
using System.Web.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller provides actions for download documents 
    /// </summary>
    [RoutePrefix("api/inbound/rail/documents")]
    public class DownloadDocumentsController : ApiControllerBase
    {
        /// <summary>
        /// The filing headers repository
        /// </summary>
        private readonly IRailFilingHeadersRepository _filingHeadersRepository;
        /// <summary>
        /// The manifest PDF generator
        /// </summary>
        private readonly IFileGenerator<RailFilingHeader> _manifestPdfGenerator;
        /// <summary>
        /// The Rail Document repository
        /// </summary>
        private readonly IDocumentRepository<RailDocument> _documentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadDocumentsController"/> class
        /// </summary>
        /// <param name="filingHeadersRepository">The filing headers repository</param>
        /// <param name="manifestPdfGenerator">The manifest PDF generator</param>
        /// <param name="documentRepository">Repository for documents operations</param>
        public DownloadDocumentsController(IRailFilingHeadersRepository filingHeadersRepository
            , IFileGenerator<RailFilingHeader> manifestPdfGenerator
            , IDocumentRepository<RailDocument> documentRepository)
        {
            _filingHeadersRepository = filingHeadersRepository;
            _manifestPdfGenerator = manifestPdfGenerator;
            _documentRepository = documentRepository;
        }

        /// <summary>
        /// Downloads the manifest for filing header specified by identifier
        /// </summary>
        /// <param name="filingHeaderId">The filing header identifier</param>
        [HttpGet]
        [Route("manifest/{filingHeaderId:int}")]
        [PermissionRequired(Permissions.RailViewManifest)]
        public HttpResponseMessage DownloadManifest(int filingHeaderId)
        {
            RailFilingHeader filingHeader = _filingHeadersRepository.GetRailFilingHeaderWithDetails(filingHeaderId);

            BinaryFileModel file = _manifestPdfGenerator.Generate(filingHeader);

            return GetFileResponse(file);
        }

        /// <summary>
        /// Downloads document file by document identifier
        /// </summary>
        /// <param name="id">The document identifier</param>
        [HttpGet]
        [Route("{id:int}")]
        [PermissionRequired(Permissions.RailViewInboundRecord)]
        public HttpResponseMessage DownloadDocument(int id)
        {
            RailDocument docFileContent = _documentRepository.Get(id);
            return GetFileResponse(docFileContent.Content, docFileContent.FileName);
        }
    }
}
