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
    public class RailRuleDefValuesTest
    {
        private IRuleService<RailDefValues> service;
        private Mock<IRuleRepository<RailDefValues>> mockRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new Mock<IRuleRepository<RailDefValues>>();

            service = new RuleService<RailDefValues>(mockRepository.Object);
        }

        [TestMethod]
        public void Add_ToAddNewRule_HasCreatedDate()
        {
            var rule = new RailDefValues();

            service.Create(rule);

            mockRepository.Verify(x => x.Add(It.Is<RailDefValues>(r =>
                r.CreatedDate.Date == DateTime.Now.Date)), Times.Once);
        }
    }
}