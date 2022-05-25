using System.Web.Http;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Services.Filing;

namespace FilingPortal.Parts.Zones.Entry.Web.Controllers
{
    /// <summary>
    /// Controller for auto-file actions
    /// </summary>
    [RoutePrefix("api/zones/entry-06/filing")]
    public class ZonesEntryAutoFileController : AutoFileControllerBase<InboundRecord>
    {
        public ZonesEntryAutoFileController(IAutoFileService<InboundRecord> autofileService) : base(autofileService)
        {
        }
    }
}
