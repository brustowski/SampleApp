using System.Web.Http;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Services.Filing;

namespace FilingPortal.Parts.Zones.Ftz214.Web.Controllers
{
    /// <summary>
    /// Controller for auto-file actions
    /// </summary>
    [RoutePrefix("api/zones/ftz-214/filing")]
    public class ZonesFtz214AutoFileController : AutoFileControllerBase<InboundRecord>
    {
        public ZonesFtz214AutoFileController(IAutoFileService<InboundRecord> autofileService) : base(autofileService)
        {
        }
    }
}
