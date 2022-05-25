using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Pipeline;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Pipeline;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Pipeline
{
    [TestClass]
    public class PipelineRuleBatchCodeControllerTests : ApiControllerFunctionTestsBase<PipelineRuleBatchCodeController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IRuleRepository<PipelineRuleBatchCode>> _repositoryMock;
        private Mock<IRuleValidator<PipelineRuleBatchCode>> _ruleValidatorMock;
        private Mock<IRuleService<PipelineRuleBatchCode>> _ruleServiceMock;

        protected override PipelineRuleBatchCodeController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new PipelineRuleActionsConfig());
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _repositoryMock = new Mock<IRuleRepository<PipelineRuleBatchCode>>();
            _ruleValidatorMock = new Mock<IRuleValidator<PipelineRuleBatchCode>>();
            _ruleServiceMock = new Mock<IRuleService<PipelineRuleBatchCode>>();

            var controller = new PipelineRuleBatchCodeController(
                _pageConfigContainerMock.Object,
                _searchRequestFactoryMock.Object,
                _repositoryMock.Object,
                _ruleServiceMock.Object,
                _ruleValidatorMock.Object)
            {
                CurrentUser = new AppUsersModel { Id = "testUser" }
            };

            return controller;
        }

        [TestMethod]
        public void AddRuleExistingError_AddAppropriateError()
        {
            var model = new ValidationResultWithFieldsErrorsViewModel();

            Controller.AddRuleExistingError(model);

            Assert.IsFalse(model.IsValid);
            Assert.AreEqual(1, model.FieldsErrors.Count);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<PipelineRuleBatchCode>(x => x.BatchCode), model.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(0).Message);
        }
    }
}