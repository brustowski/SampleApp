using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories.Truck;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Truck;
using FilingPortal.Web.PageConfigs.Configuration;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Truck
{
    [TestClass]
    public class TruckDefaultValuesControllerTests : ApiControllerFunctionTestsBase<TruckDefaultValuesController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<ITruckDefValuesReadModelRepository> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IDefValueService<TruckDefValue>> _defValueServiceMock;

        protected override TruckDefaultValuesController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new DefValuesActionsConfig());
            _repositoryMock = new Mock<ITruckDefValuesReadModelRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _defValueServiceMock = new Mock<IDefValueService<TruckDefValue>>();

            return new TruckDefaultValuesController(
                _pageConfigContainerMock.Object,
                _repositoryMock.Object, _searchRequestFactoryMock.Object, _defValueServiceMock.Object);
        }

        [TestMethod]
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/rules/truck/default-values/gettotalmatches", x => x.GetTotalMatches(null));
            AssertRoute(HttpMethod.Post, "/api/rules/truck/default-values/search", x => x.Search(null));
            AssertRoute(HttpMethod.Post, "/api/rules/truck/default-values/create", x => x.Create(null));
            AssertRoute(HttpMethod.Get, "/api/rules/truck/default-values/getNew", x => x.GetNewRow());
            AssertRoute(HttpMethod.Post, "/api/rules/truck/default-values/update", x => x.Update(null));
            AssertRoute(HttpMethod.Post, "/api/rules/truck/default-values/delete/1", x => x.Delete(1));
        }

        [TestMethod]
        public void GetNewRow_ReturnsEmptyModel()
        {
            DefValuesViewModel result = Controller.GetNewRow();

            Assert.IsTrue(string.IsNullOrEmpty(result.DefaultValue));
            Assert.IsTrue(string.IsNullOrEmpty(result.UISection));
            Assert.IsTrue(string.IsNullOrEmpty(result.ValueDesc));
            Assert.IsTrue(string.IsNullOrEmpty(result.ValueLabel));
            Assert.IsTrue(result.DisplayOnUI == null);
            Assert.IsFalse(result.Editable);
            Assert.IsFalse(result.HasDefaultValue);
            Assert.IsFalse(result.Mandatory);
            Assert.IsTrue(result.Manual == 0);
            Assert.AreEqual(0, result.Id);
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            int totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<DefValuesViewModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<DefValuesViewModel>(searchRequest))
                .ReturnsAsync(totalMatched);

            int result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();

            _searchRequestFactoryMock.Setup(x => x.Create<DefValuesViewModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllAsync<DefValuesViewModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<DefValuesViewModel> { CurrentPage = 1, Results = new List<DefValuesViewModel>() });

            SimplePagedResult<DefValuesViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }

        [TestMethod]
        public void Delete_WhenCalled_CallsService()
        {
            int ruleId = 45;
            _defValueServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(new OperationResult());

            Controller.Delete(ruleId);

            _defValueServiceMock.Verify(x => x.Delete(ruleId), Times.Once);
        }

        [TestMethod]
        public void Delete_WhenSuccess_ResultWithoutErrors()
        {
            int ruleId = 45;
            _defValueServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(new OperationResult());

            ValidationResultWithFieldsErrorsViewModel result = Controller.Delete(ruleId);

            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(string.IsNullOrEmpty(result.CommonError));
        }

        [TestMethod]
        public void Delete_WhenFailed_ResultWithCommonError()
        {
            int ruleId = 45;
            var errorResult = new OperationResult();
            errorResult.AddErrorMessage("any error");
            _defValueServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(errorResult);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Delete(ruleId);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("any error", result.CommonError);
        }

        [TestMethod]
        public void Create_ModelStateError_ReturnsInValidResult()
        {
            var model = new DefValuesEditModel();
            Controller.ModelState.AddModelError("fname", "Error");

            ValidationResultWithFieldsErrorsViewModel result = Controller.Create(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.FieldsErrors.Count);
            Assert.AreEqual("fname", result.FieldsErrors.First().FieldName);
            Assert.AreEqual("Error", result.FieldsErrors.First().Message);
        }

        [TestMethod]
        public void Create_ModelStateOk_ReturnsValidResult()
        {
            var model = new DefValuesEditModel();

            ValidationResultWithFieldsErrorsViewModel result = Controller.Create(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.FieldsErrors.Count);
        }

        [TestMethod]
        public void Create_CallsService_WhenCalled()
        {
            var model = new DefValuesEditModel
            {
                DefaultValue = "test",
                Editable = true,
                DisplayOnUI = "6",
                HasDefaultValue = true,
                Id = 0,
                Mandatory = true,
                Manual = "56",
                UISection = "01. Description",
                TableName = "table_name",
                ColumnName = "column_name",
                ValueDesc = "value description",
                ValueLabel = "value label"
            };

            Controller.Create(model);

            _defValueServiceMock.Verify(x => x.Create(It.Is<TruckDefValue>(r =>
                r.Id == 0 &&
                r.Editable &&
                r.DisplayOnUI == 6 &&
                r.HasDefaultValue &&
                r.Mandatory &&
                r.Manual == 56 &&
                r.ColumnName == "column_name" &&
                r.Description == "value description" &&
                r.Label == "value label"
            ), "table_name", "test"), Times.Once);
        }

        [TestMethod]
        public void Update_ModelStateError_ReturnsInValidResult()
        {
            var model = new DefValuesEditModel();
            Controller.ModelState.AddModelError("fname", "Error");

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.FieldsErrors.Count);
            Assert.AreEqual("fname", result.FieldsErrors.First().FieldName);
            Assert.AreEqual("Error", result.FieldsErrors.First().Message);
        }

        [TestMethod]
        public void Update_ModelStateOk_ReturnsValidResult()
        {
            var model = new DefValuesEditModel();

            _defValueServiceMock.Setup(x => x.Update(It.Is<TruckDefValue>(r => r.Id == model.Id), It.IsAny<string>(), null)).Returns(new OperationResult());

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.FieldsErrors.Count);
        }

        [TestMethod]
        public void Update_CallsService_WhenCalled()
        {
            var model = new DefValuesEditModel
            {
                Id = 67,
                DefaultValue = "test",
                Editable = true,
                DisplayOnUI = "6",
                HasDefaultValue = true,
                Mandatory = true,
                Manual = "56",
                TableName = "table_name",
                ColumnName = "column_name",
                UISection = "01. Description",
                ValueDesc = "value description",
                ValueLabel = "value label"
            };

            _defValueServiceMock.Setup(x => x.Update(It.IsAny<TruckDefValue>(), It.IsAny<string>(), "test")).Returns(new OperationResult());

            Controller.Update(model);

            _defValueServiceMock.Verify(x => x.Update(It.Is<TruckDefValue>(
                r => r.Id == model.Id &&
                r.DisplayOnUI == 6 &&
                r.Manual == 56 &&
                r.Editable &&
                r.HasDefaultValue &&
                r.Mandatory &&
                r.ColumnName == "column_name" &&
                r.Description == "value description" &&
                r.Label == "value label"), "table_name", "test"), Times.Once);
        }

        [TestMethod]
        public void Update_ServiceReturnResultWithErrors_ReturnsInValidResult()
        {
            var model = new DefValuesEditModel { Id = 67 };

            var or = new OperationResult();
            or.AddErrorMessage("error");

            _defValueServiceMock.Setup(x => x.Update(It.Is<TruckDefValue>(r => r.Id == model.Id), It.IsAny<string>(), null)).Returns(or);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("error", result.CommonError);
        }
    }
}