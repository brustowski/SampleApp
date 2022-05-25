using FilingPortal.Domain.Common.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Common.Parsing
{
    [TestClass]
    public class ParsedDataValidationResultTest
    {
        [TestMethod]
        public void IsValid_WhenCreated_ReturnTrue()
        {
            ParsedDataValidationResult<ParsingDataModel> result = new ParsedDataValidationResult<ParsingDataModel>();
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void IsValid_WithErrors_ReturnFalse()
        {
            ParsedDataValidationResult<ParsingDataModel> result = new ParsedDataValidationResult<ParsingDataModel>();
            result.AddError(new RowError(0, "", ErrorLevel.Critical, "", ""));
            Assert.IsFalse(result.IsValid);
        }
    }
}
