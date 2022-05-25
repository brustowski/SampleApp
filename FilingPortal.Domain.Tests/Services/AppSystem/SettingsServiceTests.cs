using System.Collections.Generic;
using FilingPortal.Domain.Repositories.AppSystem;
using FilingPortal.Domain.Services.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services.AppSystem
{
    [TestClass()]
    public class SettingsServiceTests
    {
        private SettingsService _service;
        private Mock<IAppSettingsRepository> _appSettingsRepository;

        [TestInitialize]
        public void Init()
        {
            _appSettingsRepository = new Mock<IAppSettingsRepository>();

            _service = new SettingsService(_appSettingsRepository.Object);
        }

        [TestMethod()]
        public void Get_RailDailyAuditCustomsQtyWarningThreshold_Returns_5_percent()
        {
            decimal railDailyAuditCustomsQtyWarningThreshold = _service.Get<decimal>("RailDailyAuditCustomsQtyWarningThreshold");

            Assert.AreEqual(0.05m, railDailyAuditCustomsQtyWarningThreshold);
        }

        [TestMethod()]
        public void Get_RailDailyAuditCustomsQtyErrorThreshold_Returns_10_percent()
        {
            decimal railDailyAuditCustomsQtyErrorThreshold = _service.Get<decimal>("RailDailyAuditCustomsQtyErrorThreshold");

            Assert.AreEqual(0.1m, railDailyAuditCustomsQtyErrorThreshold);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Get_Any_Setting_Return_Null()
        {
            _service.Get<string>("absentParam");
        }

        [TestMethod]
        public void Get_Repository_Setting_Returns_Its_value()
        {

            _appSettingsRepository.Setup(x => x.Get("repositoryParam")).Returns(new AppSettings()
            { Id = "repositoryParam", Value = "SomeValue" });
            string value = _service.Get<string>("repositoryParam");

            Assert.AreEqual("SomeValue", value);
        }

        [TestMethod()]
        public void Get_RailDailyAuditCustomsQtyWarningThreshold_Returns_Repository_Value()
        {
            _appSettingsRepository.Setup(x => x.Get("RailDailyAuditCustomsQtyWarningThreshold")).Returns(new AppSettings()
            { Id = "RailDailyAuditCustomsQtyWarningThreshold", Value = "0.5" });

            decimal railDailyAuditCustomsQtyWarningThreshold = _service.Get<decimal>("RailDailyAuditCustomsQtyWarningThreshold");

            Assert.AreEqual(0.5m, railDailyAuditCustomsQtyWarningThreshold);
        }
    }
}