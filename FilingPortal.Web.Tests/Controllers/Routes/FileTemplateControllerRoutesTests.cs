using System.Net.Http;
using FilingPortal.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class FileTemplateControllerRoutesTests : ApiControllerTestsBase<FileTemplateController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldBeExist()
        {
            const string route = "/api/file-template/by-grid-name/test";
            AssertRoute(HttpMethod.Get, route, x => x.GetByGridName("test"));
        }
    }
}