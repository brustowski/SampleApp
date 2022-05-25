using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Infrastructure.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;

namespace FilingPortal.Infrastructure.Tests.Parsing
{
    [TestClass]
    public class ExcelFileSimpleParserTests
    {

        private ExcelFileSimpleParser _parser;
        private Mock<IParseModelMapRegistry> _mapRegistryMock;
        private IParseModelMap _modelMap;

        [TestInitialize]
        public void TestInitialize()
        {
            _modelMap = new TestParseModelMap();
            _mapRegistryMock = new Mock<IParseModelMapRegistry>();
            _mapRegistryMock.Setup(x => x.Get(It.IsAny<Type>())).Returns(_modelMap);
            _mapRegistryMock.Setup(x => x.Get<TestParsingModel>()).Returns(_modelMap);

            _parser = new ExcelFileSimpleParser(_mapRegistryMock.Object);
        }
        

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Parse_ThrowsFileNotFoundException_IfFileDoesNotExist()
        {
            ParsingResult<TestParsingModel> result = _parser.Parse<TestParsingModel>(string.Empty);
        }

        [TestMethod]
        public void Parse_ReturnsValidResult_ForCorrectFile()
        {
            var fileName = Path.Combine("App_Data", "ParseTest.xlsx");
            
            ParsingResult<TestParsingModel> result = _parser.Parse<TestParsingModel>(fileName);

            Assert.IsTrue(result.Errors.Count == 0);
        }
    }
}
