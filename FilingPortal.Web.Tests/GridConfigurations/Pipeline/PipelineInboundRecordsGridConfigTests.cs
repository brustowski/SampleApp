using System.Linq;
using FilingPortal.Web.GridConfigurations;
using FilingPortal.Web.GridConfigurations.Pipeline;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.Pipeline
{
    [TestClass]
    public class PipelineInboundRecordsGridConfigTests
    {
        private PipelineInboundRecordsGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new PipelineInboundRecordsGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.PipelineInboundRecords, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsCorrectColumnsCount()
        {
            var result = _gridConfig.GetColumns();

            Assert.AreEqual(10, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(10, result.Count());
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectTextFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(5, result.Count(f => f.Type == "text"));
        }

        [TestMethod]
        public void GetFilters_ReturnsCorrectFloatNumberFiltersCount()
        {
            var result = _gridConfig.GetFilters();

            Assert.AreEqual(2, result.Count(f => f.Type == "floatNumber"));
        }

        [TestMethod]
        public void Facility_Is_Configured()
        {
            var result = _gridConfig.GetColumns();

            Assert.IsNotNull(result.FirstOrDefault(x=>x.FieldName == "Facility"));
        }

        [TestMethod]
        public void Facility_Filter_Is_Configured()
        {
            var result = _gridConfig.GetFilters();

            Assert.IsNotNull(result.FirstOrDefault(x => x.FieldName == "Facility" && x.Operand == FilterOperands.Contains));
        }
    }
}
