using FilingPortal.Domain.Services.Truck;
using FilingPortal.Web.Controllers.Truck;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Truck
{
    [TestClass]
    public class TruckSingleFilingGridControllerTests : ApiControllerFunctionTestsBase<TruckSingleFilingGridController>
    {
        Mock<ITruckSingleFilingGridService> _singleFilingGridService;

        protected override TruckSingleFilingGridController TestInitialize()
        {
            _singleFilingGridService = new Mock<ITruckSingleFilingGridService>();

            return new TruckSingleFilingGridController(_singleFilingGridService.Object);
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
