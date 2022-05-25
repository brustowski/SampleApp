using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Pipeline;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Validators.Pipeline
{
    [TestClass]
    public class SelectedInboundRecordValidatorTests
    {
        private PipelineListInboundValidator _validator;
        private Mock<IPipelineInboundReadModelRepository> _railInboundReadModelRepositoryMock;
        private Mock<IPipelineFilingHeaderRepository> _filingHeaderRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _railInboundReadModelRepositoryMock = new Mock<IPipelineInboundReadModelRepository>();
            _filingHeaderRepositoryMock = new Mock<IPipelineFilingHeaderRepository>();
            _validator = new PipelineListInboundValidator(_railInboundReadModelRepositoryMock.Object,
                _filingHeaderRepositoryMock.Object);
        }

        private PipelineInboundReadModel BuildModel(bool canBeSelected = true, Action<PipelineInboundReadModel> action = null)
        {
            var mock = new Mock<PipelineInboundReadModel>();
            mock.Setup(x => x.CanBeSelected()).Returns(canBeSelected);
            PipelineInboundReadModel result = mock.Object;

            action?.Invoke(result);

            return result;
        }

        [TestMethod]
        public void Validate_ReturnsEmptyResult_IfCanBeFiled()
        {
            var ids = new List<int> { 3, 4 };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<PipelineInboundReadModel> { BuildModel() });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(string.IsNullOrEmpty(result.CommonError));
        }

        [TestMethod]
        public void Validate_ReturnsEmptyResult_IfNoRecords()
        {
            var ids = new List<int> { 3, 4 };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<PipelineInboundReadModel>());

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(string.IsNullOrEmpty(result.CommonError));
            Assert.IsFalse(result.RecordErrors.Any());
        }

        [TestMethod]
        public void ValidateOnSave_CallsRepository()
        {
            int filingHeaderId = 34;

            var filingHeader = new PipelineFilingHeader
            {
                Id = 34,
                PipelineInbounds = new List<PipelineInbound>
                {
                    new PipelineInbound { Id = 34 },
                    new PipelineInbound { Id = 56 }
                }
            };
            _filingHeaderRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(filingHeader);

            _validator.ValidateBeforeSave(filingHeaderId);

            _filingHeaderRepositoryMock.Verify(x => x.Get(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void ValidateOnSave_NoErrors_RecordsStatusOpenFilingHeaderOpen()
        {
            int filingHeaderId = 34;

            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusOpenFilingHeaderNotDefined()
        {
            int filingHeaderId = 34;

            _filingHeaderRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns((PipelineFilingHeader)null);

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_NoErrors_RecordsStatusInReviewFilingHeaderInReview()
        {
            int filingHeaderId = 34;

            var filingHeader = new PipelineFilingHeader { MappingStatus = MappingStatus.InReview };

            _filingHeaderRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(filingHeader);

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<int[]>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });

            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId))
                .Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInReviewFilingHeaderNotDefined()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInReviewFilingHeaderDifferent()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = 26 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusOpenFilingHeaderInReview()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = 26 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInProgressFilingHeaderDefined()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InProgress });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInProgressFilingHeaderDifferentOpen()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = 89 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusMappedFilingHeaderDifferentOpen()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = 89 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusMappedFilingHeaderMapped()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Mapped });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_NoErrors_RecordsStatusErrorFilingHeaderError()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusOpenFilingHeaderError()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = 98 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInReviewFilingHeaderError()
        {
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusErrorFilingHeaderInReview()
        {
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new PipelineFilingHeader { MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateBeforeSave(1);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_CallsRepository_IfRecordsListNotEmpty()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });

            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader());

            _validator.ValidateRecordsForFiling(filingHeaderId);

            _railInboundReadModelRepositoryMock.Verify(x => x.GetList(It.IsAny<IEnumerable<int>>()), Times.Once);
            _filingHeaderRepositoryMock.Verify(x => x.Get(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_NoError_RecordsStatusOpenFilingHeaderOpen()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void ValidateRecordsForFiling_NoError_RecordsStatusInReviewFilingHeaderInReview()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void ValidateRecordsForFiling_NoError_RecordsStatusErrorFilingHeaderError()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusOpenFilingHeaderNotDefined()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusInProgressFilingHeaderInProgress()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InProgress });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusMappedFilingHeaderMapped()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Mapped });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusInReviewFilingHeaderOpen()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = 98 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusOpenFilingHeaderInReview()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = 98 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as record(s) used in Filing has status which differs from \"In Review\" or does not belong to current entity already", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusErrorFilingHeaderInReview()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = 98 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as record(s) used in Filing has status which differs from \"In Review\" or does not belong to current entity already", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusErrorFilingHeaderDifferent()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = 98 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusOpenFilingHeaderDifferent()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = 98 };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new PipelineFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusInReviewFilingHeaderNotDefined()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview };
            var inboundReadModel2 = new PipelineInboundReadModel { MappingStatus = MappingStatus.InReview };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<PipelineInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }
    }
}
