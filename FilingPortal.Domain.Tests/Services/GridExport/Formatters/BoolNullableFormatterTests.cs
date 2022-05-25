using System;
using FilingPortal.Domain.Services.GridExport.Formatters;
using Framework.Infrastructure;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services.GridExport.Formatters
{
    [TestClass]
    public class BoolNullableFormatterTests
    {
        #region Setup

        private BoolNullableFormatter Formatter { get; set; }

        #endregion

        [TestInitialize]
        public void Init()
        {
            Formatter = new BoolNullableFormatter();
        }

        [TestMethod]
        public void Format_TruePassed_ReturnsYes()
        {
            var value = Formatter.Format(true);

            value.ShouldBeEqualTo("Yes");
        }

        [TestMethod]
        public void Format_FalsePassed_ReturnsNo()
        {
            var value = Formatter.Format(false);

            value.ShouldBeEqualTo("No");
        }

        [TestMethod]
        public void Format_NullPassed_EmptyStringReturned()
        {
            var value = Formatter.Format(null);

            value.ShouldBeEqualTo(string.Empty);
        }

        [TestMethod]
        public void Format_NotABoolPassed_ExceptionThrown()
        {
            Action act = () => Formatter.Format("some value");

            AssertThat.Throws<InvalidCastException>(act);
        }
    }
}
