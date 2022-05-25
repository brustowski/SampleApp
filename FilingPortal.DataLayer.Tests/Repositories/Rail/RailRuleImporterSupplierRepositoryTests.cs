using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailRuleImporterSupplierRepositoryTests : RepositoryTestBase
    {
        private RailRuleImporterSupplierRepository _repository;

        protected override void TestInit()
        {
            _repository = new RailRuleImporterSupplierRepository(UnitOfWorkFactory);
        }

        [TestMethod]
        public void IsExistingRule_WithSameId_ReturnsFalse()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = "i1", SupplierName = "s1" };
            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(rule);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithZeroIdAndSameKeyColumns_ReturnsTrue()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = "i1", SupplierName = "s1", ProductDescription = "pd1", Port = "p1", Destination = "d1"};
            var ruleNew = new RailRuleImporterSupplier { ImporterName = "i1", SupplierName = "s1", ProductDescription = "pd1", Port = "p1", Destination = "d1" };
            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(ruleNew);

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void EqualImporterSupplier_OnSave_ReturnsException()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = "i1", SupplierName = "s1" };
            var ruleNew = new RailRuleImporterSupplier { ImporterName = "i1", SupplierName = "s1" };
            _repository.Add(rule);
            _repository.Add(ruleNew);
            _repository.Save();
        }

        [TestMethod]
        public void IsDuplicated_WithLeadingWhitespaces_ReturnsTrue()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = "i1", SupplierName = "s1", ProductDescription = "p1"};
            _repository.Add(rule);
            _repository.Save();

            var ruleNew = new RailRuleImporterSupplier { ImporterName = " i1", SupplierName = " s1", ProductDescription = " p1" };
            var result = _repository.IsDuplicate(ruleNew);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAndImporterName_ReturnsFalse()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = "i1", SupplierName = "s1" };
            _repository.Add(rule);
            _repository.Save();

            var ruleNew = new RailRuleImporterSupplier { ImporterName = "i2", SupplierName = "s1" };
            var result = _repository.IsDuplicate(ruleNew);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAndSupplierName_ReturnsFalse()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = "i1", SupplierName = "s1" };
            _repository.Add(rule);
            _repository.Save();

            var ruleNew = new RailRuleImporterSupplier { ImporterName = "ii", SupplierName = "s2" };
            var result = _repository.IsDuplicate(ruleNew);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExistingRule_WithDifferentIdAndWhitespaces_ReturnsTrue()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = "i1  ", SupplierName = "  s1" };
            _repository.Add(rule);
            _repository.Save();

            var ruleNew = new RailRuleImporterSupplier { ImporterName = " i1 ", SupplierName = "s1 " };
            var result = _repository.IsDuplicate(ruleNew);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDuplicated_WithEmptyImporterSupplier_ReturnsFalse()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = "", SupplierName = "" };

            var result = _repository.IsDuplicate(rule);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithNullImporterSupplier_ReturnsFalse()
        {
            var rule = new RailRuleImporterSupplier { ImporterName = null, SupplierName = null };

            var result = _repository.IsDuplicate(rule);

            Assert.IsFalse(result);
        }
    }
}
