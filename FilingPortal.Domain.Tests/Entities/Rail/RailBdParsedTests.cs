using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Entities.Rail
{
    [TestClass]
    public class RailBdParsedTests
    {
        private RailBdParsed _entity;

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = new RailBdParsed();
        }

        [TestMethod]
        public void CanBeDeleted_WithDefaultValues_True()
        {
            bool result = _entity.CanBeDeleted();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanBeDeleted_WithIsDeletedFlag_False()
        {
            _entity.SetDeleted();

            bool result = _entity.CanBeDeleted();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeDeleted_WithMappingStatusAny_False()
        {
            _entity.FilingHeaders.Add(new RailFilingHeader { MappingStatus = MappingStatus.Open });

            bool result = _entity.CanBeDeleted();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeDeleted_WithFilingStatusAny_False()
        {
            _entity.FilingHeaders.Add(new RailFilingHeader { FilingStatus = FilingStatus.Open });

            bool result = _entity.CanBeDeleted();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanBeFiled_WithDefaultValues_True()
        {
            bool result = _entity.CanBeFiled();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanBeFiled_WithIsDeletedFlag_False()
        {
            _entity.SetDeleted();

            bool result = _entity.CanBeFiled();

            Assert.IsFalse(result);
        }
    }
}
