using System.Net.Http;
using FilingPortal.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class ReportExportControllerRoutesTests : ApiControllerTestsBase<ReportExportController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldExist()
        {
            const string route = "/api/reports/ExportToExcel?gridName=inbound_records&data=some_data";
            AssertRoute(HttpMethod.Get, route, x => x.ExportToExcel("inbound_records", "some_data"));
        }
    }
}