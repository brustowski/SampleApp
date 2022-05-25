using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailRulePortRepositoryTests : RepositoryTestBase
    {
        private RailRulePortRepository _repository;

        protected override void TestInit()
        {
            _repository = new RailRulePortRepository(UnitOfWorkFactory);
        }

        private RailRulePort CreateValidModel(Action<RailRulePort> action = null)
        {
            var model = new RailRulePort
            {
                Port = "Port",
                Origin = "Origin",
                Destination = "Destination",
                FIRMsCode = "FIRMsCode",
                Export = "Export"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsExistingRule_WithSameId_ReturnsFalse()
        {
            RailRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRulePort rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentId_ReturnsTrue()
        {
            RailRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRulePort rule2 = CreateValidModel();

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAndPort_ReturnsFalse()
        {
            RailRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRulePort rule2 = CreateValidModel(r => r.Port = "Port 2");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithNullRule_ReturnsFalse()
        {
            RailRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithEmptyPort_ReturnsFalse()
        {
            RailRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRulePort rule2 = CreateValidModel(r => r.Port = string.Empty);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithNullPort_ReturnsFalse()
        {
            RailRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            RailRulePort rule2 = CreateValidModel(r => r.Port = null);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }
    }
}
