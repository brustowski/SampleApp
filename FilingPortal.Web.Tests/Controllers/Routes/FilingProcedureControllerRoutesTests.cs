using FilingPortal.Web.Controllers.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace FilingPortal.Web.Tests.Controllers.Routes
{
    [TestClass]
    public class FilingProcedureControllerRoutesTests : ApiControllerTestsBase<FilingProcedureController>
    {
        [TestMethod]
        public void Start_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/filing/start";
            AssertRoute(HttpMethod.Post, route, x => x.Start(null));
        }

        [TestMethod]
        public void StartFilterd_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/filing/start-filtered";
            AssertRoute(HttpMethod.Post, route, x => x.StartFiltered(null));
        }

        [TestMethod]
        public void StartUnitTrain_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/filing/start-unit-train";
            AssertRoute(HttpMethod.Post, route, x => x.StartUnitTrain(null));
        }

        [TestMethod]
        public void StartUnitTrainFilterd_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/filing/start-unit-train-filtered";
            AssertRoute(HttpMethod.Post, route, x => x.StartUnitTrainFiltered(null));
        }

        [TestMethod]
        public void ValidateFilingHeaderId_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/filing/validate/23";
            AssertRoute(HttpMethod.Get, route, x => x.ValidateFilingHeaderId(23));
        }

        [TestMethod]
        public void ValidateFilingHeaderIds_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/filing/validate";
            AssertRoute(HttpMethod.Post, route, x => x.ValidateFilingHeaderIds(new[] { 23 }));
        }

        [TestMethod]
        public void CancelFilingProcess_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/filing/cancel";
            AssertRoute(HttpMethod.Post, route, x => x.CancelFilingProcess(new[] { 23 }));
        }

        [TestMethod]
        public void GetInboundRecordIdsByFilingHeaders_RouteShouldBeExist()
        {
            const string route = "/api/inbound/rail/filing/record-ids";
            AssertRoute(HttpMethod.Post, route, x => x.GetInboundRecordIdsByFilingHeaders(new[] { 23 }));
        }
    }
}