using FilingPortal.DataLayer.Repositories.Truck;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.Truck
{
    [TestClass]
    public class TruckRulePortRepositoryTests : RepositoryTestBase
    {
        private TruckRulePortRepository _repository;

        protected override void TestInit()
        {
            _repository = new TruckRulePortRepository(UnitOfWorkFactory);
        }

        private TruckRulePort CreateValidModel(Action<TruckRulePort> action = null)
        {
            var model = new TruckRulePort
            {
                EntryPort = "EntryPort",
                ArrivalPort = "ArrivalPort",
                FIRMsCode = "FIRMsCode",
                CreatedUser = "sa",
                CreatedDate = new DateTime(2020, 1,1)
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsDublicated_WithSameId_ReturnsFalse()
        {
            TruckRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRulePort rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithDifferentId_ReturnsTrue()
        {
            TruckRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRulePort rule2 = CreateValidModel();

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDublicated_WithDifferentIdAndEntryPort_ReturnsFalse()
        {
            TruckRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRulePort rule2 = CreateValidModel(r => r.EntryPort = "Port 2");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithNullRule_ReturnsFalse()
        {
            TruckRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithEmptyEntryPort_ReturnsFalse()
        {
            TruckRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRulePort rule2 = CreateValidModel(r => r.EntryPort = string.Empty);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDublicated_WithNullEntryPort_ReturnsFalse()
        {
            TruckRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            TruckRulePort rule2 = CreateValidModel(r => r.EntryPort = null);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }
    }
}
