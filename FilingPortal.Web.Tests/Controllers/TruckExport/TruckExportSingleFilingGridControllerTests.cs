using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Services;
using FilingPortal.Web.Controllers.TruckExport;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using FilingPortal.Web.Tests.Common;

namespace FilingPortal.Web.Tests.Controllers.TruckExport
{
    [TestClass]
    public class TruckSingleFilingGridControllerTests : ApiControllerFunctionTestsBase<TruckExportSingleFilingGridController>
    {
        private Mock<ISingleFilingGridService<TruckExportRecord>> _singleFilingGridService;

        protected override TruckExportSingleFilingGridController TestInitialize()
        {
            _singleFilingGridService = new Mock<ISingleFilingGridService<TruckExportRecord>>();

            return new TruckExportSingleFilingGridController(_singleFilingGridService.Object);
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/export/truck/single-filing-grid/gettotalmatches", x => x.GetTotalMatches(null));
            AssertRoute(HttpMethod.Post, "/api/export/truck/single-filing-grid/search", x => x.Search(null));            
        }

        [TestMethod]
        public void GetTotalMatches_Calls_SingleFilingGridService_Once()
        {
            // Assign
            var data = new SearchRequestModel();

            // Act
            Controller.GetTotalMatches(data);

            // Assert

            _singleFilingGridService.Verify(x => x.GetTotalMatches(data), Times.Once);
        }

        [TestMethod]
        public void Search_Calls_SingleFilingGridService_Once()
        {
            // Assign
            var data = new SearchRequestModel();

            // Act
            Controller.Search(data);

            // Assert

            _singleFilingGridService.Verify(x => x.GetData(data), Times.Once);
        }
    }
}
