using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.Truck;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.Truck;
using FilingPortal.Web.PageConfigs.Truck;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Paging;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.Truck
{
    [TestClass]
    public class TruckRuleImporterControllerTests : ApiControllerFunctionTestsBase<TruckRuleImporterController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IRuleRepository<TruckRuleImporter>> _repositoryMock;
        private Mock<IRuleValidator<TruckRuleImporter>> _ruleValidatorMock;
        private Mock<IRuleService<TruckRuleImporter>> _ruleServiceMock;

        protected override TruckRuleImporterController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new TruckRuleActionsConfig());
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _repositoryMock = new Mock<IRuleRepository<TruckRuleImporter>>();
            _ruleValidatorMock = new Mock<IRuleValidator<TruckRuleImporter>>();
            _ruleServiceMock = new Mock<IRuleService<TruckRuleImporter>>();

            return new TruckRuleImporterController(
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
            Assert.AreEqual(2, model.FieldsErrors.Count);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<TruckRuleImporter>(x => x.CWIOR), model.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(0).Message);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<TruckRuleImporter>(x => x.CWSupplier), model.FieldsErrors.ElementAt(1).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(1).Message);
        }
    }
}