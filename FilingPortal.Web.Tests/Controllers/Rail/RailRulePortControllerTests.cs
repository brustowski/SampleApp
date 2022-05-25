using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Rail
{
    [TestClass]
    public class RailRulePortControllerTests : ApiControllerFunctionTestsBase<RailRulePortController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IRuleRepository<RailRulePort>> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IRuleService<RailRulePort>> _ruleServiceMock;
        private Mock<IRuleValidator<RailRulePort>> _ruleValidatorMock;

        protected override RailRulePortController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new RailRuleActionsConfig());
            _repositoryMock = new Mock<IRuleRepository<RailRulePort>>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _ruleServiceMock = new Mock<IRuleService<RailRulePort>>();
            _ruleValidatorMock = new Mock<IRuleValidator<RailRulePort>>();
            _ruleValidatorMock = new Mock<IRuleValidator<RailRulePort>>();
            return new RailRulePortController(
                _pageConfigContainerMock.Object,
                _searchRequestFactoryMock.Object,
                _repositoryMock.Object,
                _ruleServiceMock.Object,
                _ruleValidatorMock.Object
               )
            {
                CurrentUser = new AppUsersModel { Id = "testUser" }
            };
        }

        [TestMethod]
        public void AddRuleExistingError_AddAppropriateError()
        {
            var model = new ValidationResultWithFieldsErrorsViewModel();

            Controller.AddRuleExistingError(model);

            Assert.IsFalse(model.IsValid);
            Assert.AreEqual(1, model.FieldsErrors.Count);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRulePort>(x => x.Port), model.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(0).Message);
        }
    }
}