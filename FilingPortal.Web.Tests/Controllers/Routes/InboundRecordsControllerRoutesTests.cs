using System.Net.Http;
using FilingPortal.Web.Controllers.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class InboundRecordsControllerRoutesTests : ApiControllerTestsBase<InboundRecordsController>
    {
        [TestMethod]
        public void GetTotalMatches_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/gettotalmatches";
            AssertRoute(HttpMethod.Post, route, x => x.GetTotalMatches(null));
        }
        
        [TestMethod]
        public void Search_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/search";
            AssertRoute(HttpMethod.Post, route, x => x.Search(null));
        }

        [TestMethod]
        public void Delete_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/delete";
            AssertRoute(HttpMethod.Post, route, x => x.Delete(null));
        }

        [TestMethod]
        public void Restore_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/restore";
            AssertRoute(HttpMethod.Post, route, x => x.Restore(null));
        }

        [TestMethod]
        public void ValidateSelectedRecords_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/validate-selected-records";
            AssertRoute(HttpMethod.Post, route, x => x.ValidateSelectedRecords(null));
        }

        [TestMethod]
        public void ValidateFilteredRecords_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/validate-filtered-records";
            AssertRoute(HttpMethod.Post, route, x => x.ValidateFilteredRecords(null));
        }

        [TestMethod]
        public void Validate_Add_New_Manifest()
        {
            const string route = "/api/inbound/rail/save-inbound";
            AssertRoute(HttpMethod.Post, route, x => x.AddOrEditRecord(null));
        }

        [TestMethod]
        public void Validate_GetEditModel()
        {
            const string route = "/api/inbound/rail/1";
            AssertRoute(HttpMethod.Get, route, x => x.GetEditModel(1));
        }
    }
}