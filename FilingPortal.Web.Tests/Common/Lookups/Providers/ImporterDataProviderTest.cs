using FilingPortal.Domain.Entities.Clients;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.Common.Lookups.Providers;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Web.Tests.Common.Lookups.Providers
{
    [TestClass]
    public class ImporterDataProviderTest
    {
        private ImporterDataProvider _provider;
        private Mock<IClientRepository> _repositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<IClientRepository>();
            _repositoryMock.Setup(x => x.GetImporters(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<ClientReadModel>
                {
                    new ClientReadModel { Id = Guid.NewGuid(), ClientName = "first importer"},
                    new ClientReadModel { Id = Guid.NewGuid(), ClientName = "second importer"},
                    new ClientReadModel { Id = Guid.NewGuid(), ClientName = "third importer"}
                });
            _provider = new ImporterDataProvider(_repositoryMock.Object);
        }

        [TestMethod]
        public void Name_WhenCalled_ReturnsCorrectProviderName()
        {
            Assert.AreEqual("Importers", _provider.Name);
        }

        [TestMethod]
        public void GetData_CallsRepository_WhenCalled()
        {
            var searchInfo = new SearchInfo("first", 10);

            _provider.GetData(searchInfo);

            _repositoryMock.Verify(x => x.GetImporters("first", 10), Times.Once);
        }

        [TestMethod]
        public void GetData_Returns_ValidLookupItem()
        {
            var searchInfo = new SearchInfo(string.Empty, 0);

            LookupItem result = _provider.GetData(searchInfo).First();
            Assert.AreEqual("first importer", result.DisplayValue);
            Assert.IsInstanceOfType(result.Value, typeof(Guid));
        }
    }
}
