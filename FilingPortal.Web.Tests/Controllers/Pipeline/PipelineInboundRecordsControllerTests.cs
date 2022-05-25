using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Pipeline;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.Pipeline;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Pipeline;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Commands;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Pipeline
{
    [TestClass]
    public class PipelineInboundRecordsControllerTests : ApiControllerFunctionTestsBase<PipelineInboundRecordsController>
    {
        private Mock<IPipelineInboundReadModelRepository> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<ICommandDispatcher> _commandDispatcherMock;
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IListInboundValidator<PipelineInboundReadModel>> _recordsValidatorMock;
        private Mock<ISingleRecordValidator<PipelineInboundReadModel>> _recordValidatorMock;
        private Mock<IFilingHeaderDocumentUpdateService<PipelineDocumentDto>> _documentUploadService;

        protected override PipelineInboundRecordsController TestInitialize()
        {
            _repositoryMock = new Mock<IPipelineInboundReadModelRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _recordValidatorMock = new Mock<ISingleRecordValidator<PipelineInboundReadModel>>();
            _recordsValidatorMock = new Mock<IListInboundValidator<PipelineInboundReadModel>>();
            _documentUploadService = new Mock<IFilingHeaderDocumentUpdateService<PipelineDocumentDto>>();
            return new PipelineInboundRecordsController(
                _repositoryMock.Object, _searchRequestFactoryMock.Object,
                _commandDispatcherMock.Object, _pageConfigContainerMock.Object,
                _recordsValidatorMock.Object, _recordValidatorMock.Object, _documentUploadService.Object);
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/gettotalmatches", x => x.GetTotalMatches(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/search", x => x.Search(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/delete", x => x.Delete(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/validate-selected-records", x => x.ValidateSelectedRecords(null));
            AssertRoute(HttpMethod.Post, "/api/inbound/pipeline/documents-upload", x => x.ProcessFile(null));
        }

        [TestMethod]
        public void PermissionAssertion()
        {
            AssertPermissions(Permissions.PipelineViewInboundRecord, x => x.GetTotalMatches(null));
            AssertPermissions(Permissions.PipelineViewInboundRecord, x => x.Search(null));
            AssertPermissions(Permissions.PipelineDeleteInboundRecord, x => x.Delete(null));
            AssertPermissions(Permissions.PipelineViewInboundRecord, x => x.ValidateSelectedRecords(null));
            AssertPermissions(Permissions.PipelineFileInboundRecord, x => x.ProcessFile(null));
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<PipelineInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<PipelineInboundReadModel>(searchRequest))
                .ReturnsAsync(totalMatched);

            var result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<PipelineInboundReadModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllOrderedAsync<PipelineInboundReadModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<PipelineInboundReadModel> { CurrentPage = 1, Results = new List<PipelineInboundReadModel>() });

            var result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }
    }
}