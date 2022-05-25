using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Truck;
using FilingPortal.Domain.Services.Truck;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.Web.Controllers.Truck;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Web.Tests.Controllers.Truck
{
    [TestClass]
    public class TruckInboundFilingControllerTests : ApiControllerFunctionTestsBase<TruckInboundFilingController>
    {
        private Mock<ITruckFilingService> _procedureServiceMock;
        private Mock<ITruckFilingHeadersRepository> _filingHeadersRepositoryMock;
        private Mock<IFilingConfigurationFactory<TruckDefValueManualReadModel>> _configurationFactoryMock;

        protected override TruckInboundFilingController TestInitialize()
        {
            _procedureServiceMock = new Mock<ITruckFilingService>();
            _filingHeadersRepositoryMock = new Mock<ITruckFilingHeadersRepository>();
            _configurationFactoryMock = new Mock<IFilingConfigurationFactory<TruckDefValueManualReadModel>>();

            // CreateInitialFilingHeader should always return result
            _procedureServiceMock.Setup(x => x.CreateSingleFilingFilingHeaders(It.IsAny<IEnumerable<int>>(), It.IsAny<string>()))
                .Returns(new OperationResultWithValue<int[]> { Value = new[] { 1 } });

            return new TruckInboundFilingController(_procedureServiceMock.Object, _filingHeadersRepositoryMock.Object, _configurationFactoryMock.Object);
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/inbound/truck/filing/start", x => x.Start(null));
            AssertRoute(HttpMethod.Get, "/api/inbound/truck/filing/validate/1", x => x.ValidateFilingHeaderId(1));
            AssertRoute(HttpMethod.Post, "/api/inbound/truck/filing/validate", x => x.ValidateFilingHeaderIds(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/truck/filing/cancel", x => x.CancelFilingProcess(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/truck/filing/record-ids", x => x.GetInboundRecordIdsByFilingHeaders(null));
            AssertRoute(HttpMethod.Get, "/api/inbound/truck/filing/configuration/1", x => x.GetAllSections(1));
            AssertRoute(HttpMethod.Post, "/api/inbound/truck/filing/configuration/1/sectionName/1"
                , x => x.AddConfigurationRecord(1, "sectionName", 1));
            AssertRoute(HttpMethod.Delete, "/api/inbound/truck/filing/configuration/1/sectionName/1"
                , x => x.DeleteConfigurationRecord(1, "sectionName", 1));
        }

        [TestMethod]
        public void Start_CallsService_WithCorrectParameters()
        {
            // Assign
            var ids = new[] { 0, 1, 2 };

            // Act
            Controller.Start(ids);

            //Assert
            _procedureServiceMock.Verify(x => x.CreateSingleFilingFilingHeaders(ids, Controller.CurrentUser.Id), Times.Once);
        }

        [TestMethod]
        public void ValidateFilingHeaderId_ReturnsTrue_IfFilingHeaderCanBeEdited()
        {
            // Assign
            var filingHeaderId = 1;
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new TruckFilingHeader { MappingStatus = MappingStatus.InReview });

            // Act
            var result = Controller.ValidateFilingHeaderId(filingHeaderId);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateFilingHeaderId_ReturnsFalse_IfFilingHeaderCanNotBeEdited()
        {
            // Assign
            var filingHeaderId = 1;
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new TruckFilingHeader { MappingStatus = MappingStatus.InProgress });

            // Act
            var result = Controller.ValidateFilingHeaderId(filingHeaderId);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateFilingHeaderId_ReturnsFalse_ForNonexistentFilingHeaderId()
        {
            // Assign
            var filingHeaderId = 1;
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns<TruckFilingHeader>(null);

            // Act
            var result = Controller.ValidateFilingHeaderId(filingHeaderId);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateFilingHeaderIds_ReturnsTrue_IfAllFilingHeadersCanBeEdited()
        {
            // Assign
            var filingHeaderIds = new[] { 1, 2, 3 };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(new[]
            {
                new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
                , new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
                , new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
            });

            // Act
            var result = Controller.ValidateFilingHeaderIds(filingHeaderIds);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateFilingHeaderIds_ReturnsFalse_IfAnyOfFilingHeadersCanNotBeEdited()
        {
            // Assign
            var filingHeaderIds = new[] { 1, 2, 3 };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(new[]
            {
                new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
                , new TruckFilingHeader { MappingStatus = MappingStatus.InProgress }
                , new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
            });

            // Act
            var result = Controller.ValidateFilingHeaderIds(filingHeaderIds);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateFilingHeaderIds_ReturnsFalse_ForNonexistentFilingHeaderIds()
        {
            // Assign
            var filingHeaderIds = new[] { 1, 2, 3 };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(Enumerable.Empty<TruckFilingHeader>());
            // Act
            var result = Controller.ValidateFilingHeaderIds(filingHeaderIds);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CancelFilingProcess_ReturnsOk_IfFilingProcessCanceled()
        {
            // Assign
            var filingHeaderIds = new[] { 1, 2, 3 };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(new[]
            {
                new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
                , new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
                , new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
            });

            // Act
            IHttpActionResult result = Controller.CancelFilingProcess(filingHeaderIds);

            //Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));

        }

        [TestMethod]
        public void CancelFilingProcess_ReturnsBadRequest_IfSomeFilingHeaderCanNotBeCanceled()
        {
            // Assign
            var filingHeaderIds = new[] { 1, 2, 3 };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(new[]
            {
                new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
                , new TruckFilingHeader { MappingStatus = MappingStatus.InProgress}
                , new TruckFilingHeader { MappingStatus = MappingStatus.InReview }
            });

            // Act
            IHttpActionResult result = Controller.CancelFilingProcess(filingHeaderIds);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

        }

        [TestMethod]
        public void CancelFilingProcess_ReturnsBadRequest_ForNonexistentFilingHeaderIds()
        {
            // Assign
            var filingHeaderIds = new[] { 1, 2, 3 };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(Enumerable.Empty<TruckFilingHeader>);

            // Act
            IHttpActionResult result = Controller.CancelFilingProcess(filingHeaderIds);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

        }

        [TestMethod]
        public void GetInboundRecordIdsByFilingHeaders_Returns_FilingHeaderRecordIds()
        {
            // Assign
            var filingHeaderIds = new[] { 1 };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(new[]
            {
                new TruckFilingHeader { TruckInbounds = new []
                    {
                        new TruckInbound { Id = 1}
                        , new TruckInbound { Id = 2}
                    }
                }
            });

            // Act
            IEnumerable<int> result = Controller.GetInboundRecordIdsByFilingHeaders(filingHeaderIds);

            // Assert
            CollectionAssert.AreEqual(new[] { 1, 2 }, result.ToArray());
        }

        [TestMethod]
        public void GetAllSections_CallsCreateConfiguration_ConfigurationFactory()
        {
            var filingHeaderId = 1;

            Controller.GetAllSections(filingHeaderId);

            _configurationFactoryMock.Verify(x => x.CreateConfiguration(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void AddConfigurationRecord_CallsGet_FilingHeaderRepository()
        {
            var filingHeaderId = 1;
            var sectionName = "test_section";
            var recordId = 1;

            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new TruckFilingHeader { Id = filingHeaderId });

            Controller.AddConfigurationRecord(filingHeaderId, sectionName, recordId);

            _filingHeadersRepositoryMock.Verify(x => x.Get(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void AddConfigurationRecord_CallsAddSectionRow_FilingHeaderRepository_WhenCalled()
        {
            var filingHeaderId = 1;
            var sectionName = "test_section";
            var parentRecordId = 1;
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new TruckFilingHeader { Id = filingHeaderId, FilingStatus = FilingStatus.Open });

            Controller.AddConfigurationRecord(filingHeaderId, sectionName, parentRecordId);

            _filingHeadersRepositoryMock.Verify(x => x.AddSectionRecord(sectionName, filingHeaderId, parentRecordId, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AddConfigurationRecord_CallsCreateConfigurationForSection_ConfigurationFactory()
        {
            var filingHeaderId = 1;
            var sectionName = "test_section";
            var parentRecordId = 1;
            var result = Guid.NewGuid();
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new TruckFilingHeader { Id = filingHeaderId, FilingStatus = FilingStatus.Open });
            _filingHeadersRepositoryMock.Setup(x => x.AddSectionRecord(sectionName, filingHeaderId, parentRecordId, It.IsAny<string>())).Returns(result);

            Controller.AddConfigurationRecord(filingHeaderId, sectionName, parentRecordId);

            _configurationFactoryMock.Verify(x => x.CreateConfigurationForSection(filingHeaderId, sectionName, result), Times.Once);
        }
    }
}
