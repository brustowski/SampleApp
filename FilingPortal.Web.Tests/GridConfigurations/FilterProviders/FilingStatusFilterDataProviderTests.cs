using FilingPortal.Domain.Entities;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.GridConfigurations.FilterProviders;
using Framework.Domain.Paging;
using Framework.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace FilingPortal.Web.Tests.GridConfigurations.FilterProviders
{
    [TestClass]
    public class FilingStatusFilterDataProviderTests
    {
        private FilingStatusFilterDataProvider _provider;
        private Mock<ISearchRepository<HeaderFilingStatus>> _repositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<ISearchRepository<HeaderFilingStatus>>();

            _provider = new FilingStatusFilterDataProvider(_repositoryMock.Object);
        }

        [TestMethod]
        public void GetData_CallsRepository()
        {
            var searchInfo = new SearchInfo("search data", 20);
            _repositoryMock.Setup(x => x.GetAll<LookupItem>()).Returns(new List<LookupItem>());

            _provider.GetData(searchInfo);

            _repositoryMock.Verify(x => x.GetAll<LookupItem>(), Times.Once);
        }
    }
}
