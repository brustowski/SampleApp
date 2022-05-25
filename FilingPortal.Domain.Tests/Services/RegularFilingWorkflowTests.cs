using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Services
{
    [TestClass]
    public class RegularFilingWorkflowTests : FilingWorkflowTests<FilingWorkflow<TestFilingHeader, DefValuesManualTest>>
    {
        protected override FilingWorkflow<TestFilingHeader, DefValuesManualTest> GetFilingWorkflow() =>
            new FilingWorkflow<TestFilingHeader, DefValuesManualTest>(FilingHeadersRepositoryMock.Object, _defValuesManualRepositoryMock.Object);
    }
}
