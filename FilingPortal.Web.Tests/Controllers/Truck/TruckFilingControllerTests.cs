using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Truck;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Truck;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Web.Common;
using FilingPortal.Web.Controllers.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Models.InboundRecordModels;

namespace FilingPortal.Web.Tests.Controllers.Truck
{
    [TestClass]
    public class TruckFilingControllerTests : ControllerBaseTests<TruckFilingController>
    {
        private Mock<ITruckFilingService> _fileProcedureServiceMock;
        private Mock<IListInboundValidator<TruckInboundReadModel>> _inboundRecordValidatorMock;
        private Mock<IFilingHeaderDocumentUpdateService<TruckDocumentDto>> _documentFileProcedureServiceMock;
        private Mock<IDefValuesManualValidator<TruckDefValueManualReadModel>> _modelValidatorMock;
        private Mock<IFilingParametersService<TruckDefValueManual, TruckDefValueManualReadModel>> _paramsServiceMock;

        protected override TruckFilingController CreateControllerTestInitialize()
        {
            _fileProcedureServiceMock = new Mock<ITruckFilingService>();
            _inboundRecordValidatorMock = new Mock<IListInboundValidator<TruckInboundReadModel>>();
            _documentFileProcedureServiceMock = new Mock<IFilingHeaderDocumentUpdateService<TruckDocumentDto>>();
            _modelValidatorMock = new Mock<IDefValuesManualValidator<TruckDefValueManualReadModel>>();
            _paramsServiceMock = new Mock<IFilingParametersService<TruckDefValueManual, TruckDefValueManualReadModel>>();

            SetupValidator(new[] { 45 }, new int[0]);

            return new TruckFilingController(
                _fileProcedureServiceMock.Object,
                _inboundRecordValidatorMock.Object,
                _documentFileProcedureServiceMock.Object,
                _modelValidatorMock.Object,
                _paramsServiceMock.Object
                );
        }

        private void SetupValidator(int[] goodFilingHeaders, int[] badFilingHeaders)
        {
            var validationDictionary = new Dictionary<InboundRecordFilingParameters, DetailedValidationResult>();

            var goodValidationResult = new Mock<DetailedValidationResult>();
            goodValidationResult.Setup(x => x.IsValid).Returns(true);

            var badValidationResult = new DetailedValidationResult();
            badValidationResult.AddError("Test error");

            foreach (var filingHeaderId in goodFilingHeaders)
            {
                var filingParameters = new InboundRecordFilingParameters() { FilingHeaderId = filingHeaderId };

                validationDictionary.Add(filingParameters, goodValidationResult.Object);
            }

            foreach (var filingHeaderId in badFilingHeaders)
            {
                var filingParameters = new InboundRecordFilingParameters() { FilingHeaderId = filingHeaderId };

                validationDictionary.Add(filingParameters, badValidationResult);
            }

            _modelValidatorMock.Setup(x => x.ValidateUserModels(It.IsAny<IEnumerable<InboundRecordFilingParameters>>())).Returns(validationDictionary);
        }

        protected override string GetUserIdTestInitialize()
        {
            return "1";
        }

        [TestMethod]
        public void File_CallsService_UpdateParams()
        {
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 45,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 23, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 54, Value = "param 2"}
                }
            };

            Controller.File(new[] { model });

            _paramsServiceMock.Verify(x => x.UpdateFilingParameters(
                It.Is<InboundRecordFilingParameters>(p =>
                    p.FilingHeaderId == model.FilingHeaderId &&
                    p.Parameters.Count() == model.Parameters.Count()
                )), Times.Once);
        }

        [TestMethod]
        public void File_CallsService_File()
        {
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 45,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 23, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 54, Value = "param 2"}
                }
            };

            Controller.File(new[] { model });

            _fileProcedureServiceMock.Verify(x => x.File(model.FilingHeaderId), Times.Once);
        }

        [TestMethod]
        public void File_WhenError_ReturnsInvalidResult()
        {
            _fileProcedureServiceMock.Setup(x => x.File(It.IsAny<int>())).Throws<Exception>();

            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 45,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 23, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 54, Value = "param 2"}
                }
            };


            var result = (JsonResult)Controller.File(new[] { model });
            var data = (FilingResultBuilder)result.Data;

            Assert.IsFalse(data.IsValid);
        }

        [TestMethod]
        public void File_WhenModelStateIsNotValid_ReturnsInvalidResult()
        {
            Controller.ModelState.AddModelError("Error", "Error");

            _fileProcedureServiceMock.Setup(x => x.File(It.IsAny<int>())).Throws<Exception>();

            var result = (JsonResult)Controller.File(new[] { new InboundRecordFileModel() });
            var data = (FilingResultBuilder)result.Data;

            Assert.IsFalse(data.IsValid);
        }

        [TestMethod]
        public void File_WhenSucceeded_ReturnsOk()
        {
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 45,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 23, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 54, Value = "param 2"}
                }
            };

            Controller.File(new[] { model });

            var result = Controller.File(new[] { model }) as JsonResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void File_CallsService_AddDocumentsToFilingHeader()
        {
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 45,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 23, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 54, Value = "param 2"}
                },
                Documents = new List<InboundRecordDocumentEditModel>() { new InboundRecordDocumentEditModel() { Description = "Desc" } }
            };

            Controller.File(new[] { model });

            _documentFileProcedureServiceMock.Verify((IFilingHeaderDocumentUpdateService<TruckDocumentDto> x) => x.UpdateForFilingHeader(45, It.Is((List<TruckDocumentDto> c) => c.All(d => d.FileDesc == "Desc")), false), Times.Once);
        }

        [TestMethod]
        public void File_WhenSelectedRecordsAreNotValid_ReturnsInvalidResult()
        {
            var filingHeaderId = 3;
            var ids = new List<int>();
            _inboundRecordValidatorMock.Setup(x => x.ValidateRecordsForFiling(filingHeaderId))
                .Returns("errorMessage");

            var result = (JsonResult)Controller.File(new[] { new InboundRecordFileModel() { FilingHeaderId = filingHeaderId } });
            var data = (FilingResultBuilder)result.Data;

            Assert.IsFalse(data.IsValid);
        }

        [TestMethod]
        public void File_WhenSelectedRecordsAreFilled_ValidatesSpecifiedIds()
        {
            var filingHeaderId = 45;
            var ids = new List<int> { 1, 3, 5 };
            _inboundRecordValidatorMock.Setup(x => x.ValidateRecordsForFiling(filingHeaderId))
                .Returns("");


            Controller.File(new[] { new InboundRecordFileModel { FilingHeaderId = filingHeaderId } });

            _inboundRecordValidatorMock.Verify(x => x.ValidateRecordsForFiling(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void Save_CallsService_UpdateParams()
        {
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 89,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 8998, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 3268, Value = "param 2"}
                }
            };

            Controller.Save(new[] { model });

            _paramsServiceMock.Verify(x => x.UpdateFilingParameters(
                It.Is<InboundRecordFilingParameters>(p =>
                    p.FilingHeaderId == model.FilingHeaderId &&
                    p.Parameters.Count() == model.Parameters.Count()
                )), Times.Once);
        }

        [TestMethod]
        public void Save_CallsService_SetInReview()
        {
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 89,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 8998, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 3268, Value = "param 2"}
                }
            };

            Controller.Save(new[] { model });

            _fileProcedureServiceMock.Verify(x => x.SetInReview(model.FilingHeaderId), Times.Once);
        }

        [TestMethod]
        public void Save_WhenError_ReturnsBadRequest()
        {
            _fileProcedureServiceMock.Setup(x => x.SetInReview(It.IsAny<int>())).Throws<Exception>();

            Controller.Save(new[] { new InboundRecordFileModel() });

            AssertThatReturnBadRequest();
        }

        [TestMethod]
        public void Save_WhenModelStateIsNotValid_ReturnsOk()
        {
            Controller.ModelState.AddModelError("Error", "Error");

            var result = Controller.Save(new[] { new InboundRecordFileModel() }) as JsonResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Save_WhenSucceeded_ReturnsOk()
        {
            var result = Controller.Save(new[] { new InboundRecordFileModel() }) as JsonResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Save_CallsService_AddDocumentsToFilingHeader()
        {
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 89,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 8998, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 3268, Value = "param 2"}
                },
                Documents = new List<InboundRecordDocumentEditModel> { new InboundRecordDocumentEditModel { Description = "doc description" } }
            };

            Controller.Save(new[] { model });

            _documentFileProcedureServiceMock.Verify((IFilingHeaderDocumentUpdateService<TruckDocumentDto> x) => x.UpdateForFilingHeader(89,
                It.Is((List<TruckDocumentDto> c) => c.All(d => d.FileDesc == "doc description")), false), Times.Once);
        }

        [TestMethod]
        public void Save_ForSelectedRecords_CallsValidator()
        {
            var filingHeaderId = 89;
            var ids = new List<int> { 56, 23 };
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = filingHeaderId
            };
            _inboundRecordValidatorMock.Setup(x => x.ValidateBeforeSave(filingHeaderId)).Returns(string.Empty);

            Controller.Save(new[] { model });

            _inboundRecordValidatorMock.Verify(x => x.ValidateBeforeSave(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void Save_WhenSelectedRecordsAreNotValid_ReturnsBadRequest()
        {
            var filingHeaderId = 89;
            var ids = new List<int> { 56, 23 };
            var model = new InboundRecordFileModel
            {
                FilingHeaderId = filingHeaderId
            };
            _inboundRecordValidatorMock.Setup(x => x.ValidateBeforeSave(filingHeaderId))
                .Returns("some error");

            Controller.Save(new[] { model });

            AssertThatReturnBadRequest();
        }

    }
}