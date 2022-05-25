using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailDocumentRepositoryTests : RepositoryTestBase
    {
        private RailDocumentRepository _repository;

        protected override void TestInit()
        {
            _repository = new RailDocumentRepository(UnitOfWorkFactory);
        }

        [TestMethod]
        public void GetListByFilingHeader_WithSpecifiedFilingHeaderId_ReturnsList()
        {
            var filingHeader = new RailFilingHeader();
            DbContext.RailFilingHeaders.Add(filingHeader);
            DbContext.SaveChanges();
            var doc1 = new RailDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                FilingHeaderId = filingHeader.Id
            };
            var doc2 = new RailDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                FilingHeaderId = filingHeader.Id
            };
            DbContext.RailDocuments.Add(doc1);
            DbContext.RailDocuments.Add(doc2);
            DbContext.SaveChanges();

            System.Collections.Generic.IEnumerable<RailDocument> result = _repository.GetListByFilingHeader(filingHeader.Id, new List<int>());

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetListByFilingHeader_WithDifferentFilingHeaderIds_ReturnsList()
        {
            var filingHeader1 = new RailFilingHeader();
            var filingHeader2 = new RailFilingHeader();
            DbContext.RailFilingHeaders.Add(filingHeader1);
            DbContext.RailFilingHeaders.Add(filingHeader2);
            DbContext.SaveChanges();
            var doc1 = new RailDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                FilingHeaderId = filingHeader1.Id
            };
            var doc2 = new RailDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                FilingHeaderId = filingHeader2.Id
            };
            var doc3 = new RailDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                FilingHeaderId = filingHeader1.Id
            };
            DbContext.RailDocuments.Add(doc1);
            DbContext.RailDocuments.Add(doc2);
            DbContext.RailDocuments.Add(doc3);
            DbContext.SaveChanges();

            var result = _repository.GetListByFilingHeader(filingHeader1.Id, new List<int>()).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(x => x.FilingHeaderId == filingHeader1.Id));
        }
    }
}