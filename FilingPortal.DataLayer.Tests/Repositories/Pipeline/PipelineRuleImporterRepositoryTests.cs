using FilingPortal.DataLayer.Repositories.Pipeline;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.Pipeline
{
    [TestClass]
    public class PipelineRuleImporterRepositoryTests : RepositoryTestBase
    {
        private PipelineRuleImporterRepository _repository;

        protected override void TestInit()
        {
            _repository = new PipelineRuleImporterRepository(UnitOfWorkFactory);
        }

        private PipelineRuleImporter CreateValidModel(Action<PipelineRuleImporter> action = null)
        {
            var model = new PipelineRuleImporter
            {
                Consignee = "Consignee",
                CountryOfExport = "CountryOfExport",
                FTARecon = "FTARecon",
                Importer = "Importer",
                Manufacturer = "Manufacturer",
                ManufacturerAddress = "ManufacturerAddress",
                MID = "MID",
                Origin = "Origin",
                ReconIssue = "ReconIssue",
                Seller = "Seller",
                SPI = "SPI",
                Supplier = "Supplier",
                TransactionRelated = "TransactionRelated",
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsDuplicated_WithSameId_ReturnsFalse()
        {
            PipelineRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            PipelineRuleImporter rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithDifferentId_ReturnsTrue()
        {
            PipelineRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            PipelineRuleImporter rule2 = CreateValidModel();

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDuplicated_WithNullRule_ReturnsFalse()
        {
            PipelineRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }

        [DataRow("Importer", true)]
        [DataRow("Importer2", false)]
        [DataRow("", false)]
        [DataRow(null, false)]
        [DataTestMethod]
        public void IsDuplicated_WithDifferentId(string importer, bool expectedResult)
        {
            PipelineRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            PipelineRuleImporter rule2 = CreateValidModel(x => x.Importer = importer);

            var result = _repository.IsDuplicate(rule2);

            Assert.AreEqual(expectedResult, result);
        }
    }
}
