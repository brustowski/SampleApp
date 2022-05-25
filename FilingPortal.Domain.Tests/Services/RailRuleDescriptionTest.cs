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
    public class RailRuleDescriptionTest
    {
        private IRuleService<RailRuleDescription> service;
        private Mock<IRuleRepository<RailRuleDescription>> mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<IRuleRepository<RailRuleDescription>>();

            service = new RuleService<RailRuleDescription>(mockRepository.Object);
        }

        [TestMethod]
        public void Add_ToAddNewRule_HasCreatedDateAndDefaultUser()
        {
            var rule = new RailRuleDescription();

            service.Create(rule);

            mockRepository.Verify(x => x.Add(It.Is<RailRuleDescription>(r =>
                r.CreatedDate.Date == DateTime.Now.Date &&
                r.CreatedUser == "sa")), Times.Once);
        }
    }
}