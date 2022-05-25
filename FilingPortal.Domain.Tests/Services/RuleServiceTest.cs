using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services
{
    public class TestRule : Entity { }

    [TestClass]
    public class RuleServiceTest
    {
        private IRuleService<TestRule> service;
        private Mock<IRuleRepository<TestRule>> mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<IRuleRepository<TestRule>>();

            service = new RuleService<TestRule>(mockRepository.Object);
        }

        [TestMethod]
        public void Add_ToAddNewRule_CallRepositoryAdd()
        {
            var rule = new TestRule
            {
                Id = 36
            };

            service.Create(rule);

            mockRepository.Verify(x => x.Add(It.Is<TestRule>(r => r.Id == rule.Id)), Times.Once);
        }

        [TestMethod]
        public void Add_ToAddNewRule_CallRepositorySave()
        {
            var rule = new TestRule
            {
                Id = 36
            };

            service.Create(rule);

            mockRepository.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void Update_ToUpdateRule_CallRepositoryUpdate()
        {
            var rule = new TestRule
            {
                Id = 36
            };

            mockRepository.Setup(x => x.IsExist(It.IsAny<int>())).Returns(true);

            var result = service.Update(rule);

            mockRepository.Verify(x => x.Update(It.Is<TestRule>(r => r.Id == rule.Id)), Times.Once);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Update_RuleNotFound_ReturnOperationResultWithError()
        {
            var rule = new TestRule
            {
                Id = 36
            };

            mockRepository.Setup(x => x.IsExist(It.IsAny<int>())).Returns(false);

            var result = service.Update(rule);

            mockRepository.Verify(x => x.Update(It.Is<TestRule>(r => r.Id == rule.Id)), Times.Never);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Update_ToUpdateRule_CallRepositorySave()
        {
            var rule = new TestRule
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
            var rule = new TestRule
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
            var rule = new TestRule
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
            var rule = new TestRule
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
