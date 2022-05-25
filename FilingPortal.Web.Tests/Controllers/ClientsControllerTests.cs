using FilingPortal.Domain.Common;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Clients;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Web.Models.ClientManagement;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class ClientsControllerTests : ApiControllerFunctionTestsBase<ClientsController>
    {
        private Mock<IClientRepository> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;

        protected override ClientsController TestInitialize()
        {
            _repositoryMock = new Mock<IClientRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();

            return new ClientsController(_repositoryMock.Object, _searchRequestFactoryMock.Object);
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/clients/gettotalmatches", x => x.GetTotalMatches(null));
            AssertRoute(HttpMethod.Post, "/api/clients/search", x => x.Search(null));
        }

        [TestMethod]
        public void PermissionAssertion()
        {
            AssertPermissions(Permissions.ViewClients, x => x.GetTotalMatches(null));
            AssertPermissions(Permissions.ViewClients, x => x.Search(null));
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            const int totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<Client>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<Client>(searchRequest))
                .ReturnsAsync(totalMatched);

            var result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();

            _searchRequestFactoryMock.Setup(x => x.Create<Client>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllAsync<Client>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<Client>() { CurrentPage = 1, Results = new List<Client>() });

            SimplePagedResult<ClientViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }
    }
}