using System;
using System.Collections.Generic;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services
{
    public class DefValuesManualTest : BaseDefValuesManual
    {
    }

    [TestClass]
    public abstract class FilingWorkflowTests<TFilingWorkflow>
        where TFilingWorkflow : IFilingWorkflow<TestFilingHeader, DefValuesManualTest>
    {
        protected TFilingWorkflow _filingWorkflow;
        protected Mock<IFilingHeaderRepository<TestFilingHeader>> FilingHeadersRepositoryMock;
        protected Mock<IDefValuesManualRepository<DefValuesManualTest>> _defValuesManualRepositoryMock;

        protected abstract TFilingWorkflow GetFilingWorkflow();

        [TestInitialize]
        public void TestInitialize()
        {
            FilingHeadersRepositoryMock = new Mock<IFilingHeaderRepository<TestFilingHeader>>();
            _defValuesManualRepositoryMock = new Mock<IDefValuesManualRepository<DefValuesManualTest>>();

            _filingWorkflow = GetFilingWorkflow();
        }

        [TestMethod]
        public void File_ToInitFiling_CallsRepository()
        {
            int filingHeaderId = 45;
            var filingHeader = new TestFilingHeader { Id = filingHeaderId };
            FilingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(filingHeader);

            _filingWorkflow.File(filingHeaderId);

            FilingHeadersRepositoryMock.Verify(x => x.FileRecordsWithHeader(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void File_WhenErrorOccured_ThrowsException()
        {
            FilingHeadersRepositoryMock.Setup(x => x.FileRecordsWithHeader(It.IsAny<int>())).Throws<Exception>();

            void Action() => _filingWorkflow.File(567);

            AssertThat.Throws<Exception>(Action);
        }

        [TestMethod]
        public void File_ToFinishFiling_ChangesStatus()
        {
            int filingHeaderId = 45;
            var filingHeader = new TestFilingHeader { Id = filingHeaderId };
            FilingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(filingHeader);

            _filingWorkflow.File(filingHeaderId);

            FilingHeadersRepositoryMock.Verify(x => x.Update(It.Is<TestFilingHeader>(f =>
                f.Id == filingHeaderId &&
                f.MappingStatus == MappingStatus.InProgress)), Times.Once);
            FilingHeadersRepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void SetInReview_CallsRepository()
        {
            int filingHeaderId = 45;
            var filingHeader = new TestFilingHeader { Id = filingHeaderId };
            FilingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(filingHeader);

            _filingWorkflow.SetInReview(filingHeaderId);

            FilingHeadersRepositoryMock.Verify(x => x.Get(filingHeaderId), Times.Once);
        }

        [TestMethod]
        public void SetInReview_CallsRepositoryToUpdate()
        {
            int filingHeaderId = 45;
            var filingHeader = new TestFilingHeader { Id = filingHeaderId };
            FilingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(filingHeader);

            _filingWorkflow.SetInReview(filingHeaderId);

            FilingHeadersRepositoryMock.Verify(x =>
                x.Update(It.Is<TestFilingHeader>(f => f.Id == filingHeaderId
                                                      && f.MappingStatus == MappingStatus.InReview)), Times.Once);
        }

        [TestMethod]
        public void SetInReview_CallsRepositoryToSave()
        {
            int filingHeaderId = 45;
            var filingHeader = new TestFilingHeader { Id = filingHeaderId };
            FilingHeadersRepositoryMock.Setup(x => x.Get(filingHeaderId)).Returns(filingHeader);

            _filingWorkflow.SetInReview(filingHeaderId);

            FilingHeadersRepositoryMock.Verify(x => x.Save(), Times.Once);
        }

        [TestMethod]
        public void CancelFilingProcess_Calls_Repository()
        {
            _filingWorkflow.CancelFilingProcess(44, 22, 76);

            FilingHeadersRepositoryMock.Verify(x => x.CancelFilingProcess(It.IsAny<int>()), Times.Exactly(3));
        }

        [TestMethod]
        public void CreateSingleFilingFilingHeaders_Saves_Headers()
        {
            var headers = new List<TestFilingHeader>()
            {
                new TestFilingHeader {Id = 43},
                new TestFilingHeader {Id = 78}
            };

            _filingWorkflow.StartSingleFiling(headers);

            FilingHeadersRepositoryMock.Verify(x => x.Save(), Times.Once);
        }
    }
}
