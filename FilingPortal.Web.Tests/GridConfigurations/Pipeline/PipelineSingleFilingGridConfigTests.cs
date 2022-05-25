using System.Linq;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Web.GridConfigurations.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.GridConfigurations.Pipeline
{
    [TestClass()]
    public class PipelineSingleFilingGridConfigTests
    {
        private PipelineSingleFilingGridConfig _gridConfig;
        private Mock<IAgileConfiguration<PipelineDefValueReadModel>> _agileConfig;

        [TestInitialize]
        public void Init()
        {
            _agileConfig = new Mock<IAgileConfiguration<PipelineDefValueReadModel>>();
            _gridConfig = new PipelineSingleFilingGridConfig(_agileConfig.Object);
        }

        [TestMethod()]
        public void Columns_Not_Sortable()
        {
            var result = _gridConfig.GetColumns().ToList();
            Assert.IsTrue(result.All(x => x.IsSortable == false));
        }
    }
}