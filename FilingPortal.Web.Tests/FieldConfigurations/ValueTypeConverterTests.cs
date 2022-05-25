using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass]
    public class ValueTypeConverterTests
    {
        private ValueTypeConverter _valueTypeConverter;

        [TestInitialize]
        public void TestInitialize()
        {
            _valueTypeConverter = new ValueTypeConverter();
        }

        [TestMethod]
        public void Convert_DateToDate()
        {
            var result = _valueTypeConverter.Convert("date");

            Assert.AreEqual("Date", result);
        }

        [TestMethod]
        public void Convert_IntToNumber()
        {
            var result = _valueTypeConverter.Convert("int");

            Assert.AreEqual("Number", result);
        }

        [TestMethod]
        public void Convert_NumericToNumber()
        {
            var result = _valueTypeConverter.Convert("numeric");

            Assert.AreEqual("Number", result);
        }

        [TestMethod]
        public void Convert_VarcharToText()
        {
            var result = _valueTypeConverter.Convert("varchar");

            Assert.AreEqual("Text", result);
        }

        [TestMethod]
        public void Convert_UndefinitedToText()
        {
            var result = _valueTypeConverter.Convert("nvarchar");

            Assert.AreEqual("Text", result);
        }
    }
}
