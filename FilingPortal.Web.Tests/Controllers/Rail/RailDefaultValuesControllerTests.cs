using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Domain.Services;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.PageConfigs.Configuration;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Rail
{
    [TestClass]
    public class RailDefaultValuesControllerTests : ApiControllerFunctionTestsBase<RailDefaultValuesController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IRailDefValuesReadModelRepository> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IDefValueService<RailDefValues>> _defValueService;

        protected override RailDefaultValuesController TestInitialize()
        {

            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new DefValuesActionsConfig());
            _repositoryMock = new Mock<IRailDefValuesReadModelRepository>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _defValueService = new Mock<IDefValueService<RailDefValues>>();

            return new RailDefaultValuesController(
                _pageConfigContainerMock.Object,
                _repositoryMock.Object,
                _searchRequestFactoryMock.Object,
                _defValueService.Object);
        }

        [TestMethod]
        public void GetNewRow_ReturnsEmptyModel()
        {
            DefValuesViewModel result = Controller.GetNewRow();

            Assert.IsTrue(string.IsNullOrEmpty(result.DefaultValue));
            Assert.IsTrue(string.IsNullOrEmpty(result.ColumnName));
            Assert.IsTrue(string.IsNullOrEmpty(result.TableName));
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
            _defValueService.Setup(x => x.Delete(It.IsAny<int>())).Returns(new OperationResult());

            Controller.Delete(ruleId);

            _defValueService.Verify(x => x.Delete(ruleId), Times.Once);
        }

        [TestMethod]
        public void Delete_WhenSuccess_ResultWithoutErrors()
        {
            int ruleId = 45;
            _defValueService.Setup(x => x.Delete(It.IsAny<int>())).Returns(new OperationResult());

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
            _defValueService.Setup(x => x.Delete(It.IsAny<int>())).Returns(errorResult);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Delete(ruleId);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("any error", result.CommonError);
        }

        [TestMethod]
        public void Create_ModelStateError_ReturnsInValidResult()
        {
            var model = new RailDefValuesEditModel();
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
            var model = new RailDefValuesEditModel();

            ValidationResultWithFieldsErrorsViewModel result = Controller.Create(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.FieldsErrors.Count);
        }

        [TestMethod]
        public void Create_CallsService_WhenCalled()
        {
            var model = new RailDefValuesEditModel
            {
                DefaultValue = "test",
                Editable = true,
                ColumnName = "col name",
                DisplayOnUI = "6",
                HasDefaultValue = true,
                Id = 0,
                Mandatory = true,
                Manual = "56",
                TableName = "table",
                UISection = "01. Description",
                ValueDesc = "value descr",
                ValueLabel = "value label"
            };

            Controller.Create(model);

            _defValueService.Verify(x => x.Create(It.Is<RailDefValues>(r =>
                r.Editable == true &&
                r.ColumnName == "col name" &&
                r.DisplayOnUI == 6 &&
                r.HasDefaultValue == true &&
                r.Mandatory == true &&
                r.Manual == 56 &&
                r.Description == "value descr" &&
                r.Label == "value label"
            ), "table", "test"), Times.Once);
        }

        [TestMethod]
        public void Update_ModelStateError_ReturnsInValidResult()
        {
            var model = new RailDefValuesEditModel();
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
            var model = new RailDefValuesEditModel
            {
                Id = 7,
                TableName = "table"
            };

            _defValueService.Setup(x => x.Update(It.Is<RailDefValues>(r => r.Id == model.Id), "table", null)).Returns(new OperationResult());

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.FieldsErrors.Count);
        }

        [TestMethod]
        public void Update_CallsService_WhenCalled()
        {
            var model = new RailDefValuesEditModel
            {
                Id = 67,
                DefaultValue = "test",
                Editable = true,
                ColumnName = "col name",
                DisplayOnUI = "6",
                HasDefaultValue = true,
                Mandatory = true,
                Manual = "56",
                TableName = "table",
                UISection = "01. Description",
                ValueDesc = "value descr",
                ValueLabel = "value label"
            };

            _defValueService.Setup(x => x.Update(It.Is<RailDefValues>(r => r.Id == model.Id), "table", "test")).Returns(new OperationResult());

            Controller.Update(model);

            _defValueService.Verify(x =>
                x.Update(
                    It.Is<RailDefValues>(r => r.Id == model.Id &&
                                              r.Editable == true &&
                                              r.ColumnName == "col name" &&
                                              r.DisplayOnUI == 6 &&
                                              r.HasDefaultValue == true &&
                                              r.Mandatory == true &&
                                              r.Manual == 56 &&
                                              r.Description == "value descr" &&
                                              r.Label == "value label"), "table", "test"), Times.Once);
        }

        [TestMethod]
        public void Update_ServiceReturnResultWithErrors_ReturnsInValidResult()
        {
            var model = new RailDefValuesEditModel { Id = 67 };

            var or = new OperationResult();
            or.AddErrorMessage("error");

            _defValueService.Setup(x => x.Update(It.Is<RailDefValues>(r => r.Id == model.Id), It.IsAny<string>(), null)).Returns(or);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("error", result.CommonError);
        }
    }
}