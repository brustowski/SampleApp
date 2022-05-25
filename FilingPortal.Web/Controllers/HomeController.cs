using System.Web.Mvc;

namespace FilingPortal.Web.Controllers
{
    /// <summary>
    /// Base controller for loading SPA application
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns default SPA page
        /// </summary>
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
