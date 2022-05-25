using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.DataLayer.Repositories.Audit.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Audit.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Repositories.Audit.Rail
{
    [TestClass]
    public class RailDailyAuditSpiRulesRepositoryTests : RepositoryTestBase
    {
        private RailDailyAuditSpiRulesRepository _repository;


        private AuditRailDailySpiRule CreateValidRule(Action<AuditRailDailySpiRule> action = null)
        {
            var rule = new AuditRailDailySpiRule()
            {
                SupplierCode = "SupplierCode",
                GoodsDescription = "Goods Description",
                DateFrom = new DateTime(2020, 1, 1),
                DateTo = new DateTime(2020, 1, 15),
                DestinationState = "AL",
                ImporterCode = "ImporterCode",
                Spi = "VL",
                CreatedUser = CurrentUser.Id,
                CreatedDate = new DateTime(2020, 1, 1),
                Author = CurrentUser
            };

            action?.Invoke(rule);

            return rule;
        }

        private AuditRailDaily CreateValidRecord(Action<AuditRailDaily> action = null)
        {
            var record = new AuditRailDaily()
            {
                SupplierCode = "SupplierCode",
                GoodsDescription = "Goods Description",
                ImportDate = new DateTime(2020, 1, 1),
                ReleaseDate = new DateTime(2020, 1, 1),
                DestinationState = "AL",
                ImporterCode = "ImporterCode",
                Spi = "VL",
                CreatedUser = CurrentUser.Id,
                CreatedDate = new DateTime(2020, 1, 1),
            };

            action?.Invoke(record);

            return record;
        }

        protected override void TestInit()
        {
            _repository = new RailDailyAuditSpiRulesRepository(UnitOfWorkFactory);
        }

        [TestMethod]
        public async Task CorrectRuleIsSaving()
        {
            AuditRailDailySpiRule rule = CreateValidRule();

            _repository.Add(rule);

            await _repository.SaveAsync();
        }

        [TestMethod]
        public async Task FindCorrespondingRules_returns_rule_based_on_release_date()
        {
            AuditRailDaily record = CreateValidRecord(x =>
            {
                x.ImportDate = new DateTime(2020, 1, 1);
                x.ReleaseDate = new DateTime(2020, 5, 5);
            });

            AuditRailDailySpiRule correctRule = CreateValidRule(rule =>
            {
                rule.DateFrom = new DateTime(2020, 5, 1);
                rule.DateTo = new DateTime(2020, 5, 15);
            });

            AuditRailDailySpiRule incorrectRule = CreateValidRule();

            _repository.Add(correctRule);
            _repository.Add(incorrectRule);

            await _repository.SaveAsync();

            IList<AuditRailDailySpiRule> rules = await _repository.FindCorrespondingRules(record);

            Assert.IsTrue(rules.Any());
            CollectionAssert.Contains((ICollection)rules, correctRule);
            CollectionAssert.DoesNotContain((ICollection)rules, incorrectRule);
        }
    }
}