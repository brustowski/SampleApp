using System.Linq;
using FilingPortal.Web.GridConfigurations;
using FilingPortal.Web.GridConfigurations.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.GridConfigurations.Pipeline
{
    [TestClass]
    public class PipelineDefaultValuesGridConfigTest
    {
        private PipelineDefaultValuesGridConfig _gridConfig;

        [TestInitialize]
        public void TestInitialize()
        {
            _gridConfig = new PipelineDefaultValuesGridConfig();
            _gridConfig.Configure();
        }

        [TestMethod]
        public void GridName_ReturnsCorrectGridName()
        {
            var result = _gridConfig.GridName;

            Assert.AreEqual(GridNames.PipelineDefaultValues, result);
        }

        [TestMethod]
        public void GetColumns_ReturnsDefValueColumn()
        {
            var result = _gridConfig.GetColumns();

            Assert.IsTrue(result.Any(x => x.FieldName == "DefValue"));
        }

        [TestMethod]
        public void GetColumns_Returns_OnlyResizableColumns()
        {
            var result = _gridConfig.GetColumns().ToList();
            Assert.IsTrue(result.All(x => x.IsResizable));
        }
    }
}
