using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Pipeline;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.Web.Controllers.Pipeline;
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

namespace FilingPortal.Web.Tests.Controllers.Pipeline
{
    [TestClass]
    public class PipelineInboundFilingControllerTests : ApiControllerFunctionTestsBase<PipelineInboundFilingController>
    {
        private Mock<IPipelineFilingService> _procedureServiceMock;
        private Mock<IPipelineFilingHeaderRepository> _filingHeadersRepositoryMock;
        private Mock<IFilingConfigurationFactory<PipelineDefValueManualReadModel>> _configurationBuilderMock;
        private Mock<IFilingParametersService<PipelineDefValueManual, PipelineDefValueManualReadModel>> _paramsServiceMock;

        protected override PipelineInboundFilingController TestInitialize()
        {
            _procedureServiceMock = new Mock<IPipelineFilingService>();
            _filingHeadersRepositoryMock = new Mock<IPipelineFilingHeaderRepository>();
            _configurationBuilderMock = new Mock<IFilingConfigurationFactory<PipelineDefValueManualReadModel>>();
            _paramsServiceMock = new Mock<IFilingParametersService<PipelineDefValueManual, PipelineDefValueManualReadModel>>();

            // CreateInitialFilingHeader should always return result
            _procedureServiceMock.Setup(x => x.CreateSingleFilingFilingHeaders(It.IsAny<IEnumerable<int>>(), It.IsAny<string>()))
                .Returns(new OperationResultWithValue<int[]> { Value = new[] { 1 } });

            return new PipelineInboundFilingController(
                _procedureServiceMock.Object
                , _filingHeadersRepositoryMock.Object
                , _configurationBuilderMock.Object
                , _paramsServiceMock.Object
                );
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/filing/start", x => x.Start(null));
            AssertRoute(HttpMethod.Get, "/api/inbound/pipeline/filing/validate/1", x => x.ValidateFilingHeaderId(1));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/filing/validate", x => x.ValidateFilingHeaderIds(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/filing/cancel", x => x.CancelFilingProcess(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/filing/record-ids", x => x.GetInboundRecordIdsByFilingHeaders(null));
            AssertRoute(HttpMethod.Get, "/api/inbound/pipeline/filing/configuration/1", x => x.GetAllSections(1));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/filing/configuration/1/sectionName/1"
                , x => x.AddConfigurationRecord(1, "sectionName", 1));
            AssertRoute(HttpMethod.Delete, "/api/inbound/pipeline/filing/configuration/1/sectionName/1"
                , x => x.DeleteConfigurationRecord(1, "sectionName", 1));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/filing/process_changes", x => x.ProcessChanges(null));
        }

        [TestMethod]
        public void PermissionAssertion()
        {
            AssertPermissions(Permissions.PipelineFileInboundRecord, x => x.Start(null));
            AssertPermissions(Permissions.PipelineFileInboundRecord, x => x.ValidateFilingHeaderId(1));
            AssertPermissions(Permissions.PipelineFileInboundRecord, x => x.ValidateFilingHeaderIds(null));
            AssertPermissions(Permissions.PipelineFileInboundRecord, x => x.CancelFilingProcess(null));
            AssertPermissions(Permissions.PipelineViewInboundRecord, x => x.GetInboundRecordIdsByFilingHeaders(null));
            AssertPermissions(Permissions.PipelineViewInboundRecord, x => x.GetAllSections(1));
            AssertPermissions(Permissions.PipelineFileInboundRecord, x => x.ProcessChanges(null));
            AssertPermissions(Permissions.PipelineFileInboundRecord
                , x => x.AddConfigurationRecord(1, "section_name", 1));
            AssertPermissions(Permissions.PipelineFileInboundRecord
                , x => x.DeleteConfigurationRecord(1, "section_name", 1));
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
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { MappingStatus = MappingStatus.InReview });

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
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { MappingStatus = MappingStatus.InProgress });

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
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns<PipelineFilingHeader>(null);

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
                new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
                , new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
                , new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
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
                new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
                , new PipelineFilingHeader { MappingStatus = MappingStatus.InProgress }
                , new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
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
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(Enumerable.Empty<PipelineFilingHeader>());
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
                new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
                , new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
                , new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
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
                new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
                , new PipelineFilingHeader { MappingStatus = MappingStatus.InProgress}
                , new PipelineFilingHeader { MappingStatus = MappingStatus.InReview }
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
            _filingHeadersRepositoryMock.Setup(x => x.GetList(filingHeaderIds)).Returns(Enumerable.Empty<PipelineFilingHeader>);

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
                new PipelineFilingHeader { PipelineInbounds = new []
                    {
                        new PipelineInbound { Id = 1}
                        , new PipelineInbound { Id = 2}
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

            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId });

            Controller.AddConfigurationRecord(filingHeaderId, sectionName, recordId);

            _filingHeadersRepositoryMock.Verify(x => x.Get(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void AddConfigurationRecord_CallsAddSectionRow_FilingHeaderRepository_WhenCalled()
        {
            var filingHeaderId = 1;
            var sectionName = "test_section";
            var recordId = 1;
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, FilingStatus = FilingStatus.Open });

            Controller.AddConfigurationRecord(filingHeaderId, sectionName, recordId);

            _filingHeadersRepositoryMock.Verify(x => x.AddSectionRecord(sectionName, filingHeaderId, recordId, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AddConfigurationRecord_CallsCreate_ConfigurationBuilder()
        {
            var filingHeaderId = 1;
            var sectionName = "test_section";
            var recordId = 1;
            var guid = Guid.NewGuid();
            _filingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, FilingStatus = FilingStatus.Open });
            _filingHeadersRepositoryMock.Setup(x => x.AddSectionRecord(sectionName, filingHeaderId, recordId, It.IsAny<string>())).Returns(guid);

            Controller.AddConfigurationRecord(filingHeaderId, sectionName, recordId);

            _configurationBuilderMock.Verify(x => x.CreateConfigurationForSection(filingHeaderId, sectionName, guid), Times.Once);
        }
    }
}
