using System.Linq;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.Web.GridConfigurations.Audit.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.GridConfigurations.Audit.Rail
{
    [TestClass]
    public class DailyAuditRulesGridConfigTests
    {
        private DailyAuditRulesGridConfig _config;
        private Mock<IKeyFieldsService> _keyFieldsService;

        [TestInitialize]
        public void Init()
        {
            _keyFieldsService = new Mock<IKeyFieldsService>();
            _config = new DailyAuditRulesGridConfig(_keyFieldsService.Object);
        }

        [TestMethod]
        public void GetColumns_With_Column_In_Unique_Returns_Key_Field()
        {
            _keyFieldsService.Setup(x => x.IsKeyField<AuditRailDailyRule>("Carrier")).Returns(true);

            _config.Configure();

            var columns = _config.GetColumns();
            var column = columns.First(x => x.FieldName == "Carrier");

            Assert.IsTrue(column.IsKeyField);
        }

        [TestMethod]
        public void GetColumns_With_Column_Not_In_Unique_Returns_Non_Key_Field()
        {
            _keyFieldsService.Setup(x => x.IsKeyField<AuditRailDailyRule>("Carrier")).Returns(false);

            _config.Configure();

            var columns = _config.GetColumns();
            var column = columns.First(x => x.FieldName == "Carrier");

            Assert.IsFalse(column.IsKeyField);
        }
    }
}