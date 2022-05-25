using FilingPortal.DataLayer.Repositories.VesselImport;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.VesselImport
{
    [TestClass]
    public class VesselRuleProductRepositoryTests : RepositoryTestBase
    {
        private VesselRuleProductRepository _repository;

        protected override void TestInit()
        {
            _repository = new VesselRuleProductRepository(UnitOfWorkFactory);
        }

        private VesselRuleProduct CreateValidModel(Action<VesselRuleProduct> action = null)
        {
            var model = new VesselRuleProduct
            {
                Tariff = "Tariff",
                GoodsDescription = "GoodsDescription",
                CustomsAttribute1 = "CustomsAttribute1",
                CustomsAttribute2 = "CustomsAttribute2",
                InvoiceUQ = "InvoiceUQ",
                TSCARequirement = "TSCARequirement",
                CreatedUser = "User"

            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsDuplicated_WithSameId_ReturnsFalse()
        {
            VesselRuleProduct rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRuleProduct rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithDifferentId_ReturnsTrue()
        {
            VesselRuleProduct rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRuleProduct rule2 = CreateValidModel();

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDuplicated_WithDifferentIdAndUniqueValue_ReturnsFalse()
        {
            VesselRuleProduct rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRuleProduct rule2 = CreateValidModel(r => r.Tariff = "Tariff2");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithNullRule_ReturnsFalse()
        {
            VesselRuleProduct rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithEmptyUniqueValue_ReturnsFalse()
        {
            VesselRuleProduct rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRuleProduct rule2 = CreateValidModel(r => r.Tariff = string.Empty);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithNullUniqueValue_ReturnsFalse()
        {
            VesselRuleProduct rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRuleProduct rule2 = CreateValidModel(r => r.Tariff = null);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExist_WithExistingRecordId_ReturnTrue()
        {
            VesselRuleProduct rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            Assert.IsTrue(_repository.IsExist(rule.Id));
        }

        [TestMethod]
        public void IsExist_WithNonexistentRecordId_ReturnFalse()
        {
            VesselRuleProduct rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            Assert.IsFalse(_repository.IsExist(rule.Id + 1));
        }
    }
}
