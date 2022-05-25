using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Repositories.TruckExport;
using FilingPortal.Domain.Services.TruckExport;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.Web.Controllers.TruckExport;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Results;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Web.Tests.Controllers.TruckExport
{
    [TestClass]
    public class TruckExportFilingControllerTests : ApiControllerFunctionTestsBase<TruckExportFilingController>
    {
        private Mock<ITruckExportFilingService> _procedureServiceMock;
        private Mock<ITruckExportFilingHeadersRepository> _filingHeadersRepositoryMock;
        private Mock<IFilingConfigurationFactory<TruckExportDefValuesManualReadModel>> _configurationBuilderMock;
        private Mock<ITruckExportAutoRefileService> _autoRefileServiceMock;
        private Mock<ITruckExportRepository> _truckExportRepository;

        protected override TruckExportFilingController TestInitialize()
        {
            _procedureServiceMock = new Mock<ITruckExportFilingService>();
            _filingHeadersRepositoryMock = new Mock<ITruckExportFilingHeadersRepository>();
            _configurationBuilderMock = new Mock<IFilingConfigurationFactory<TruckExportDefValuesManualReadModel>>();
            _autoRefileServiceMock = new Mock<ITruckExportAutoRefileService>();
            _truckExportRepository = new Mock<ITruckExportRepository>();

            // CreateSingleFilingFilingHeaders should always return result
            _procedureServiceMock.Setup(x => x.CreateSingleFilingFilingHeaders(It.IsAny<IEnumerable<int>>(), It.IsAny<string>()))
                .Returns(new OperationResultWithValue<int[]> { Value = new[] { 1 } });

            return new TruckExportFilingController(
                _procedureServiceMock.Object
                , _filingHeadersRepositoryMock.Object
                , _configurationBuilderMock.Object
                , _autoRefileServiceMock.Object
                , _truckExportRepository.Object
                );
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/export/truck/filing/start", x => x.Start(null));
            AssertRoute(HttpMethod.Get, "/api/export/truck/filing/validate/1", x => x.ValidateFilingHeaderId(1));
            AssertRoute(HttpMethod.Post, "/api/export/truck/filing/validate", x => x.ValidateFilingHeaderIds(null));
            AssertRoute(HttpMethod.Post, "/api/export/truck/filing/cancel", x => x.CancelFilingProcess(null));
            AssertRoute(HttpMethod.Post, "/api/export/truck/filing/record-ids", x => x.GetInboundRecordIdsByFilingHeaders(null));
            AssertRoute(HttpMethod.Get, "/api/export/truck/filing/configuration/1", x => x.GetAllSections(1));
            AssertRoute(HttpMethod.Post, "/api/export/truck/filing/configuration/1/sectionName/1"
                , x => x.AddConfigurationRecord(1, "sectionName", 1));
            AssertRoute(HttpMethod.Delete, "/api/export/truck/filing/configuration/1/sectionName/1"
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
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new TruckExportFilingHeader { JobStatus = JobStatus.InReview });

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
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new TruckExportFilingHeader { JobStatus = JobStatus.InProgress });

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
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns<TruckExportFilingHeader>(null);

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
                new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
                , new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
                , new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
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
                new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
                , new TruckExportFilingHeader { JobStatus = JobStatus.InProgress }
                , new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
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
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(Enumerable.Empty<TruckExportFilingHeader>());
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
                new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
                , new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
                , new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
            });

            // Act
            System.Web.Http.IHttpActionResult result = Controller.CancelFilingProcess(filingHeaderIds);

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
                new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
                , new TruckExportFilingHeader { JobStatus = JobStatus.InProgress}
                , new TruckExportFilingHeader { JobStatus = JobStatus.InReview }
            });

            // Act
            System.Web.Http.IHttpActionResult result = Controller.CancelFilingProcess(filingHeaderIds);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

        }

        [TestMethod]
        public void CancelFilingProcess_ReturnsBadRequest_ForNonexistentFilingHeaderIds()
        {
            // Assign
            var filingHeaderIds = new[] { 1, 2, 3 };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(Enumerable.Empty<TruckExportFilingHeader>);

            // Act
            System.Web.Http.IHttpActionResult result = Controller.CancelFilingProcess(filingHeaderIds);

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
                new TruckExportFilingHeader { TruckExports = new []
                    {
                        new TruckExportRecord { Id = 1}
                        , new TruckExportRecord { Id = 2}
                    }
                }
            });

            // Act
            IEnumerable<int> result = Controller.GetInboundRecordIdsByFilingHeaders(filingHeaderIds);

            // Assert
            CollectionAssert.AreEqual(new[] { 1, 2 }, result.ToArray());
        }

        [TestMethod]
        public void GetAllSections_CallsCreate_ConfigurationBuilder()
        {
            var filingHeaderId = 1;

            Controller.GetAllSections(filingHeaderId);

            _configurationBuilderMock.Verify(x => x.CreateConfiguration(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void AddConfigurationRecord_CallsGet_FilingHeaderRepository()
        {
            var filingHeaderId = 1;
            var sectionName = "test_section";
            var recordId = 1;

            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new TruckExportFilingHeader { Id = filingHeaderId });

            Controller.AddConfigurationRecord(filingHeaderId, sectionName, recordId);

            _filingHeadersRepositoryMock.Verify(x => x.Get(filingHeaderId), Times.Once);
        }

        //[TestMethod]
        //public void Update_CallsUpdateService_WhenIdAreSpecified()
        //{
        //    // Assign
        //    var ids = new[] { 0, 1, 2 };

        //    // Act
        //    Controller.Update(ids);

        //    //Assert
        //    _procedureServiceMock.Verify(x => x.Refile(ids, Controller.CurrentUser), Times.Once);
        //}

        //[TestMethod]
        //public void Update_CallsUpdateService_WithoutSpecifiedIds()
        //{
        //    // Act
        //    Controller.Update(null);

        //    //Assert
        //    _procedureServiceMock.Verify(x => x.Refile(null, Controller.CurrentUser), Times.Once);
        //}

        //[TestMethod]
        //public void Update_Returns_ListOfTheUpdateFilingHeaderIds()
        //{
        //    // Assign
        //    var ids = new[] { 0, 1, 2 };
        //    _procedureServiceMock.Setup(x => x.Refile(ids, Controller.CurrentUser)).Returns(ids);

        //    // Act
        //    IHttpActionResult result = Controller.Update(ids);

        //    //Assert
        //    Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<int[]>));
        //    CollectionAssert.AreEqual(ids, (result as OkNegotiatedContentResult<int[]>)?.Content);

        //}

        //[TestMethod]
        //public void Update_ReturnsBadRequest_IfThereAreNoUpdatedRecords()
        //{
        //    // Assign
        //    var ids = new[] { 0, 1, 2 };
        //    _updatingServiceMock.Setup(x => x.Update(ids, Controller.CurrentUser)).Returns(Enumerable.Empty<int>());

        //    // Act
        //    IHttpActionResult result = Controller.Update(ids);

        //    //Assert
        //    Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        //    Assert.AreEqual("The system could not find any records to update.", (result as BadRequestErrorMessageResult)?.Message);

        //}
    }
}
