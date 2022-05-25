using System.Collections.Generic;
using FilingPortal.Domain.Services.GridExport.Configuration;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services.GridExport.Configuration
{
    [TestClass]
    public class ReportModelMapContainerTests
    {
        [TestMethod]
        public void GetMap_WhenMapPassedInCtor_CanBeResolved()
        {
            var reportModelMap = new Mock<IReportModelMap>();
            reportModelMap.Setup(x => x.GetModelType).Returns(typeof(SampleReportModel));
            var container = new ReportModelMapContainer(new List<IReportModelMap> { reportModelMap.Object });

            var map = container.GetMap<SampleReportModel>();

            map.ShouldBeEqualTo(reportModelMap.Object);
        }
    }
}
