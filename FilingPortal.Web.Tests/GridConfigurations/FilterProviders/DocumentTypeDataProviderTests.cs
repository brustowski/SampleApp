using FilingPortal.Domain.DocumentTypes;
using FilingPortal.Domain.DocumentTypes.Entities;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using FilingPortal.Web.Models.DocumentTypeModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Web.Tests.GridConfigurations.FilterProviders
{
    [TestClass]
    public class DocumentTypeDataProviderTests
    {
        private DocumentTypeDataProvider _provider;
        private Mock<IDocumentTypesRepository> _documentTypesRepositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _documentTypesRepositoryMock = new Mock<IDocumentTypesRepository>();

            _provider = new DocumentTypeDataProvider(_documentTypesRepositoryMock.Object);
        }

        [TestMethod]
        public void GetData_CallsRepository()
        {
            var searchInfo = new SearchInfo("search data", 20);
            _documentTypesRepositoryMock.Setup(x => x.GetFilteredData("search data", 20)).Returns(new List<DocumentType>());

            _provider.GetData(searchInfo);

            _documentTypesRepositoryMock.Verify(x => x.GetFilteredData("search data", 20), Times.Once);
        }

        [TestMethod]
        public void GetData_ReturnsValuesWithDescription()
        {
            var searchInfo = new SearchInfo("search data", 20);
            var docType1 = new DocumentType { Id = 23, TypeName = "type 1", Description = "desc 1" };
            var docType2 = new DocumentType { Id = 98, TypeName = "type 2", Description = "desc 2" };
            _documentTypesRepositoryMock.Setup(x => x.GetFilteredData("search data", 20)).Returns(new List<DocumentType> { docType1, docType2 });

            var result = _provider.GetData(searchInfo).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ElementAt(0) is DocumentTypeFilterItem);
            Assert.AreEqual("type 1", ((DocumentTypeFilterItem)result.ElementAt(0)).Value);
            Assert.AreEqual("type 1", ((DocumentTypeFilterItem)result.ElementAt(0)).DisplayValue);
            Assert.AreEqual("desc 1", ((DocumentTypeFilterItem)result.ElementAt(0)).Description);
        }
    }
}
