using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class InboundRecordConfigurationControllerTests : ApiControllerFunctionTestsBase<InboundRecordConfigurationController>
    {
        private Mock<IInboundRecordConfigurationBuilder> _builderMock;

        protected override InboundRecordConfigurationController TestInitialize()
        {
            _builderMock = new Mock<IInboundRecordConfigurationBuilder>();

            return new InboundRecordConfigurationController(_builderMock.Object);
        }
        
        [TestMethod]
        public void GetConfiguration_WithFilingHeaderId_CallsBuilder()
        {
            int filingHeaderId = 876;

            _builderMock.Setup(x => x.Build(It.IsAny<int>())).Returns(new InboundRecordFieldConfiguration());

            Controller.GetConfiguration(filingHeaderId);

            _builderMock.Verify(x => x.Build(filingHeaderId), Times.Once);
        }
        
        [TestMethod]
        public void GetConfiguration_WithFilingHeaderId_ReturnsConfiguration()
        {
            int filingHeaderId = 876;
            var config = new InboundRecordFieldConfiguration();

            _builderMock.Setup(x => x.Build(It.IsAny<int>())).Returns(config);

            var result = Controller.GetConfiguration(filingHeaderId);

            Assert.AreEqual(config, result);
        }

        [TestMethod]
        public void SingleFilingGridContents_Calls_Configuration()
        {
            // Assign
            var ids = new[] { 1, 2, 3 };

            // Act 
            var result = Controller.SingleFilingGridContents(ids);

            // Assert
            _builderMock.Verify(x => x.BuildSingleFiling(ids), Times.AtMostOnce);
        }
        [TestMethod]
        public void SingleFilingGridContents_Returns_Configuration()
        {
            // Assign
            var ids = new[] { 1, 2, 3 };
            var config = new InboundRecordFieldConfiguration();
            _builderMock.Setup(x => x.BuildSingleFiling(ids)).Returns(config);

            // Act 
            var result = Controller.SingleFilingGridContents(ids);

            // Assert
            Assert.AreEqual(result, config);
        }
    }
}
