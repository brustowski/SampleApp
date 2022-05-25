using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Rail;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Validators
{
    [TestClass]
    public class SelectedInboundRecordValidatorTests
    {
        private RailListInboundValidator _validator;
        private Mock<IRailInboundReadModelRepository> _railInboundReadModelRepositoryMock;
        private Mock<IRailFilingHeadersRepository> _filingHeaderRepositoryMock;
        private Mock<ISpecificationBuilder> _specificationBuilderMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _railInboundReadModelRepositoryMock = new Mock<IRailInboundReadModelRepository>();
            _filingHeaderRepositoryMock = new Mock<IRailFilingHeadersRepository>();
            _specificationBuilderMock = new Mock<ISpecificationBuilder>();
            _validator = new RailListInboundValidator(_railInboundReadModelRepositoryMock.Object,
                _filingHeaderRepositoryMock.Object, _specificationBuilderMock.Object);
        }

        private RailInboundReadModel BuildModel(bool canBeSelected = true, Action<RailInboundReadModel> action = null)
        {
            var mock = new Mock<RailInboundReadModel>();
            mock.Setup(x => x.CanBeSelected()).Returns(canBeSelected);
            RailInboundReadModel result = mock.Object;

            action?.Invoke(result);

            return result;
        }

        [TestMethod]
        public void Validate_ReturnsEmptyResult_IfCanBeFiled()
        {
            var ids = new List<int> { 3, 4 };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel> { BuildModel() });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(string.IsNullOrEmpty(result.CommonError));
        }

        [TestMethod]
        public void Validate_ReturnsEmptyResult_IfNoRecords()
        {
            var ids = new List<int> { 3, 4 };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>());

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(string.IsNullOrEmpty(result.CommonError));
            Assert.IsFalse(result.RecordErrors.Any());
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfCanNotBeFiled()
        {
            var ids = new List<int> { 3 };


            var canNotBeFiledInboindRecord = new RailInboundReadModel
            {
                IsDeleted = true
            };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel> { canNotBeFiledInboindRecord });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.AreEqual("At least one of selected records has incorrect status", result.CommonError);
            Assert.AreEqual(1, result.RecordErrors.SelectMany(x => x.Errors).Count());
            Assert.AreEqual("At least one of selected records has incorrect status", result.RecordErrors.First().Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfHaveNotTheSameImports()
        {
            var ids = new List<int> { 3, 7 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Id = 3; x.Importer = "A1"; x.Supplier = "r"; x.TrainNumber = "1"; x.HTS = "t"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Id = 7; x.Importer = "A2"; x.Supplier = "r"; x.TrainNumber = "1"; x.HTS = "t"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<List<int>>())).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS", result.CommonError);
            Assert.AreEqual(2, result.RecordErrors.SelectMany(x => x.Errors).Count());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.First().Errors.FirstOrDefault());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.ElementAt(1).Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfHaveNotTheSameSupplier()
        {
            var ids = new List<int> { 3, 7 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Id = 3; x.Importer = "a"; x.Supplier = "r1"; x.TrainNumber = "1"; x.HTS = "t"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Id = 7; x.Importer = "a"; x.Supplier = "r2"; x.TrainNumber = "1"; x.HTS = "t"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS", result.CommonError);
            Assert.AreEqual(2, result.RecordErrors.SelectMany(x => x.Errors).Count());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.First().Errors.FirstOrDefault());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.ElementAt(1).Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfHaveNotTheSameTrainNumber()
        {
            var ids = new List<int> { 3 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Importer = "a"; x.Supplier = "r1"; x.TrainNumber = "1"; x.HTS = "t"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Importer = "a"; x.Supplier = "r2"; x.TrainNumber = "2"; x.HTS = "t"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS", result.CommonError);
            Assert.AreEqual(2, result.RecordErrors.SelectMany(x => x.Errors).Count());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.First().Errors.FirstOrDefault());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.ElementAt(1).Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfHaveNotTheSameHTS()
        {
            var ids = new List<int> { 3 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Importer = "a"; x.Supplier = "r"; x.TrainNumber = "1"; x.HTS = "t1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Importer = "a"; x.Supplier = "r"; x.TrainNumber = "1"; x.HTS = "t2"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS", result.CommonError);
            Assert.AreEqual(2, result.RecordErrors.SelectMany(x => x.Errors).Count());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.First().Errors.FirstOrDefault());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.ElementAt(1).Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfHaveNotTheSameBdParsedPortOfUnlading()
        {
            var ids = new List<int> { 3 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.PortCode = "p1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.PortCode = "p2"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS", result.CommonError);
            Assert.AreEqual(2, result.RecordErrors.SelectMany(x => x.Errors).Count());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.First().Errors.FirstOrDefault());
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier, Train #, Port and HTS",
                result.RecordErrors.ElementAt(1).Errors.FirstOrDefault());
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfHaveNotTheSameBdParsedImporter()
        {
            var ids = new List<int> { 3 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.BdParsedImporterConsignee = "i1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.BdParsedImporterConsignee = "i2"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier and Description1 attributes in manifests", result.CommonError);
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfHaveNotTheSameBdParsedSupplier()
        {
            var ids = new List<int> { 3 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.BdParsedSupplier = "s1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.BdParsedSupplier = "s2"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier and Description1 attributes in manifests", result.CommonError);
        }

        [TestMethod]
        public void Validate_ReturnsCommonErrorMessage_IfHaveNotTheSameBdParsedDescription1()
        {
            var ids = new List<int> { 3 };


            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.BdParsedDescription1 = "d1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.BdParsedDescription1 = "d2"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Selected rows shall have the same Importer, Supplier and Description1 attributes in manifests", result.CommonError);
        }

        [TestMethod]
        public void Validate_ReturnsNoParticularErrorMessages_IfHaveSameValues()
        {
            var ids = new List<int> { 3, 7 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Id = 3; x.Importer = "i"; x.Supplier = "s"; x.BdParsedDescription1 = "d1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Id = 7; x.Importer = "i"; x.Supplier = "s"; x.BdParsedDescription1 = "d1"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(3, result.RecordErrors.ElementAt(0).Id);
            Assert.AreEqual(0, result.RecordErrors.ElementAt(0).Errors.Count);
            Assert.AreEqual(7, result.RecordErrors.ElementAt(1).Id);
            Assert.AreEqual(0, result.RecordErrors.ElementAt(1).Errors.Count);
        }

        [TestMethod]
        public void Validate_ReturnsParticularErrorMessages_IfHaveNotTheSameBdParsedDescription1()
        {
            var ids = new List<int> { 3, 7 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Id = 3; x.BdParsedDescription1 = "d11"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Id = 7; x.BdParsedDescription1 = "d12"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(3, result.RecordErrors.ElementAt(0).Id);
            Assert.AreEqual(1, result.RecordErrors.ElementAt(0).Errors.Count);
            Assert.AreEqual("Description1 value (\"d11\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(0).Errors.ElementAt(0));
            Assert.AreEqual(7, result.RecordErrors.ElementAt(1).Id);
            Assert.AreEqual(1, result.RecordErrors.ElementAt(1).Errors.Count);
            Assert.AreEqual("Description1 value (\"d12\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(1).Errors.ElementAt(0));
        }

        [TestMethod]
        public void Validate_ReturnsParticularErrorMessages_IfHaveNotTheSameBdParsedImporter()
        {
            var ids = new List<int> { 3, 7 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Id = 3; x.BdParsedImporterConsignee = "i1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Id = 7; x.BdParsedImporterConsignee = "i2"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(3, result.RecordErrors.ElementAt(0).Id);
            Assert.AreEqual(1, result.RecordErrors.ElementAt(0).Errors.Count);
            Assert.AreEqual("Importer value (\"i1\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(0).Errors.ElementAt(0));
            Assert.AreEqual(7, result.RecordErrors.ElementAt(1).Id);
            Assert.AreEqual(1, result.RecordErrors.ElementAt(1).Errors.Count);
            Assert.AreEqual("Importer value (\"i2\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(1).Errors.ElementAt(0));
        }

        [TestMethod]
        public void Validate_ReturnsParticularErrorMessages_IfHaveNotTheSameBdParsedSupplier()
        {
            var ids = new List<int> { 3, 7 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Id = 3; x.BdParsedSupplier = "s1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Id = 7; x.BdParsedSupplier = "s2"; });


            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(3, result.RecordErrors.ElementAt(0).Id);
            Assert.AreEqual(1, result.RecordErrors.ElementAt(0).Errors.Count);
            Assert.AreEqual("Supplier value (\"s1\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(0).Errors.ElementAt(0));
            Assert.AreEqual(7, result.RecordErrors.ElementAt(1).Id);
            Assert.AreEqual(1, result.RecordErrors.ElementAt(1).Errors.Count);
            Assert.AreEqual("Supplier value (\"s2\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(1).Errors.ElementAt(0));
        }

        [TestMethod]
        public void Validate_ReturnsParticularErrorMessages_IfHaveNotTheSameBdParsedSupplierAndImporter()
        {
            var ids = new List<int> { 3, 7 };

            RailInboundReadModel canNotBeFiledInboindRecord1 = BuildModel(action: x => { x.Id = 3; x.BdParsedSupplier = "s1"; x.BdParsedImporterConsignee = "i1"; });
            RailInboundReadModel canNotBeFiledInboindRecord2 = BuildModel(action: x => { x.Id = 7; x.BdParsedSupplier = "s2"; x.BdParsedImporterConsignee = "i2"; });

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
            {
                canNotBeFiledInboindRecord1,
                canNotBeFiledInboindRecord2
            });

            InboundRecordValidationResult result = _validator.Validate(ids);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(3, result.RecordErrors.ElementAt(0).Id);
            Assert.AreEqual(2, result.RecordErrors.ElementAt(0).Errors.Count);
            Assert.AreEqual("Importer value (\"i1\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(0).Errors.ElementAt(0));
            Assert.AreEqual("Supplier value (\"s1\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(0).Errors.ElementAt(1));
            Assert.AreEqual(7, result.RecordErrors.ElementAt(1).Id);
            Assert.AreEqual(2, result.RecordErrors.ElementAt(1).Errors.Count);
            Assert.AreEqual("Importer value (\"i2\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(1).Errors.ElementAt(0));
            Assert.AreEqual("Supplier value (\"s2\") from manifest shall be the same for all selected rows",
                result.RecordErrors.ElementAt(1).Errors.ElementAt(1));
        }

        [TestMethod]
        public void ValidateOnSave_CallsRepository()
        {
            int filingHeaderId = 34;

            var filingHeader = new RailFilingHeader
            {
                Id = 34,
                RailBdParseds = new List<RailBdParsed>
                {
                    new RailBdParsed { Id = 34 },
                    new RailBdParsed { Id = 56 }
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

            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusOpenFilingHeaderNotDefined()
        {
            int filingHeaderId = 34;

            _filingHeaderRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns((RailFilingHeader)null);

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_NoErrors_RecordsStatusInReviewFilingHeaderInReview()
        {
            int filingHeaderId = 34;

            var filingHeader = new RailFilingHeader { MappingStatus = MappingStatus.InReview };

            _filingHeaderRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(filingHeader);

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<int[]>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });

            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId))
                .Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInReviewFilingHeaderNotDefined()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(ids)).Returns(new List<RailInboundReadModel>
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
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = 26 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusOpenFilingHeaderInReview()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = 26 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInProgressFilingHeaderDefined()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InProgress });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInProgressFilingHeaderDifferentOpen()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = 89 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusMappedFilingHeaderDifferentOpen()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = 89 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusMappedFilingHeaderMapped()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Mapped });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_NoErrors_RecordsStatusErrorFilingHeaderError()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusOpenFilingHeaderError()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = 98 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusInReviewFilingHeaderError()
        {
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Error };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateBeforeSave(filingHeaderId);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateOnSave_Error_RecordsStatusErrorFilingHeaderInReview()
        {
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Error };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Error };
            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new RailFilingHeader { MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateBeforeSave(1);

            Assert.AreEqual("System can not perform Save operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_CallsRepository_IfRecordsListNotEmpty()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });

            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader());

            _validator.ValidateRecordsForFiling(filingHeaderId);

            _railInboundReadModelRepositoryMock.Verify(x => x.GetList(It.IsAny<IEnumerable<int>>()), Times.Once);
            _filingHeaderRepositoryMock.Verify(x => x.Get(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_NoError_RecordsStatusOpenFilingHeaderOpen()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void ValidateRecordsForFiling_NoError_RecordsStatusInReviewFilingHeaderInReview()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void ValidateRecordsForFiling_NoError_RecordsStatusErrorFilingHeaderError()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusOpenFilingHeaderNotDefined()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Open };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Open };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
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

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InProgress, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InProgress });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusMappedFilingHeaderMapped()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = filingHeaderId };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Mapped, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Mapped });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusInReviewFilingHeaderOpen()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = 98 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusOpenFilingHeaderInReview()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = 98 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as record(s) used in Filing has status which differs from \"In Review\" or does not belong to current entity already", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusErrorFilingHeaderInReview()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = 98 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.InReview });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as record(s) used in Filing has status which differs from \"In Review\" or does not belong to current entity already", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusErrorFilingHeaderDifferent()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = 98 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Error, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Error });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusOpenFilingHeaderDifferent()
        {
            var ids = new List<int> { 1, 2, 3 };
            int filingHeaderId = 5;

            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = 98 };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.Open, FilingHeaderId = filingHeaderId };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });
            _filingHeaderRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(new RailFilingHeader { Id = filingHeaderId, MappingStatus = MappingStatus.Open });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }

        [TestMethod]
        public void ValidateRecordsForFiling_Error_RecordsStatusInReviewFilingHeaderNotDefined()
        {
            var ids = new List<int> { 34, 56 };
            int filingHeaderId = 34;
            var inboundReadModel1 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview };
            var inboundReadModel2 = new RailInboundReadModel { MappingStatus = MappingStatus.InReview };

            _railInboundReadModelRepositoryMock.Setup(x => x.GetList(It.IsAny<IEnumerable<int>>())).Returns(new List<RailInboundReadModel>
            {
                inboundReadModel1,
                inboundReadModel2
            });

            string result = _validator.ValidateRecordsForFiling(filingHeaderId);

            Assert.AreEqual("System can not perform File operation as at least one record used in Filing has status which differs from \"Open\" or \"Error\"", result);
        }
    }
}
