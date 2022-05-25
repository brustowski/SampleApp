using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FilingPortal.Domain;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Common.InboundTypes;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.Services.Filing;
using Framework.Infrastructure;

namespace FilingPortal.PluginEngine.Controllers
{
    /// <summary>
    /// Contains methods for auto-filing
    /// </summary>
    /// <typeparam name="TInboundType">Auto-file entity</typeparam>
    public abstract class AutoFileControllerBase<TInboundType> : ApiControllerBase
        where TInboundType : IAutoFilingEntity
    {
        private readonly IAutoFileService<TInboundType> _autofileService;

        /// <summary>
        /// Creates a new instance of <see cref="AutoFileControllerBase{TInboundType}"/>
        /// </summary>
        /// <param name="autofileService"></param>
        protected AutoFileControllerBase(IAutoFileService<TInboundType> autofileService)
        {
            _autofileService = autofileService;
        }

        /// <summary>
        /// Returns array of available permissions on auto-file process
        /// </summary>
        protected virtual IEnumerable<int> AvailablePermissions()
        {
            return new[] {(int) Permissions.AdminAutoCreateConfiguration};
        }

        /// <summary>
        /// Executes auto-file manually
        /// </summary>
        [AcceptVerbs("GET", "POST")]
        [Route("auto-file")]
        [PermissionRequired]
        public async Task<HttpResponseMessage> AutoFile()
        {
            if (!CurrentUser.HasPermissions(AvailablePermissions()))
            {
                AppLogger.Error(ErrorMessages.InsufficientPermissions);
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, ErrorMessages.InsufficientPermissions);
            }

            var result = await _autofileService.Execute(CurrentUser);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
