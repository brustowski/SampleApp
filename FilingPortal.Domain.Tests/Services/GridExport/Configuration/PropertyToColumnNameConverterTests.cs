using System;
using FilingPortal.Domain.Services.GridExport.Configuration;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services.GridExport.Configuration
{
    [TestClass]
    public class PropertyToColumnNameConverterTests
    {
        #region Setup

        private PropertyToColumnNameConverter PropertyToColumnNameConverter { get; set; }

        #endregion

        [TestInitialize]
        public void Init()
        {
            PropertyToColumnNameConverter = new PropertyToColumnNameConverter();
        }

        [TestMethod]
        public void Convert_WhenCapitalLettersExists_SplitsByThem()
        {
            var name = "BasicCaseToTest";

            var splitted = PropertyToColumnNameConverter.Convert(name);

            splitted.ShouldBeEqualTo("Basic Case To Test");
        }

        [TestMethod]
        public void Convert_WhenAbbreviation_SplitsByLetter()
        {
            var name = "ABBREVIATION";

            var splitted = PropertyToColumnNameConverter.Convert(name);

            splitted.ShouldBeEqualTo("A B B R E V I A T I O N");
        }

        [TestMethod]
        public void Convert_WhenHTS_DoesNotSplit()
        {
            var name = "AnHTSValue";

            var splitted = PropertyToColumnNameConverter.Convert(name);

            splitted.ShouldBeEqualTo("An HTS Value");
        }
    }
}
