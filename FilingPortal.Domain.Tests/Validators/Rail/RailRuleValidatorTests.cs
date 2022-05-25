using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Tests.EntitiesTests;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Validators.Rail
{
    [TestClass]
    public class RailRuleValidatorTests
    {
        private RuleValidator<RuleTest> _validator;
        private Mock<IRuleRepository<RuleTest>> _repositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<IRuleRepository<RuleTest>>();
            _validator = new RuleValidator<RuleTest>(_repositoryMock.Object);
        }

        [TestMethod]
        public void IsDuplicate_WhenCalled_CallsRepository()
        {
            var rule = new RuleTest();

            _validator.IsDuplicate(rule);

            _repositoryMock.Verify(x => x.IsDuplicate(rule), Times.Once);
        }

        [TestMethod]
        public void IsDuplicate_WhenNotDuplicatedInRepository_ReturnsFalse()
        {
            var rule = new RuleTest();
            _repositoryMock.Setup(x => x.IsDuplicate(rule)).Returns(false);

            var result = _validator.IsDuplicate(rule);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDuplicate_WhenDuplicatedInRepository_ReturnsTrue()
        {
            var rule = new RuleTest();
            _repositoryMock.Setup(x => x.IsDuplicate(rule)).Returns(true);

            var result = _validator.IsDuplicate(rule);

            Assert.IsTrue(result);
        }
    }
}
