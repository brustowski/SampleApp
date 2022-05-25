using FilingPortal.DataLayer.Repositories.Truck;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.Truck
{
    [TestClass]
    public class TruckRuleImporterRepositoryTests : RepositoryTestBase
    {
        private TruckRuleImporterRepository _repository;

        protected override void TestInit()
        {
            _repository = new TruckRuleImporterRepository(UnitOfWorkFactory);
        }

        private TruckRuleImporter CreateValidModel(Action<TruckRuleImporter> action = null)
        {
            var model = new TruckRuleImporter
            {
                CWIOR = "CWIOR",
                CWSupplier = "CWSupplier",
                ConsigneeCode = "consignee code",
                ArrivalPort = "Arrival Port",
                CE = "CE",
                Charges = 123456789012.123456M,
                CO = "CO",
                CustomAttrib1 = "CustomAttrib1",
                CustomAttrib2 = "CustomAttrib2",
                CustomQuantity = 123456789012.123456M,
                CustomUQ = "CustomUQ",
                DestinationState = "DestinationState",
                EntryPort = "Entry Port",
                GoodsDescription = "GoodsDescription",
                GrossWeight = 123456789012.123456M,
                GrossWeightUQ = "GrossWeightUQ",
                InvoiceQTY = 123456789012.123456M,
                InvoiceUQ = "InvoiceUQ",
                LinePrice = 123456789012.123456M,
                ManufacturerMID = "ManufacturerMID",
                NAFTARecon = "NAFTARecon",
                ProductID = "ProductID",
                ReconIssue = "ReconIssue",
                SPI = "SPI",
                SupplierMID = "SupplierMID",
                Tariff = "Tariff",
                TransactionsRelated = "TransactionsRelated"

            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsDublicated_WithSameId_ReturnsFalse()
        {
            TruckRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRuleImporter rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithDifferentId_ReturnsTrue()
        {
            TruckRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRuleImporter rule2 = CreateValidModel();

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDublicated_WithDifferentIdImporter_ReturnsFalse()
        {
            TruckRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRuleImporter rule2 = CreateValidModel(r => r.CWIOR = "Importer 2");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithDifferentIdSupplier_ReturnsFalse()
        {
            TruckRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRuleImporter rule2 = CreateValidModel(r => r.CWSupplier = "Supplier 2");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithNullRule_ReturnsFalse()
        {
            TruckRuleImporter rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }
    }
}
