using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Pipeline;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.Web.Controllers.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Pipeline
{
    [TestClass]
    public class PipelineFilingControllerTests : ControllerBaseTests<PipelineFilingController>
    {
        private Mock<IPipelineFilingService> _fileProcedureServiceMock;
        private Mock<IPipelineFilingHeaderDocumentUpdateService> _documentFileProcedureServiceMock;
        private Mock<IListInboundValidator<PipelineInboundReadModel>> _inboundRecordValidatorMock;
        private Mock<IDefValuesManualValidator<PipelineDefValueManualReadModel>> _modelValidatorMock;
        private Mock<IFilingParametersService<PipelineDefValueManual, PipelineDefValueManualReadModel>> _filingParamsServiceMock;
        protected override PipelineFilingController CreateControllerTestInitialize()
        {
            _fileProcedureServiceMock = new Mock<IPipelineFilingService>();
            _inboundRecordValidatorMock = new Mock<IListInboundValidator<PipelineInboundReadModel>>();
            _documentFileProcedureServiceMock = new Mock<IPipelineFilingHeaderDocumentUpdateService>();
            _modelValidatorMock = new Mock<IDefValuesManualValidator<PipelineDefValueManualReadModel>>();
            _filingParamsServiceMock = new Mock<IFilingParametersService<PipelineDefValueManual, PipelineDefValueManualReadModel>>();

            SetupValidator(new[] { 45 }, new int[0]);

            return new PipelineFilingController(
                _fileProcedureServiceMock.Object,
                _inboundRecordValidatorMock.Object,
                _documentFileProcedureServiceMock.Object,
                _modelValidatorMock.Object,
                _filingParamsServiceMock.Object
                );
        }

        private void SetupValidator(IEnumerable<int> goodFilingHeaders, IEnumerable<int> badFilingHeaders)
        {
            var goodValidationResult = new Mock<DetailedValidationResult>();
            goodValidationResult.Setup(x => x.IsValid).Returns(true);

            var badValidationResult = new DetailedValidationResult();
            badValidationResult.AddError("Test error");

            Dictionary<InboundRecordFilingParameters, DetailedValidationResult> validationDictionary = goodFilingHeaders
                .Select(filingHeaderId => new InboundRecordFilingParameters { FilingHeaderId = filingHeaderId })
                .ToDictionary(filingParameters => filingParameters, filingParameters => goodValidationResult.Object);

            foreach (var filingHeaderId in badFilingHeaders)
            {
                var filingParameters = new InboundRecordFilingParameters { FilingHeaderId = filingHeaderId };

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

            _filingParamsServiceMock.Verify(x => x.UpdateFilingParameters(
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
                Documents = new List<InboundRecordDocumentEditModel> { new InboundRecordDocumentEditModel { Description = "Desc" } }
            };

            Controller.File(new[] { model });

            _documentFileProcedureServiceMock.Verify(x
                => x.UpdateForFilingHeader(45, It.Is((List<PipelineDocumentDto> c) => c.All(d => d.FileDesc == "Desc")), true), Times.Once);
        }

        [TestMethod]
        public void File_WhenSelectedRecordsAreNotValid_ReturnsInvalidResult()
        {
            var filingHeaderId = 3;

            _inboundRecordValidatorMock.Setup(x => x.ValidateRecordsForFiling(filingHeaderId))
                .Returns("errorMessage");

            var result = (JsonResult)Controller.File(new[] { new InboundRecordFileModel { FilingHeaderId = filingHeaderId } });
            var data = (FilingResultBuilder)result.Data;

            Assert.IsFalse(data.IsValid);
        }

        [TestMethod]
        public void File_WhenSelectedRecordsAreFilled_ValidatesSpecifiedIds()
        {
            var filingHeaderId = 45;

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

            _filingParamsServiceMock.Verify(x => x.UpdateFilingParameters(
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

            _documentFileProcedureServiceMock.Verify(x => x.UpdateForFilingHeader(89,
                It.Is((List<PipelineDocumentDto> c) => c.All(d => d.FileDesc == "doc description")), false), Times.Once);
        }

        [TestMethod]
        public void Save_ForSelectedRecords_CallsValidator()
        {
            var filingHeaderId = 89;

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