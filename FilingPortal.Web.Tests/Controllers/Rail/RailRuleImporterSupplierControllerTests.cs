using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.PageConfigs.Rail;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Rail
{
    [TestClass]
    public class RailRuleImporterSupplierControllerTests : ApiControllerFunctionTestsBase<RailRuleImporterSupplierController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<IRuleRepository<RailRuleImporterSupplier>> _repositoryMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IRuleValidator<RailRuleImporterSupplier>> _ruleImporterSupplierValidatorMock;
        private Mock<IRuleService<RailRuleImporterSupplier>> _ruleServiceMock;

        protected override RailRuleImporterSupplierController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new RailRuleActionsConfig());
            _repositoryMock = new Mock<IRuleRepository<RailRuleImporterSupplier>>();
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _ruleImporterSupplierValidatorMock = new Mock<IRuleValidator<RailRuleImporterSupplier>>();
            _ruleServiceMock = new Mock<IRuleService<RailRuleImporterSupplier>>();

            return new RailRuleImporterSupplierController(
                _pageConfigContainerMock.Object,
                _searchRequestFactoryMock.Object,
                _repositoryMock.Object,
                _ruleImporterSupplierValidatorMock.Object,
                _ruleServiceMock.Object)
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
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleImporterSupplier>(x => x.ImporterName), model.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(0).Message);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleImporterSupplier>(x => x.SupplierName), model.FieldsErrors.ElementAt(1).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(1).Message);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleImporterSupplier>(x => x.ProductDescription), model.FieldsErrors.ElementAt(2).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(2).Message);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleImporterSupplier>(x => x.Port), model.FieldsErrors.ElementAt(3).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(3).Message);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<RailRuleImporterSupplier>(x => x.Destination), model.FieldsErrors.ElementAt(4).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(4).Message);
        }
    }
}