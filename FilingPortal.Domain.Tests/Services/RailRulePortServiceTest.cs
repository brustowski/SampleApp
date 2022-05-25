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
    public class RailRulePortServiceTest
    {
        private IRuleService<RailRulePort> service;
        private Mock<IRuleRepository<RailRulePort>> mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<IRuleRepository<RailRulePort>>();

            service = new RuleService<RailRulePort>(mockRepository.Object);
        }

        [TestMethod]
        public void Add_ToAddNewRule_HasCreatedDateAndDefaultUser()
        {
            var rule = new RailRulePort();

            service.Create(rule);

            mockRepository.Verify(x => x.Add(It.Is<RailRulePort>(r =>
                r.CreatedDate.Date == DateTime.Now.Date &&
                r.CreatedUser == "sa")), Times.Once);
        }
    }
}