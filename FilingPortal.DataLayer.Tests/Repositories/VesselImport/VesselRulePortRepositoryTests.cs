using System;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.DataLayer.Repositories.VesselImport;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Repositories.VesselImport
{
    [TestClass]
    public class VesselRulePortRepositoryTests : RepositoryTestBase
    {
        private VesselRulePortRepository _repository;

        protected override void TestInit()
        {


            _repository = new VesselRulePortRepository(UnitOfWorkFactory);
        }

        private VesselRulePort CreateValidModel(Action<VesselRulePort> action = null)
        {
            var model = new VesselRulePort
            {
                DischargeName = "DischargeName",
                EntryPort = "Port",
                DischargePort = "Port",
                FirmsCode = new CargowiseFirmsCodes { FirmsCode = "1234", Name = "1234", IsActive = true, LastUpdatedTime = DateTime.Now },
                HMF = "Y",
                DestinationCode = "11001",
                CreatedUser = "User"

            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void IsDuplicated_WithSameId_ReturnsFalse()
        {
            VesselRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRulePort rule2 = CreateValidModel(r => r.Id = rule.Id);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithDifferentId_ReturnsTrue()
        {
            VesselRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRulePort rule2 = CreateValidModel();
            rule2.FirmsCode = rule.FirmsCode;
            rule2.FirmsCodeId = rule.FirmsCodeId;

            var result = _repository.IsDuplicate(rule2);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDuplicated_WithDifferentIdAndUniqueValue_ReturnsFalse()
        {
            VesselRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRulePort rule2 = CreateValidModel(r => r.DischargeName = "DischargeName2");

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithNullRule_ReturnsFalse()
        {
            VesselRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            var result = _repository.IsDuplicate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithEmptyUniqueValue_ReturnsFalse()
        {
            VesselRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRulePort rule2 = CreateValidModel(r => r.DischargeName = string.Empty);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicated_WithNullUniqueValue_ReturnsFalse()
        {
            VesselRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            VesselRulePort rule2 = CreateValidModel(r => r.DischargeName = null);

            var result = _repository.IsDuplicate(rule2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsExist_WithExistingRecordId_ReturnTrue()
        {
            VesselRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            Assert.IsTrue(_repository.IsExist(rule.Id));
        }

        [TestMethod]
        public void IsExist_WithNonexistentRecordId_ReturnFalse()
        {
            VesselRulePort rule = CreateValidModel();

            _repository.Add(rule);
            _repository.Save();

            Assert.IsFalse(_repository.IsExist(rule.Id + 1));
        }
    }
}
