using System;
using System.Collections.Generic;
using FilingPortal.Domain.Services.GridExport.Formatters;
using Framework.Infrastructure;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services.GridExport.Formatters
{
    [TestClass]
    public class EnumFormatterTests
    {
        #region Setup

        private EnumFormatter<TestEnum> Formatter { get; set; }

        #endregion

        [TestInitialize]
        public void Init()
        {
            Formatter = new EnumFormatter<TestEnum>();
        }

        [TestMethod]
        public void Format_NullPassed_ReturnsEmptyString()
        {
            var value = Formatter.Format(null);

            value.ShouldBeEqualTo(string.Empty);
        }

        [TestMethod]
        public void Format_ValidEnumPassed_ReturnsDescription()
        {
            var value = Formatter.Format(TestEnum.Value1);

            value.ShouldBeEqualTo("Description1");
        }

        [TestMethod]
        public void Format_ValidEnumPassedAndNoDescription_Value()
        {
            var value = Formatter.Format(TestEnum.Value2);

            value.ShouldBeEqualTo("Value2");
        }

        [TestMethod]
        public void Format_NotEnumValuePassed_ReturnsException()
        {
            Action act = () => Formatter.Format(NotIncludedTestEnum.Value);

            AssertThat.Throws<KeyNotFoundException>(act, "Enum value `Value` not found in `FilingPortal.Domain.Tests.Services.GridExport.Formatters.TestEnum`.");
        }
    }
}
