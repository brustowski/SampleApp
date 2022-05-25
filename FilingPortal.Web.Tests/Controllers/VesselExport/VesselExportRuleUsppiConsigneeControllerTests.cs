using System.Linq;
using System.Net.Http;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Controllers.VesselExport;
using FilingPortal.Web.Models;
using FilingPortal.Web.Models.VesselExport;
using FilingPortal.Web.PageConfigs.Vessel;
using FilingPortal.Web.Tests.Common;
using Framework.Domain.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers.VesselExport
{
    [TestClass]
    public class VesselExportRuleUsppiConsigneeControllerTests : ApiControllerFunctionTestsBase<VesselExportRuleUsppiConsigneeController>
    {
        private Mock<IPageConfigContainer> _pageConfigContainerMock;
        private Mock<ISearchRequestFactory> _searchRequestFactoryMock;
        private Mock<IRuleRepository<VesselExportRuleUsppiConsignee>> _repositoryMock;
        private Mock<IRuleValidator<VesselExportRuleUsppiConsignee>> _ruleValidatorMock;
        private Mock<IRuleService<VesselExportRuleUsppiConsignee>> _ruleServiceMock;

        protected override VesselExportRuleUsppiConsigneeController TestInitialize()
        {
            _pageConfigContainerMock = new Mock<IPageConfigContainer>();
            _pageConfigContainerMock.Setup(x => x.GetPageConfig(It.IsAny<string>())).Returns(new VesselRuleActionsConfig());
            _searchRequestFactoryMock = new Mock<ISearchRequestFactory>();
            _repositoryMock = new Mock<IRuleRepository<VesselExportRuleUsppiConsignee>>();

            _ruleValidatorMock = new Mock<IRuleValidator<VesselExportRuleUsppiConsignee>>();
            _ruleValidatorMock.Setup(x => x.IsDuplicate(It.IsAny<VesselExportRuleUsppiConsignee>())).Returns(false);

            _ruleServiceMock = new Mock<IRuleService<VesselExportRuleUsppiConsignee>>();

            return new VesselExportRuleUsppiConsigneeController(
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
        public void RoutesAssertion()
        {
            AssertRoute(HttpMethod.Post, "/api/rules/export/vessel/usppi-consignee/gettotalmatches", x => x.GetTotalMatches(null));
            AssertRoute(HttpMethod.Post, "/api/rules/export/vessel/usppi-consignee/search", x => x.Search(null));
            AssertRoute(HttpMethod.Post, "/api/rules/export/vessel/usppi-consignee/create", x => x.Create(null));
            AssertRoute(HttpMethod.Get, "/api/rules/export/vessel/usppi-consignee/getNew", x => x.GetNewRow());
            AssertRoute(HttpMethod.Post, "/api/rules/export/vessel/usppi-consignee/update", x => x.Update(null));
            AssertRoute(HttpMethod.Post, "/api/rules/export/vessel/usppi-consignee/delete/1", x => x.Delete(1));
        }

        [TestMethod]
        public void AddRuleExistingError_AddAppropriateError()
        {
            var model = new ValidationResultWithFieldsErrorsViewModel();

            Controller.AddRuleExistingError(model);

            Assert.IsFalse(model.IsValid);
            Assert.AreEqual(2, model.FieldsErrors.Count);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<VesselExportRuleUsppiConsigneeEditModel>(x => x.Usppi), model.FieldsErrors.ElementAt(0).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(0).Message);
            Assert.AreEqual(PropertyExpressionHelper.GetPropertyName<VesselExportRuleUsppiConsigneeEditModel>(x => x.Consignee), model.FieldsErrors.ElementAt(1).FieldName);
            Assert.AreEqual(string.Empty, model.FieldsErrors.ElementAt(1).Message);
        }
    }
}