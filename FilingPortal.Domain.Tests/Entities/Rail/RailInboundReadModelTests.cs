using System;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Entities.Rail
{
    [TestClass]
    public class RailInboundReadModelTests
    {
        private RailInboundReadModel CreateValidModel(Action<RailInboundReadModel> action = null)
        {
            var model = new RailInboundReadModel
            {
                Importer = "Importer",
                BdParsedDescription1 = "BdParsedDescription1",
                BdParsedImporterConsignee = "BdParsedImporter",
                BdParsedSupplier = "BdParsedSupplier",
                BOLNumber = "BOLNumber",
                ContainerNumber = "ContainerNumber",
                CreatedDate = DateTime.Now,
                Description = "Description",
                HTS = "HTS",
                Id = 1,
                IsDeleted = false,
                IsDuplicated = false,
                ManifestRecordId = 1,
                PortCode = "PortCode",
                RulePortPort = "RulePortPort",
                Supplier = "Supplier",
                TrainNumber = "TrainNumber",
                HasAllRequiredRules = true
            };
            action?.Invoke(model);
            return model;
        }

        private RailInboundReadModel CreateValidModelWithFilingHeader(Action<RailInboundReadModel> action = null)
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.FilingHeaderId = 55;
            });
            action?.Invoke(model);
            return model;
        }

        [DataRow(0, false)]
        [DataRow(-60, false)]
        [DataRow(-61, true)]
        [DataTestMethod]
        public void Validate_IsArchived_ReturnValidValue(int dayOffset, bool result)
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.CreatedDate = DateTime.Now.AddDays(dayOffset);
            });
            Assert.AreEqual(result, model.IsArchived);
        }

        [DataRow(null, 0, false, false, true)]
        [DataRow(MappingStatus.Open, 0, false, false, true)]
        [DataRow(MappingStatus.InReview, 0, false, false, false)]
        [DataRow(MappingStatus.InProgress, 0, false, false, false)]
        [DataRow(MappingStatus.Mapped, 0, false, false, false)]
        [DataRow(MappingStatus.Error, 0, false, false, false)]
        [DataRow(null, -61, false, false, false)]
        [DataRow(null, 0, true, false, false)]
        [DataRow(null, 0, false, true, false)]
        [DataTestMethod]
        public void Validate_CanBeFiled_ReturnValidValue(MappingStatus mappingStatus, int daysOffset, bool isDel, bool isDup, bool result)
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.MappingStatus = mappingStatus;
                x.CreatedDate = DateTime.Now.AddDays(daysOffset);
                x.IsDeleted = isDel;
                x.IsDuplicated = isDup;
            });
            Assert.AreEqual(result, model.CanBeFiled());
        }

        [DataRow(null, null, 0, false, false, true)]
        [DataRow(null, null, 0, false, true, true)]
        [DataRow(null, null, 0, false, false, true)]
        [DataRow(MappingStatus.Open, null, 0, false, false, true)]
        [DataRow(MappingStatus.Error, null, 0, false, false, true)]
        [DataRow(null, FilingStatus.Open, 0, false, false, true)]
        [DataRow(null, FilingStatus.Error, 0, false, false, true)]
        [DataRow(MappingStatus.Open, FilingStatus.Open, 0, false, false, true)]
        [DataRow(null, null, 0, true, false, false)]
        [DataRow(MappingStatus.InProgress, null, 0, false, false, false)]
        [DataRow(MappingStatus.InReview, null, 0, true, false, false)]
        [DataRow(MappingStatus.Mapped, null, 0, false, false, false)]
        [DataRow(null, FilingStatus.InProgress, 0, false, false, false)]
        [DataRow(null, FilingStatus.Filed, 0, false, false, false)]
        [DataTestMethod]
        public void Validate_CanBeDeleted_ReturnValidValue(MappingStatus mappingStatus, FilingStatus filingStatus, int daysOffset, bool isDel, bool isDup, bool result)
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.MappingStatus = mappingStatus;
                x.FilingStatus = filingStatus;
                x.CreatedDate = DateTime.Now.AddDays(daysOffset);
                x.IsDeleted = isDel;
                x.IsDuplicated = isDup;
            });
            Assert.AreEqual(result, model.CanBeDeleted());
        }

        [DataRow(true, true)]
        [DataRow(false, false)]
        [DataTestMethod]
        public void Validate_CanBeRestore_ReturnValidValue(bool isDel, bool result)
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.IsDeleted = isDel;
            });
            Assert.AreEqual(result, model.CanBeRestored());
        }

        [DataRow(MappingStatus.InProgress, true)]
        [DataRow(MappingStatus.Mapped, true)]
        [DataRow(MappingStatus.Open, false)]
        [DataRow(MappingStatus.InReview, false)]
        [DataRow(MappingStatus.Error, false)]
        [DataTestMethod]
        public void Validate_CanBeViewed_ReturnValidValue(MappingStatus mappingStatus, bool result)
        {
            RailInboundReadModel model = CreateValidModelWithFilingHeader(x =>
            {
                x.MappingStatus = mappingStatus;
            });
            Assert.AreEqual(result, model.CanBeViewed());
        }

        [TestMethod]
        public void Validate_CanBeViewed_WithoutFilingHeader_Returns_false()
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.FilingHeaderId = null;
                x.MappingStatus = MappingStatus.Mapped;
            });
            Assert.AreEqual(false, model.CanBeViewed());
        }

        [DataRow(MappingStatus.InReview, null, 0, false, false, true)]
        [DataRow(MappingStatus.Error, null, 0, false, false, true)]
        [DataRow(MappingStatus.InReview, FilingStatus.Open, 0, false, false, true)]
        [DataRow(MappingStatus.InReview, FilingStatus.Error, 0, false, false, true)]
        [DataRow(null, null, 0, false, false, false)]
        [DataRow(MappingStatus.Open, null, 0, false, false, false)]
        [DataRow(MappingStatus.InProgress, null, 0, false, false, false)]
        [DataRow(MappingStatus.Mapped, null, 0, false, false, false)]
        [DataRow(MappingStatus.InReview, null, -61, false, false, false)]
        [DataRow(MappingStatus.InReview, null, 0, true, false, false)]
        [DataRow(MappingStatus.InReview, null, 0, false, true, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.InProgress, 0, false, false, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.Filed, 0, false, false, false)]
        [DataTestMethod]
        public void Validate_CanBeEdited_ReturnValidValue(MappingStatus mappingStatus, FilingStatus filingStatus, int daysOffset, bool isDel, bool isDup, bool result)
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.MappingStatus = mappingStatus;
                x.FilingStatus = filingStatus;
                x.CreatedDate = DateTime.Now.AddDays(daysOffset);
                x.IsDeleted = isDel;
                x.IsDuplicated = isDup;
            });
            Assert.AreEqual(result, model.CanBeEdited());
        }

        [DataRow(MappingStatus.InReview, null, true)]
        [DataRow(MappingStatus.InReview, FilingStatus.Open, true)]
        [DataRow(MappingStatus.InReview, FilingStatus.Error, true)]
        [DataRow(MappingStatus.Error, null, true)]
        [DataRow(MappingStatus.Error, FilingStatus.Open, true)]
        [DataRow(MappingStatus.Error, FilingStatus.Error, true)]
        [DataRow(null, null, false)]
        [DataRow(MappingStatus.Open, null, false)]
        [DataRow(MappingStatus.InProgress, null, false)]
        [DataRow(MappingStatus.Mapped, null, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.Filed, false)]
        [DataTestMethod]
        public void Validate_CanBeReverted_ReturnValidValue(MappingStatus mappingStatus, FilingStatus filingStatus, bool result)
        {
            RailInboundReadModel model = CreateValidModelWithFilingHeader(x =>
            {
                x.MappingStatus = mappingStatus;
                x.FilingStatus = filingStatus;
            });
            Assert.AreEqual(result, model.CanBeCanceled());
        }

        [TestMethod]
        public void Validate_CanBeReverted_WithoutFilingHeader_Returns_false()
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.MappingStatus = MappingStatus.InReview;
                x.FilingStatus = FilingStatus.Open;
                x.FilingHeaderId = null;
            });
            Assert.AreEqual(false, model.CanBeCanceled());
        }

        [DataTestMethod]
        [DataRow(0, false, false, true)]
        [DataRow(-60, false, false, true)]
        [DataRow(-61, false, false, false)]
        [DataRow(0, true, false, true)]
        [DataRow(0, false, true, false)]
        public void Validate_CanBeSelected(int daysOffset, bool isDel, bool isDup, bool result)
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.CreatedDate = DateTime.Now.AddDays(daysOffset);
                x.IsDeleted = isDel;
                x.IsDuplicated = isDup;
            });
            Assert.AreEqual(result, model.CanBeSelected());
        }

        [DataTestMethod]
        [DataRow(0, false, false, "Open")]
        [DataRow(-60, false, false, "Open")]
        [DataRow(-61, false, false, "Archived")]
        [DataRow(0, true, false, "Deleted")]
        [DataRow(0, false, true, "Duplicated")]
        [DataRow(-61, true, true, "Deleted; Duplicated; Archived")]
        public void Validate_Status(int daysOffset, bool isDel, bool isDup, string result)
        {
            RailInboundReadModel model = CreateValidModel(x =>
            {
                x.CreatedDate = DateTime.Now.AddDays(daysOffset);
                x.IsDeleted = isDel;
                x.IsDuplicated = isDup;
            });
            Assert.AreEqual(result, model.Status);
        }

    }
}
