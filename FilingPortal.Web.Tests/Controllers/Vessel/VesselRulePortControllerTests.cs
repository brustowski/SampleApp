using System.Linq;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Vessel;
using FilingPortal.Web.Models;
using FilingPortal.Web.PageConfigs.Vessel;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Vessel
{
    [TestClass]
    public class VesselRulePortControllerTests : ApiControllerFunctionTestsBase<VesselRulePortController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IRuleRepository<VesselRulePort>> _repositoryMock;
        private Mock<IRuleValidator<VesselRulePort>> _ruleValidatorMock;
        private Mock<IRuleService<VesselRulePort>> _ruleServiceMock;

        protected override VesselRulePortController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new VesselRuleActionsConfig());
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _repositoryMock = new Mock<IRuleRepository<VesselRulePort>>();

            _ruleValidatorMock = new Mock<IRuleValidator<VesselRulePort>>();
            _ruleValidatorMock.Setup(x => x.IsDuplicate(It.IsAny<VesselRulePort>())).Returns(false);

            _ruleServiceMock = new Mock<IRuleService<VesselRulePort>>();

            return new VesselRulePortController(
                _pageConfigContainerMock.Object,
                _searchRequestFactoryMock.Object, _repositoryMock.Object, _ruleServiceMock.Object, _ruleValidatorMock.Object)
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
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<VesselRulePort>(x => x.FirmsCodeId), model.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(0).Message);
        }
    }
}