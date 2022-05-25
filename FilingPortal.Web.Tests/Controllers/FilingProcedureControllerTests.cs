using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Rail;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Rail;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.Models;
using FilingPortal.Web.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Http.Results;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class FilingProcedureControllerTests : ApiControllerFunctionTestsBase<FilingProcedureController>
    {
        private Mock<IFileProcedureService> _fileProcedureServiceMock;
        private Mock<IRailImportRecordsFilingValidator> _inboundRecordValidatorMock;
        private Mock<IRailFilingHeadersRepository> _filingHeadersRepositoryMock;
        private Mock<IFilingConfigurationFactory<RailDefValuesManualReadModel>> _configurationBuilderMock;
        private Mock<IFilingParametersService<RailDefValuesManual, RailDefValuesManualReadModel>> _paramsServiceMock;

        protected override FilingProcedureController TestInitialize()
        {
            _fileProcedureServiceMock = new Mock<IFileProcedureService>();
            _inboundRecordValidatorMock = new Mock<IRailImportRecordsFilingValidator>();
            _filingHeadersRepositoryMock = new Mock<IRailFilingHeadersRepository>();
            _configurationBuilderMock = new Mock<IFilingConfigurationFactory<RailDefValuesManualReadModel>>();
            _paramsServiceMock = new Mock<IFilingParametersService<RailDefValuesManual, RailDefValuesManualReadModel>>();

            return new FilingProcedureController(
                _inboundRecordValidatorMock.Object,
                _fileProcedureServiceMock.Object,
                _filingHeadersRepositoryMock.Object,
                _configurationBuilderMock.Object,
                _paramsServiceMock.Object);
        }

        [TestMethod]
        public void Start_Not_CallsService_IfNoRecords()
        {
            var ids = new int[0];
            var result = new OperationResultWithValue<int[]>
            {
                Value = new[] { 3, 4 }
            };

            _fileProcedureServiceMock.Setup(x => x.CreateSingleFilingFilingHeaders(ids, null)).Returns(result);

            Controller.Start(ids);

            _fileProcedureServiceMock.Verify(x => x.CreateSingleFilingFilingHeaders(ids, null), Times.Never);
        }

        [TestMethod]
        public void Start_CallsService_IfAllRecordsCanBeFiled()
        {
            int[] ids = { 1, 2, 3 };
            var result = new OperationResultWithValue<int[]>
            {
                Value = new[] { 3, 4 }
            };

            _fileProcedureServiceMock.Setup(x => x.CreateSingleFilingFilingHeaders(ids, It.IsAny<string>())).Returns(result);
            _filingHeadersRepositoryMock.Setup(x => x.FindByInboundRecordIds(ids))
                .Returns(new List<RailFilingHeader>());

            Controller.Start(ids);

            _fileProcedureServiceMock.Verify(x => x.CreateSingleFilingFilingHeaders(ids, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Start_CallsService_IfSomeRecordsCanBeFiled()
        {
            int[] ids = { 1, 2, 3 };
            var result = new OperationResultWithValue<int[]>
            {
                Value = new[] { 3, 4 }
            };

            _fileProcedureServiceMock.Setup(x => x.CreateSingleFilingFilingHeaders(It.IsAny<IEnumerable<int>>(), It.IsAny<string>())).Returns(result);

            var bdParsedRecordMock = new Mock<RailBdParsed>();
            bdParsedRecordMock.Object.Id = 3;

            var filingHeaderMock = new Mock<RailFilingHeader>();
            filingHeaderMock.Setup(x => x.CanBeEdited).Returns(true);
            filingHeaderMock.Setup(x => x.RailBdParseds).Returns(new List<RailBdParsed> { bdParsedRecordMock.Object });

            _filingHeadersRepositoryMock.Setup(x => x.FindByInboundRecordIds(ids))
                .Returns(new List<RailFilingHeader> { filingHeaderMock.Object });

            Controller.Start(ids);

            _fileProcedureServiceMock.Verify(x => x.CreateSingleFilingFilingHeaders(new[] { 1, 2 }, It.IsAny<string>()), Times.Once);
        }
        [TestMethod]
        public void Start_Returns_FilingHeaderIds_Both_For_New_And_Filed()
        {
            int[] ids = { 1, 2, 3 };
            var result = new OperationResultWithValue<int[]>
            {
                Value = new[] { 3, 4 }
            };
            // New records will have filing header ids equals to 3 and 4
            _fileProcedureServiceMock.Setup(x => x.CreateSingleFilingFilingHeaders(It.IsAny<IEnumerable<int>>(), It.IsAny<string>())).Returns(result);

            var bdParsedRecordMock = new Mock<RailBdParsed>();
            bdParsedRecordMock.Object.Id = 3;

            var filingHeaderMock = new Mock<RailFilingHeader>();
            filingHeaderMock.Object.Id = 45;
            filingHeaderMock.Setup(x => x.CanBeEdited).Returns(true);
            filingHeaderMock.Setup(x => x.RailBdParseds).Returns(new List<RailBdParsed> { bdParsedRecordMock.Object });

            // Existing record (for id = 3) will return filing header with Id = 45
            _filingHeadersRepositoryMock.Setup(x => x.FindByInboundRecordIds(ids))
                .Returns(new List<RailFilingHeader> { filingHeaderMock.Object });

            System.Web.Http.IHttpActionResult operationResult = Controller.Start(ids);

            var typedResult = operationResult as OkNegotiatedContentResult<int[]>;

            Assert.IsNotNull(typedResult);
            CollectionAssert.AreEqual(typedResult.Content, new[] { 45, 3, 4 });
        }

        [TestMethod]
        public void StartFiltered_CallsService_IfRecordsCanBeFiled()
        {
            var filtersSet = new FilterSetWithExludedRecordsId();
            var result = new OperationResultWithValue<int[]>
            {
                Value = new int[] { 3, 4 }
            };

            _fileProcedureServiceMock.Setup(x => x.CreateSingleFilingFilingHeaders(filtersSet, null)).Returns(result);

            Controller.StartFiltered(filtersSet);

            _fileProcedureServiceMock.Verify(x => x.CreateSingleFilingFilingHeaders(filtersSet, null));
        }


        [TestMethod]
        public void StartUnitTrain_ReturnBadRequest_IfRecordsCanNotBeFiled()
        {
            var ids = new int[0];

            _inboundRecordValidatorMock.Setup(x => x.Validate(ids)).Returns(new InboundRecordValidationResult { CommonError = "error" });

            System.Web.Http.IHttpActionResult result = Controller.StartUnitTrain(ids);

            Assert.IsTrue(result is BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void StartUnitTrain_CallsService_IfRecordsCanBeFiled()
        {
            var ids = new int[0];

            _inboundRecordValidatorMock.Setup(x => x.Validate(ids)).Returns(new InboundRecordValidationResult { CommonError = "" });

            var result = new OperationResultWithValue<int>
            {
                Value = 3
            };

            _fileProcedureServiceMock.Setup(x => x.CreateUnitTrainFilingHeader(ids, It.IsAny<string>())).Returns(result);

            Controller.StartUnitTrain(ids);

            _fileProcedureServiceMock.Verify(x => x.CreateUnitTrainFilingHeader(ids, It.IsAny<string>()));
        }

        [TestMethod]
        public void StartUnitTrainFiltered_ReturnBadRequest_IfRecordsCanNotBeFiled()
        {
            var filtersSet = new FilterSetWithExludedRecordsId();

            _inboundRecordValidatorMock.Setup(x => x.Validate(filtersSet)).Returns(new InboundRecordValidationResult { CommonError = "error" });

            System.Web.Http.IHttpActionResult result = Controller.StartUnitTrainFiltered(filtersSet);

            Assert.IsTrue(result is BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void StartUnitTrainFiltered_CallsService_IfRecordsCanBeFiled()
        {
            var filtersSet = new FilterSetWithExludedRecordsId();
            var result = new OperationResultWithValue<int>
            {
                Value = 3
            };

            _inboundRecordValidatorMock.Setup(x => x.Validate(filtersSet)).Returns(new InboundRecordValidationResult { CommonError = "" });

            _fileProcedureServiceMock.Setup(x => x.CreateUnitTrainFilingHeader(filtersSet, null)).Returns(result);

            Controller.StartUnitTrainFiltered(filtersSet);

            _fileProcedureServiceMock.Verify(x => x.CreateUnitTrainFilingHeader(filtersSet, null));
        }

        [TestMethod]
        public void ValidateFilingHeaderId_CallsRepository()
        {
            var id = 2;

            _filingHeadersRepositoryMock.Setup(x => x.Get(id))
                .Returns(new RailFilingHeader { MappingStatus = MappingStatus.InReview });

            var result = Controller.ValidateFilingHeaderId(id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateFilingHeaderId_IfNotExist()
        {
            var id = 2;

            _filingHeadersRepositoryMock.Setup(x => x.Get(id)).Returns((RailFilingHeader)null);

            var result = Controller.ValidateFilingHeaderId(id);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CancelFilingProcess_ReturnsBadRequest_IfFilingHeaderIsNotExist()
        {
            var id = 2;
            _filingHeadersRepositoryMock.Setup(x => x.Get(id)).Returns((RailFilingHeader)null);

            System.Web.Http.IHttpActionResult result = Controller.CancelFilingProcess(new[] { id });

            Assert.IsTrue(result is BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void CancelFilingProcess_ReturnsBadRequest_IfFilingHeaderIsNotAbleToUndo()
        {
            var id = 2;
            var railFilingHeader = new RailFilingHeader()
            {
                Id = id,
                MappingStatus = MappingStatus.Open,
                RailBdParseds = new List<RailBdParsed>() { new RailBdParsed() { Id = 4 } }
            };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(new[] { id })).Returns(new[] { railFilingHeader });

            System.Web.Http.IHttpActionResult result = Controller.CancelFilingProcess(new[] { id });

            Assert.IsTrue(result is BadRequestErrorMessageResult);
        }

        [TestMethod]
        public void CancelFilingProcess_CallsFilingProcedure_IfFilingHeaderIsValid()
        {
            var id = 2;
            var railFilingHeader = new RailFilingHeader()
            {
                Id = id,
                MappingStatus = MappingStatus.InReview,
                RailBdParseds = new List<RailBdParsed>() { new RailBdParsed() { Id = 4 } }
            };
            _filingHeadersRepositoryMock.Setup(x => x.GetList(new[] { id })).Returns(new[] { railFilingHeader });

            System.Web.Http.IHttpActionResult result = Controller.CancelFilingProcess(new[] { id });

            _fileProcedureServiceMock.Verify(x => x.CancelFilingProcess(new[] { railFilingHeader.Id }));
        }
    }
}