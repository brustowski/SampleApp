using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.OperationResult;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Controllers;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Vessel;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public class RuleControllerBaseTests : ApiControllerFunctionTestsBase<RuleControllerBase<FakeRuleEntity, FakeRuleEntityViewModel, FakeRuleEntityEditModel>>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IRuleRepository<FakeRuleEntity>> _repositoryMock;
        private Mock<IRuleValidator<FakeRuleEntity>> _ruleValidatorMock;
        private Mock<IRuleService<FakeRuleEntity>> _ruleServiceMock;

        protected override RuleControllerBase<FakeRuleEntity, FakeRuleEntityViewModel, FakeRuleEntityEditModel> TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new VesselRuleActionsConfig());
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _repositoryMock = new Mock<IRuleRepository<FakeRuleEntity>>();

            _ruleValidatorMock = new Mock<IRuleValidator<FakeRuleEntity>>();
            _ruleValidatorMock.Setup(x => x.IsDuplicate(It.IsAny<FakeRuleEntity>())).Returns(false);

            _ruleServiceMock = new Mock<IRuleService<FakeRuleEntity>>();

            var controller = new Mock<RuleControllerBase<FakeRuleEntity, FakeRuleEntityViewModel, FakeRuleEntityEditModel>>(_pageConfigContainerMock.Object,
                _searchRequestFactoryMock.Object, _repositoryMock.Object, _ruleServiceMock.Object, _ruleValidatorMock.Object)
            {
                CallBase = true
            };
            controller.Object.CurrentUser = new AppUsersModel { Id = "testUser" };
            controller.Setup(x => x.PageConfigurationName).Returns("testPageName");
            controller.Setup(x => x.AddRuleExistingError(It.IsAny<ValidationResultWithFieldsErrorsViewModel>()))
                .Callback<ValidationResultWithFieldsErrorsViewModel>(r => { r.AddFieldError("Name", string.Empty); });
            return controller.Object;
        }

        [TestMethod]
        public void GetNewRow_ReturnsEmptyModel()
        {
            FakeRuleEntityViewModel result = Controller.GetNewRow();

            Assert.IsTrue(string.IsNullOrEmpty(result.Name));
            Assert.AreEqual(0, result.Id);
        }

        [TestMethod]
        public async Task GetTotalMatches_Returns()
        {
            var totalMatched = 234;
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();
            _searchRequestFactoryMock.Setup(x => x.Create<FakeRuleEntityViewModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetTotalMatchesAsync<FakeRuleEntityViewModel>(searchRequest))
                .ReturnsAsync(totalMatched);

            var result = await Controller.GetTotalMatches(searchRequestModel);

            Assert.AreEqual(totalMatched, result);
        }

        [TestMethod]
        public async Task Search_Returns()
        {
            var searchRequestModel = new SearchRequestModel();
            var searchRequest = new SearchRequest();

            _searchRequestFactoryMock.Setup(x => x.Create<FakeRuleEntityViewModel>(searchRequestModel))
                .Returns(searchRequest);

            _repositoryMock.Setup(x => x.GetAllAsync<FakeRuleEntityViewModel>(searchRequest))
                .ReturnsAsync(
                    new SimplePagedResult<FakeRuleEntityViewModel>() { CurrentPage = 1, Results = new List<FakeRuleEntityViewModel>() });

            SimplePagedResult<FakeRuleEntityViewModel> result = await Controller.Search(searchRequestModel);

            Assert.AreEqual(1, result.CurrentPage);
        }

        [TestMethod]
        public void Update_ValidModelState_ReturnsValidResult()
        {
            var model = new FakeRuleEntityEditModel();

            _ruleServiceMock.Setup(x => x.Update(It.Is<FakeRuleEntity>(r => r.Id == model.Id))).Returns(new OperationResult());

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.FieldsErrors.Count);
        }

        [TestMethod]
        public void Update_ModelStateError_ReturnsInvalidResult()
        {
            var model = new FakeRuleEntityEditModel();
            Controller.ModelState.AddModelError("fname", "Error");

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.FieldsErrors.Count);
            Assert.AreEqual("fname", result.FieldsErrors.First().FieldName);
            Assert.AreEqual("Error", result.FieldsErrors.First().Message);
        }

        [TestMethod]
        public void Update_DuplicatesValidatorIsCalled_WhenCalled()
        {
            var model = new FakeRuleEntityEditModel { Id = 237, Name = "Name" };

            _ruleServiceMock.Setup(x => x.Update(It.Is<FakeRuleEntity>(r => r.Id == model.Id))).Returns(new OperationResult());

            Controller.Update(model);

            _ruleValidatorMock.Verify(x => x.IsDuplicate(It.Is<FakeRuleEntity>(r => r.Id == 237 && r.Name == "Name")));
        }

        [TestMethod]
        public void Update_DuplicatesValidatorError_ReturnsNotValidResult()
        {
            var model = new FakeRuleEntityEditModel();
            _ruleValidatorMock.Setup(x => x.IsDuplicate(It.IsAny<FakeRuleEntity>())).Returns(true);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Rule already exists for such values", result.CommonError);
            Assert.AreEqual(1, result.FieldsErrors.Count);
            Assert.AreEqual("Name", result.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(string.Empty, result.FieldsErrors.ElementAt(0).Message);
        }

        [TestMethod]
        public void Update_CallsService_WhenCalled()
        {
            var model = new FakeRuleEntityEditModel();


            _ruleServiceMock.Setup(x => x.Update(It.Is<FakeRuleEntity>(r => r.Id == model.Id))).Returns(new OperationResult());

            Controller.Update(model);

            _ruleServiceMock.Verify(x => x.Update(It.Is<FakeRuleEntity>(r => r.Id == model.Id)), Times.Once);
        }

        [TestMethod]
        public void Update_ServiceReturnResultWithErrors_ReturnsInvalidResult()
        {
            var model = new FakeRuleEntityEditModel();


            var or = new OperationResult();
            or.AddErrorMessage("error");

            _ruleServiceMock.Setup(x => x.Update(It.Is<FakeRuleEntity>(r => r.Id == model.Id))).Returns(or);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Update(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("error", result.CommonError);
        }

        [TestMethod]
        public void Create_ModelStateError_ReturnsInValidResult()
        {
            var model = new FakeRuleEntityEditModel();

            Controller.ModelState.AddModelError("fname", "Error");

            ValidationResultWithFieldsErrorsViewModel result = Controller.Create(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.FieldsErrors.Count);
            Assert.AreEqual("fname", result.FieldsErrors.First().FieldName);
            Assert.AreEqual("Error", result.FieldsErrors.First().Message);
        }

        [TestMethod]
        public void Create_ValidModel_ReturnsValidResult()
        {
            var model = new FakeRuleEntityEditModel();


            ValidationResultWithFieldsErrorsViewModel result = Controller.Create(model);

            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.FieldsErrors.Count);
        }

        [TestMethod]
        public void Create_DuplicatesValidatorIsCalled_WhenCalled()
        {
            var model = new FakeRuleEntityEditModel { Id = 237, Name = "Name" };


            Controller.Create(model);

            _ruleValidatorMock.Verify(x => x.IsDuplicate(It.Is<FakeRuleEntity>(r => r.Id == 237 && r.Name == "Name")));
        }

        [TestMethod]
        public void Create_DuplicatesValidatorError_ReturnsNotValidResult()
        {
            var model = new FakeRuleEntityEditModel();
            _ruleValidatorMock.Setup(x => x.IsDuplicate(It.IsAny<FakeRuleEntity>())).Returns(true);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Create(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Rule already exists for such values", result.CommonError);
            Assert.AreEqual(1, result.FieldsErrors.Count);
            Assert.AreEqual("Name", result.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(string.Empty, result.FieldsErrors.ElementAt(0).Message);
        }

        [TestMethod]
        public void Create_CallsService_WhenCalled()
        {
            var model = new FakeRuleEntityEditModel();


            Controller.Create(model);

            _ruleServiceMock.Verify(x => x.Create(It.Is<FakeRuleEntity>(r => r.Id == model.Id)), Times.Once);
        }

        [TestMethod]
        public void Delete_CallsService_WhenCalled()
        {
            var id = 2;

            _ruleServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(new OperationResult());

            Controller.Delete(id);

            _ruleServiceMock.Verify(x => x.Delete(id), Times.Once);
        }

        [TestMethod]
        public void Delete_ServiceReturnResultWithErrors_ReturnsInvalidResult()
        {
            var id = 2;

            var or = new OperationResult();
            or.AddErrorMessage("error");

            _ruleServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(or);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Delete(id);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("error", result.CommonError);
        }

        [TestMethod]
        public void Delete_ServiceReturnResultWithErrors_ReturnsValidResult()
        {
            var id = 2;

            var or = new OperationResult();

            _ruleServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(or);

            ValidationResultWithFieldsErrorsViewModel result = Controller.Delete(id);

            Assert.IsTrue(result.IsValid);
        }
    }
}
