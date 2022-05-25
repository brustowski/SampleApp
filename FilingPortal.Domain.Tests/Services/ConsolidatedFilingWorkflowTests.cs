using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace FilingPortal.Domain.Tests.Services
{
    [TestClass]
    public class ConsolidatedFilingWorkflowTests : FilingWorkflowTests<ConsolidatedFilingWorkflow<TestFilingHeader, DefValuesManualTest>>
    {
        protected override ConsolidatedFilingWorkflow<TestFilingHeader, DefValuesManualTest> GetFilingWorkflow() =>
            new ConsolidatedFilingWorkflow<TestFilingHeader, DefValuesManualTest>(FilingHeadersRepositoryMock.Object, _defValuesManualRepositoryMock.Object);

        [TestMethod]
        public void Start_CallsStoreProcedure()
        {
            var header = new TestFilingHeader();

            _filingWorkflow.StartUnitTradeFiling(header);

            FilingHeadersRepositoryMock.Verify(x => x.FillDataForFilingHeader(It.IsAny<int>(), null));
        }

        [TestMethod]
        public void CreateUnitTradeFilingHeader_returns_badresult_if_Headers_Save_throws_exception()
        {
            var header = new TestFilingHeader();

            FilingHeadersRepositoryMock.Setup(x => x.Save()).Throws(new Exception());

            Domain.Common.OperationResult.OperationResultWithValue<int> result = _filingWorkflow.StartUnitTradeFiling(header);

            Assert.AreEqual(false, result.IsValid);
        }

        [TestMethod]
        public void CreateUnitTradeFilingHeader_returns_badresult_if_Filing_Procedure_throws_exception()
        {
            var header = new TestFilingHeader {Id = 1};

            FilingHeadersRepositoryMock.Setup(x => x.FillDataForFilingHeader(header.Id, null)).Throws(new Exception());

            Domain.Common.OperationResult.OperationResultWithValue<int> result = _filingWorkflow.StartUnitTradeFiling(header);

            Assert.AreEqual(false, result.IsValid);
        }
    }
}
