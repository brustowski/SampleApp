using System;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services
{
    [TestClass]
    public class RailRuleImporterSupplierServiceTest
    {
        private IRuleService<RailRuleImporterSupplier> service;
        private Mock<IRuleRepository<RailRuleImporterSupplier>> mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<IRuleRepository<RailRuleImporterSupplier>>();

            service = new RuleService<RailRuleImporterSupplier>(mockRepository.Object);
        }

        [TestMethod]
        public void Add_ToAddNewRule_CallRepositoryAdd()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };
            
            service.Create(rule);

            mockRepository.Verify(x => x.Add(It.Is<RailRuleImporterSupplier>(r => r.Id == rule.Id)), Times.Once);
        }

        [TestMethod]
        public void Add_ToAddNewRule_HasCreatedDateAndDefaultUser()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };

            service.Create(rule);

            mockRepository.Verify(x => x.Add(It.Is<RailRuleImporterSupplier>(r =>
                r.CreatedDate.Date == DateTime.Now.Date &&
                r.CreatedUser == "sa")), Times.Once);
        }

        [TestMethod]
        public void Add_ToAddNewRule_CallRepositorySave()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };

            service.Create(rule);

            mockRepository.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Update_ToUpdateRule_CallRepositoryUpdate()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };

            mockRepository.Setup(x => x.IsExist(It.IsAny<int>())).Returns(true);

            var result = service.Update(rule);

            mockRepository.Verify(x => x.Update(It.Is<RailRuleImporterSupplier>(r => r.Id == rule.Id)), Times.Once);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Update_RuleNotFound_ReturnOperationResultWithError()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };

            mockRepository.Setup(x => x.IsExist(It.IsAny<int>())).Returns(false);

            var result = service.Update(rule);

            mockRepository.Verify(x => x.Update(It.Is<RailRuleImporterSupplier>(r => r.Id == rule.Id)), Times.Never);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Update_ToUpdateRule_CallRepositorySave()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };

            mockRepository.Setup(x => x.IsExist(It.IsAny<int>())).Returns(true);

            service.Update(rule);

            mockRepository.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Delete_ToDeleteRule_CallRepositoryDeleteById()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };

            mockRepository.Setup(x => x.IsExist(It.IsAny<int>())).Returns(true);

            var result = service.Delete(rule.Id);

            mockRepository.Verify(x => x.DeleteById(It.Is<int>(id => id == rule.Id)), Times.Once);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Delete_ToDeleteRule_CallRepositorySave()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };

            mockRepository.Setup(x => x.IsExist(It.IsAny<int>())).Returns(true);

            service.Delete(rule.Id);

            mockRepository.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Delete_RuleNotFound_ReturnOperationResultWithError()
        {
            var rule = new RailRuleImporterSupplier
            {
                Id = 36
            };

            mockRepository.Setup(x => x.IsExist(It.IsAny<int>())).Returns(false);

            var result = service.Delete(rule.Id);

            mockRepository.Verify(x => x.DeleteById(It.Is<int>(id => id == rule.Id)), Times.Never);
            Assert.IsFalse(result.IsValid);
        }
    }
}
