using System.Web.Http;
using FilingPortal.Domain;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.FieldConfigurations;

namespace FilingPortal.Web.Controllers.Rail
{
    /// <summary>
    /// Controller provides actions for Rail manifest
    /// </summary>
    [RoutePrefix("api/inbound/rail/manifest")]
    public class ManifestController : ApiControllerBase
    {
        /// <summary>
        /// The repository of parsed manifest record data
        /// </summary>
        private readonly IBdParsedRepository _repository;

        /// <summary>
        /// The Manifest factory
        /// </summary>
        private readonly IManifestFactory _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManifestController" /> class
        /// </summary>
        /// <param name="repository">The repository of parsed manifest record data</param>
        /// <param name="factory">The Manifest factory</param>
        public ManifestController(IBdParsedRepository repository, IManifestFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }

        /// <summary>
        /// Gets Edi message text of the selected record
        /// </summary>
        /// <param name="id">Edi message record identifier</param>
        [HttpGet]
        [Route("{id:int}")]
        [PermissionRequired(Permissions.RailViewManifest)]
        public IHttpActionResult GetRecordManifest(int id)
        {
            var manifest = _repository.GetManifest(id);
            if (manifest == null)
            {
                return BadRequest(ErrorMessages.RecordDoesNotExistError);
            }
            var result = _factory.CreateFrom(manifest);
            return Ok(result);
        }
    }
}
