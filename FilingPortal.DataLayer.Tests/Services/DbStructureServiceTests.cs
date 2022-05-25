using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Parts.Common.DataLayer.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Services
{
    [TestClass()]
    public class DbStructureServiceTests : RepositoryTestBase
    {
        private BaseDbStructureService _service;

        protected override void TestInit()
        {
            _service = new BaseDbStructureService(UnitOfWorkFactory);
        }

        [TestMethod()]
        public void GetDbColumnNameTest()
        {
            var columnName = _service.GetDbColumnName<AuditRailDailyRule>("ImporterCode");
            Assert.AreEqual(columnName, "importer_code");
        }

        [TestMethod()]
        public void GetDbTableNameTest()
        {
            var tableName = _service.GetDbTableName<AuditRailDailyRule>();
            Assert.AreEqual(tableName, "dbo.imp_rail_audit_daily_rules");

        }
    }
}