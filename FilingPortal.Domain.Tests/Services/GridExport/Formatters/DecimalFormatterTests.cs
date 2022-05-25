using System;
using FilingPortal.Domain.Services.GridExport.Formatters;
using Framework.Infrastructure;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services.GridExport.Formatters
{
    [TestClass]
    public class DecimalFormatterTests
    {
        #region Setup

        private DecimalFormatter Formatter { get; set; }

        #endregion

        [TestInitialize]
        public void Init()
        {
            Formatter = new DecimalFormatter();
        }

        [TestMethod]
        public void Format_ValuePassed_ReturnsCorrectValue()
        {
            var value = Formatter.Format(20123.452m);

            value.ShouldBeEqualTo("20123.452");
        }

        [TestMethod]
        public void Format_NullPassed_ExceptionThrown()
        {
            Action act = () => Formatter.Format(null);

            AssertThat.Throws<NullReferenceException>(act);
        }
    }
}
