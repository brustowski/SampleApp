using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Entities
{
    [TestClass]
    public class BaseFilingHeaderTests
    {
        private FilingHeaderOld _filingHeader;

        [TestInitialize]
        public void TestInitialize()
        {
            _filingHeader = new Mock<FilingHeaderOld>().Object;
        }

        [DataRow(null, null, true)]
        [DataRow(MappingStatus.Open, null, false)]
        [DataRow(MappingStatus.InReview, null, true)]
        [DataRow(MappingStatus.InProgress, null, false)]
        [DataRow(MappingStatus.Mapped, null, false)]
        [DataRow(MappingStatus.Error, null, true)]
        [DataRow(MappingStatus.Updated, null, true)]
        [DataRow(null, FilingStatus.Open, true)]
        [DataRow(MappingStatus.Open, FilingStatus.Open, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.Open, true)]
        [DataRow(MappingStatus.InProgress, FilingStatus.Open, false)]
        [DataRow(MappingStatus.Mapped, FilingStatus.Open, false)]
        [DataRow(MappingStatus.Error, FilingStatus.Open, true)]
        [DataRow(MappingStatus.Updated, FilingStatus.Open, true)]
        [DataRow(null, FilingStatus.Error, true)]
        [DataRow(MappingStatus.Open, FilingStatus.Error, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.Error, true)]
        [DataRow(MappingStatus.InProgress, FilingStatus.Error, false)]
        [DataRow(MappingStatus.Mapped, FilingStatus.Error, false)]
        [DataRow(MappingStatus.Error, FilingStatus.Error, true)]
        [DataRow(MappingStatus.Updated, FilingStatus.Error, true)]
        [DataRow(null, FilingStatus.InProgress, true)]
        [DataRow(MappingStatus.Open, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.InProgress, true)]
        [DataRow(MappingStatus.InProgress, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.Mapped, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.Error, FilingStatus.InProgress, true)]
        [DataRow(MappingStatus.Updated, FilingStatus.InProgress, true)]
        [DataRow(null, FilingStatus.Filed, true)]
        [DataRow(MappingStatus.Open, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.Filed, true)]
        [DataRow(MappingStatus.InProgress, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.Mapped, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.Error, FilingStatus.Filed, true)]
        [DataRow(MappingStatus.Updated, FilingStatus.Filed, true)]
        [DataTestMethod]
        public void CanBeEdited_ReturnsValidValue_ForDifferentMappingStatuses(MappingStatus? mappingStatus, FilingStatus? filingStatus, bool expectedResult)
        {
            _filingHeader.MappingStatus = mappingStatus;
            _filingHeader.FilingStatus = filingStatus;

            bool result = _filingHeader.CanBeEdited;

            Assert.AreEqual(expectedResult, result);
        }

        [DataRow(null, null, true)]
        [DataRow(MappingStatus.Open, null, false)]
        [DataRow(MappingStatus.InReview, null, true)]
        [DataRow(MappingStatus.InProgress, null, false)]
        [DataRow(MappingStatus.Mapped, null, false)]
        [DataRow(MappingStatus.Error, null, true)]
        [DataRow(MappingStatus.Updated, null, true)]
        [DataRow(null, FilingStatus.Open, true)]
        [DataRow(MappingStatus.Open, FilingStatus.Open, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.Open, true)]
        [DataRow(MappingStatus.InProgress, FilingStatus.Open, false)]
        [DataRow(MappingStatus.Mapped, FilingStatus.Open, false)]
        [DataRow(MappingStatus.Error, FilingStatus.Open, true)]
        [DataRow(MappingStatus.Updated, FilingStatus.Open, true)]
        [DataRow(null, FilingStatus.Error, true)]
        [DataRow(MappingStatus.Open, FilingStatus.Error, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.Error, true)]
        [DataRow(MappingStatus.InProgress, FilingStatus.Error, false)]
        [DataRow(MappingStatus.Mapped, FilingStatus.Error, false)]
        [DataRow(MappingStatus.Error, FilingStatus.Error, true)]
        [DataRow(MappingStatus.Updated, FilingStatus.Error, true)]
        [DataRow(null, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.Open, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.InProgress, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.Mapped, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.Error, FilingStatus.InProgress, false)]
        [DataRow(MappingStatus.Updated, FilingStatus.InProgress, false)]
        [DataRow(null, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.Open, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.InReview, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.InProgress, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.Mapped, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.Error, FilingStatus.Filed, false)]
        [DataRow(MappingStatus.Updated, FilingStatus.Filed, false)]
        [DataTestMethod]
        public void CanBeCanceled_ReturnsValidValue_ForDifferentMappingStatuses(MappingStatus? mappingStatus, FilingStatus? filingStatus, bool expectedResult)
        {
            _filingHeader.MappingStatus = mappingStatus;
            _filingHeader.FilingStatus = filingStatus;

            bool result = _filingHeader.CanBeCanceled;

            Assert.AreEqual(expectedResult, result);
        }

    }
}
