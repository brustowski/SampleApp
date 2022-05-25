using System;
using FilingPortal.Domain.Services.GridExport.Formatters;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services.GridExport.Formatters
{
    [TestClass]
    public class DateTimeNullableFormatterTests
    {
        #region Setup

        private DateTimeNullableFormatter DateTimeFormatter { get; set; }

        #endregion

        [TestInitialize]
        public void Init()
        {
            DateTimeFormatter = new DateTimeNullableFormatter();
        }

        [TestMethod]
        public void Format_ValuePassed_ReturnsCorrectValue()
        {
            var value = DateTimeFormatter.Format(new DateTime(2016, 11, 10));

            value.ShouldBeEqualTo("11/10/2016");
        }

        [TestMethod]
        public void Format_NullPassed_ReturnsCorrectValue()
        {
            var value = DateTimeFormatter.Format(null);

            value.ShouldBeEqualTo("");
        }
    }
}
