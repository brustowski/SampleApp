using System.Collections.Generic;
using FilingPortal.DataLayer.Repositories.VesselImport;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Vessel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.DataLayer.Tests.Repositories.VesselImport
{
    [TestClass]
    public class VesselImportDocumentsRepositoryTests : RepositoryTestBase
    {
        private VesselImportDocumentsRepository _repository;

        protected override void TestInit()
        {
            _repository = new VesselImportDocumentsRepository(UnitOfWorkFactory);
        }

        [TestMethod]
        public void GetListByFilingHeader_WithSpecifiedFilingHeaderId_ReturnsList()
        {
            var filingHeader = new VesselImportFilingHeader { CreatedUser = "test_user" };
            DbContext.VesselImportFilingHeaders.Add(filingHeader);
            DbContext.SaveChanges();
            var doc1 = new VesselImportDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                CreatedUser = "test_user",
                FilingHeaderId = filingHeader.Id
            };
            var doc2 = new VesselImportDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                CreatedUser = "test_user",
                FilingHeaderId = filingHeader.Id
            };
            DbContext.VesselImportDocuments.Add(doc1);
            DbContext.VesselImportDocuments.Add(doc2);
            DbContext.SaveChanges();

            System.Collections.Generic.IEnumerable<VesselImportDocument> result = _repository.GetListByFilingHeader(filingHeader.Id, new List<int>());

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public void GetListByFilingHeader_WithDifferentFilingHeaderIds_ReturnsList()
        {
            var filingHeader1 = new VesselImportFilingHeader { CreatedUser = "test_user" };
            var filingHeader2 = new VesselImportFilingHeader { CreatedUser = "test_user" };
            DbContext.VesselImportFilingHeaders.Add(filingHeader1);
            DbContext.VesselImportFilingHeaders.Add(filingHeader2);
            DbContext.SaveChanges();
            var doc1 = new VesselImportDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                CreatedUser = "test_user",
                FilingHeaderId = filingHeader1.Id
            };
            var doc2 = new VesselImportDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                CreatedUser = "test_user",
                FilingHeaderId = filingHeader2.Id
            };
            var doc3 = new VesselImportDocument
            {
                Content = new byte[] { 1, 2, 3 },
                DocumentType = "doc type",
                FileName = "fname",
                Extension = "image/jpeg",
                CreatedUser = "test_user",
                FilingHeaderId = filingHeader1.Id
            };
            DbContext.VesselImportDocuments.Add(doc1);
            DbContext.VesselImportDocuments.Add(doc2);
            DbContext.VesselImportDocuments.Add(doc3);
            DbContext.SaveChanges();

            var result = _repository.GetListByFilingHeader(filingHeader1.Id, new List<int>()).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(x => x.FilingHeaderId == filingHeader1.Id));
        }
    }
}