using FilingPortal.Domain.DTOs;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.Models.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.Web.FieldConfigurations.Rail;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass]
    public class ManifestFieldFactoryTest
    {
        private IManifestFactory _factory;

        [TestInitialize]
        public void TestInitialize()
        {
            var builder = new Mock<IFieldConfigurationBuilder>();
            builder.Setup(x => x.Create(It.IsAny<string>()).DefaultValue(It.IsAny<string>())).Returns(builder.Object);
            builder.Setup(x => x.Long()).Returns(builder.Object);
            builder.Setup(x => x.Multiline()).Returns(builder.Object);
            builder.Setup(x => x.Separator()).Returns(builder.Object);
            builder.Setup(x => x.Build()).Returns(new FieldModel());
            _factory = new RailManifestFactory(builder.Object);
        }

        [TestMethod]
        public void CreateFrom_Manifest_CreatesFieldSet()
        {
            var manifest = new Manifest
            {
                Consignee = "Consignee",
                Importer = "Importer",
                Supplier = "Supplier",
                EquipmentInitial = "EquipmentInitial",
                EquipmentNumber = "EquipmentNumber",
                IssuerCode = "IssuerCode",
                BillofLading = "BillofLading",
                PortofUnlading = "PortofUnlading",
                Description1 = "Description1",
                ManifestUnits = "ManifestUnits",
                Weight = "Weight",
                WeightUnit = "WeightUnit",
                ReferenceNumber1 = "ReferenceNumber1",
                Destination = "LA"
            };

            ManifestModel result = _factory.CreateFrom(manifest);
            Assert.IsInstanceOfType(result, typeof(ManifestModel));
            Assert.AreEqual(14, result.Fields.Count());
        }
    }
}
