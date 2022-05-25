using System;
using System.Collections.Generic;
using Framework.Infrastructure;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services.GridExport.Configuration
{
    [TestClass]
    public class ReportModelMapTests
    {
        private SampleReportModelMap map;

        [TestInitialize]
        public void TestInit()
        {
            map = new SampleReportModelMap();
        }
        [TestMethod]
        public void Config_ForString_ReturnsValidValue()
        {
            var expected = "SomeName";
            var info = map.GetConfig("NotMappedName");
            var sampleReportModel = new SampleReportModel()
            {
                NotMappedName = expected
            };

            var name = info.Getter(sampleReportModel);

            name.ShouldBeEqualTo(expected);
        }

        [TestMethod]
        public void Config_ForDecimal_ReturnsValidValue()
        {
            var expected = 12341.534m;
            var info = map.GetConfig("NotMappedValue");
            var sampleReportModel = new SampleReportModel()
            {
                NotMappedValue = expected
            };

            var name = info.Getter(sampleReportModel);

            name.ShouldBeEqualTo(expected);
        }

        [TestMethod]
        public void Config_ByDefaultForString_ReturnsNull()
        {
            var info = map.GetConfig("NotMappedName");

            var formatter = info.ValueFormatter;

            formatter.ShouldBeNull();
        }

        [TestMethod]
        public void Config_ByDefaultPropertyName_IsSplittedCamelCase()
        {
            var info = map.GetConfig("SomeLongWord");

            var title = info.Title;

            title.ShouldBeEqualTo("Some Long Word");
        }

        [TestMethod]
        public void Config_ByDefaultPropertyNameCapitalLetters_IsSplittedCamelCase()
        {
            var info = map.GetConfig("ABBREVIATION");

            var title = info.Title;

            title.ShouldBeEqualTo("A B B R E V I A T I O N");
        }

        [TestMethod]
        public void Config_NameOverriden_ReturnsOverridenTitle()
        {
            var info = map.GetConfig("OverridenName");

            var title = info.Title;

            title.ShouldBeEqualTo("Name Overriden");
        }

        [TestMethod]
        public void Config_IgnoredProperty_NotAddInConfig()
        {
            Action action = () => map.GetConfig("Ignored");

            AssertThat.Throws<KeyNotFoundException>(action);
        }
    }
}
