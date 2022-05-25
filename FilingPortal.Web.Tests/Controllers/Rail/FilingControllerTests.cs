using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Rail;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Web.Common;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.Models.InboundRecordModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Rail
{
    [TestClass]
    public class FilingControllerTests : ControllerBaseTests<FilingController>
    {
        private Mock<IFileProcedureService> _fileProcedureServiceMock;
        private Mock<IListInboundValidator<RailInboundReadModel>> _inboundRecordValidatorMock;
        private Mock<IFilingHeaderDocumentUpdateService<RailDocumentDto>> _documentFileProcedureServiceMock;
        private Mock<IDefValuesManualValidator<RailDefValuesManualReadModel>> _validator;

        protected override FilingController CreateControllerTestInitialize()
        {
            _fileProcedureServiceMock = new Mock<IFileProcedureService>();
            _inboundRecordValidatorMock = new Mock<IListInboundValidator<RailInboundReadModel>>();
            _documentFileProcedureServiceMock = new Mock<IFilingHeaderDocumentUpdateService<RailDocumentDto>>();
            _validator = new Mock<IDefValuesManualValidator<RailDefValuesManualReadModel>>();

            SetupValidator(new[] { 45 }, new int[0]);

            return new FilingController(
                _fileProcedureServiceMock.Object,
                _inboundRecordValidatorMock.Object,
                _documentFileProcedureServiceMock.Object,
                _validator.Object
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

            _validator.Setup(x => x.ValidateUserModels(It.IsAny<IEnumerable<InboundRecordFilingParameters>>())).Returns(validationDictionary);
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
                    new InboundRecordParameterModel {Id = 23, Value = "sdfsd"},
                    new InboundRecordParameterModel {Id = 54, Value = "jkh"}
                }
            };

            Controller.File(new[] { model });

            _fileProcedureServiceMock.Verify(x => x.UpdateFilingParameters(
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
                    new InboundRecordParameterModel {Id = 23, Value = "sdfsd"},
                    new InboundRecordParameterModel {Id = 54, Value = "jkh"}
                }
            };

            Controller.File(new[] { model });

            _fileProcedureServiceMock.Verify(x => x.File(new[] { model.FilingHeaderId }), Times.Once);
        }

        [TestMethod]
        public void File_WhenError_ReturnsInvalidModel()
        {
            _fileProcedureServiceMock.Setup(x => x.File(It.IsAny<int[]>())).Throws<Exception>();

            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 45,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 23, Value = "sdfsd"},
                    new InboundRecordParameterModel {Id = 54, Value = "jkh"}
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

            _fileProcedureServiceMock.Setup(x => x.File(It.IsAny<int[]>())).Throws<Exception>();

            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 45,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 23, Value = "sdfsd"},
                    new InboundRecordParameterModel {Id = 54, Value = "jkh"}
                }
            };

            Controller.File(new[] { model });

            var result = (JsonResult)Controller.File(new[] { model });
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
                    new InboundRecordParameterModel {Id = 23, Value = "sdfsd"},
                    new InboundRecordParameterModel {Id = 54, Value = "jkh"}
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
                    new InboundRecordParameterModel {Id = 23, Value = "sdfsd"},
                    new InboundRecordParameterModel {Id = 54, Value = "jkh"}
                }
            };

            Controller.File(new[] { model });

            _documentFileProcedureServiceMock.Verify((IFilingHeaderDocumentUpdateService<RailDocumentDto> x) => x.UpdateForFilingHeader(45, It.Is((List<RailDocumentDto> c) => c.All(d => d.FileDesc == "Desc"))), Times.Once);
        }

        [TestMethod]
        public void File_WhenSelectedRecordsAreNotValid_ReturnsInvalidresult()
        {
            var filingHeaderId = 3;
            var ids = new List<int>();
            _inboundRecordValidatorMock.Setup(x => x.ValidateRecordsForFiling(filingHeaderId))
                .Returns("errorMessage");

            var model = new InboundRecordFileModel
            {
                FilingHeaderId = filingHeaderId,
            };

            var result = (JsonResult)Controller.File(new[] { model });
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

            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 45,
            };

            Controller.File(new[] { model });

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

            _fileProcedureServiceMock.Verify(x => x.UpdateFilingParameters(
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

            _fileProcedureServiceMock.Verify(x => x.SetInReview(new[] { model.FilingHeaderId }), Times.Once);
        }

        [TestMethod]
        public void Save_WhenError_ReturnsBadRequest()
        {
            _fileProcedureServiceMock.Setup(x => x.SetInReview(It.IsAny<int[]>())).Throws<Exception>();

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

            AssertThatReturnBadRequest();
        }

        [TestMethod]
        public void Save_WhenModelStateIsNotValid_ReturnsOk()
        {
            Controller.ModelState.AddModelError("Error", "Error");

            var model = new InboundRecordFileModel
            {
                FilingHeaderId = 89,
                Parameters = new List<InboundRecordParameterModel>
                {
                    new InboundRecordParameterModel {Id = 8998, Value = "param 1"},
                    new InboundRecordParameterModel {Id = 3268, Value = "param 2"}
                }
            };

            var result = Controller.Save(new[] { model }) as JsonResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Save_WhenSucceeded_ReturnsOk()
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

            var result = Controller.Save(new[] { model }) as JsonResult;

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

            _documentFileProcedureServiceMock.Verify((IFilingHeaderDocumentUpdateService<RailDocumentDto> x) => x.UpdateForFilingHeader(89,
                It.Is((List<RailDocumentDto> c) => c.All(d => d.FileDesc == "doc description"))), Times.Once);
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

        [TestMethod]
        public void File_With_ValidationErrors_ReturnsInvalidResult()
        {
            var filingHeaderId = 3;
            SetupValidator(new int[0], new[] { 3 });

            var model = new InboundRecordFileModel
            {
                FilingHeaderId = filingHeaderId,
            };

            var result = (JsonResult)Controller.File(new[] { model });
            var data = (FilingResultBuilder)result.Data;

            Assert.IsFalse(data.IsValid);
        }

        [TestMethod]
        public void File_Without_ValidationErrors_Returns_GoodResult()
        {
            var filingHeaderId = 3;
            SetupValidator(new int[] { 3 }, new int[0]);

            var model = new InboundRecordFileModel
            {
                FilingHeaderId = filingHeaderId,
            };

            ActionResult result = Controller.File(new[] { model });

            AssertThatReturnGoodRequest();
        }

    }
}