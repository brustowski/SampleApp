using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailRuleDescriptionRepositoryTests : RepositoryTestBase
    {
        private RailRuleDescriptionRepository _repository;

        protected override void TestInit()
        {
            _repository = new RailRuleDescriptionRepository(UnitOfWorkFactory);
        }

        private RailRuleDescription CreateValidModel(Action<RailRuleDescription> action = null)
        {
            var model = new RailRuleDescription
            {
                Description1 = "Description1",
                Importer = "Importer",
                Supplier = "Supplier",
                Port = "Port",
                Destination = "AK",
                ProductID = "ProductID",
                Attribute1 = "Attribute",
                Tariff = "Tariff",
                GoodsDescription = "GoodsDescription",
                InvoiceUOM = "InvoiceUOM",
                TemplateHTSQuantity = 13.47M,
                TemplateInvoiceQuantity = 13.47M
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsExistingRule_WithSameId_ReturnsFalse()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRuleDescription rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentId_ReturnsTrue()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRuleDescription rule2 = CreateValidModel();

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAndDescription1_ReturnsFalse()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRuleDescription rule2 = CreateValidModel(r => r.Description1 = "Different Description1");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAndImporter_ReturnsFalse()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRuleDescription rule2 = CreateValidModel(r => r.Importer = "Different Importer");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAnSupplier_ReturnsFalse()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRuleDescription rule2 = CreateValidModel(r => r.Supplier = "Different Supplier");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAndPort_ReturnsFalse()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRuleDescription rule2 = CreateValidModel(r => r.Port = "Different Port");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAndDestination_ReturnsFalse()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRuleDescription rule2 = CreateValidModel(r => r.Destination = "AL");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithAllDifferentParameters_ReturnsFalse()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRuleDescription rule2 = CreateValidModel(r =>
            {
                r.Description1 = "Different Description1";
                r.Importer = "Different Importer";
                r.Supplier = "Different Supplier";
                r.Port = "Different Port";
                r.Destination = "AL";

            });

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithNullRule_ReturnsFalse()
        {
            RailRuleDescription rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }
    }
}
