using System;
using FilingPortal.DataLayer.Repositories.VesselImport;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Repositories.VesselImport
{
    [TestClass]
    public class VesselRuleImporterRepositoryTests : RepositoryTestBase
    {
        private VesselRuleImporterRepository _repository;

        protected override void TestInit()
        {
            _repository = new VesselRuleImporterRepository(UnitOfWorkFactory);
        }

        private VesselRuleImporter CreateValidModel(Action<VesselRuleImporter> action = null)
        {
            var model = new VesselRuleImporter
            {
                Importer = "Importer",
                CWImporter = "CWImporter",
                CreatedUser = "User"

            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsDublicated_WithSameId_ReturnsFalse()
        {
            var rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithDifferentId_ReturnsTrue()
        {
            var rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var rule2 = CreateValidModel();

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDublicated_WithDifferentIdAndUniqueValue_ReturnsFalse()
        {
            var rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var rule2 = CreateValidModel(r => r.Importer= "Importer2");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithNullRule_ReturnsFalse()
        {
            var rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithEmptyUniqueValue_ReturnsFalse()
        {
            var rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var rule2 = CreateValidModel(r => r.Importer = string.Empty);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithNullUniqueValue_ReturnsFalse()
        {
            var rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var rule2 = CreateValidModel(r => r.Importer = null);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExist_WithExistingRecordId_ReturnTrue()
        {
            var rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            Assert.IsTrue(_repository.IsExist(rule.Id));
        }

        [TestMethod]
        public void IsExist_WithNonexistentRecordId_ReturnFalse()
        {
            var rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            Assert.IsFalse(_repository.IsExist(rule.Id + 1));
        }
    }
}
