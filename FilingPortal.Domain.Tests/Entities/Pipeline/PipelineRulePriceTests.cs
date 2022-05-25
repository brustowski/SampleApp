using System;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Entities.Pipeline
{
    /// <summary>
    /// Summary description for PipelineRulePriceTests
    /// </summary>
    [TestClass]
    public class PipelineRulePriceTests
    {
        private Mock<Client> _client;
        private Mock<PipelineRuleBatchCode> _crudeType;

        private PipelineRulePrice _model;

        [TestInitialize]
        public void Init()
        {
            _client = new Mock<Client>();
            _crudeType = new Mock<PipelineRuleBatchCode>();

            _model = new PipelineRulePrice()
            {
                ImporterId = new Guid("{30956B08-FCF5-449D-88E3-2666CF535201}"),
                Freight = 100,
                Importer = _client.Object,
                Pricing = 300,
                Id = 1,
                CreatedDate = new DateTime(2000, 6, 2, 12, 0, 0),
                CrudeType = _crudeType.Object,
                CrudeTypeId = 40,
                CreatedUser = "test user"
            };
        }

        [TestMethod]
        public void HasRequiredProperties()
        {
            Assert.IsInstanceOfType(_model.Importer, typeof(Client));
            Assert.IsInstanceOfType(_model.ImporterId, typeof(Guid));
            Assert.IsInstanceOfType(_model.CrudeType, typeof(PipelineRuleBatchCode));
            Assert.IsInstanceOfType(_model.CrudeTypeId, typeof(int));
            Assert.IsInstanceOfType(_model.Freight, typeof(decimal));
            Assert.IsInstanceOfType(_model.Pricing, typeof(decimal));
            Assert.IsInstanceOfType(_model.CreatedDate, typeof(DateTime));
            Assert.IsInstanceOfType(_model.CreatedUser, typeof(string));
        }
    }
}
