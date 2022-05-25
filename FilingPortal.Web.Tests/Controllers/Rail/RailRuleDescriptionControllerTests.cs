using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Web.Tests.Controllers.Rail
{
    [TestClass]
    public class RailRuleDescriptionControllerTests : ApiControllerFunctionTestsBase<RailRuleDescriptionController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IRuleRepository<RailRuleDescription>> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IRuleService<RailRuleDescription>> _ruleServiceMock;
        private Mock<IRuleValidator<RailRuleDescription>> _ruleValidatorMock;

        protected override RailRuleDescriptionController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new RailRuleActionsConfig());
            _repositoryMock = new Mock<IRuleRepository<RailRuleDescription>>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _ruleServiceMock = new Mock<IRuleService<RailRuleDescription>>();
            _ruleValidatorMock = new Mock<IRuleValidator<RailRuleDescription>>();

            return new RailRuleDescriptionController(
                _pageConfigContainerMock.Object,
                _searchRequestFactoryMock.Object,
                _repositoryMock.Object,
                _ruleServiceMock.Object,
                _ruleValidatorMock.Object)
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
            Assert.AreEqual(5, model.FieldsErrors.Count);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleDescription>(x => x.Description1), model.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleDescription>(x => x.Importer), model.FieldsErrors.ElementAt(1).FieldName);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleDescription>(x => x.Supplier), model.FieldsErrors.ElementAt(2).FieldName);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleDescription>(x => x.Port), model.FieldsErrors.ElementAt(3).FieldName);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleDescription>(x => x.Destination), model.FieldsErrors.ElementAt(4).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(0).Message);
        }
    }
}