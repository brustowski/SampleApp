using System.Collections;
using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.FieldConfigurations.Rail;
using Framework.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.FieldConfigurations.Rail
{
    [TestClass]
    public class RailFormConfigFactoryTests
    {
        private RailInboundFormConfigFactory _factory;

        [TestInitialize]
        public void Init()
        {
            _factory = new RailInboundFormConfigFactory();
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ImporterField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Importer");

            Assert.IsNotNull(field);
            Assert.AreEqual("Importer", field.Title);
            Assert.IsFalse(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.ImporterLongTitles, field.Options["provider"]);
            CollectionAssert.DoesNotContain((ICollection)field.Options.Keys, "long");
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ConsigneeField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Consignee");

            Assert.IsNotNull(field);
            Assert.AreEqual("Consignee", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.ImporterLongTitles, field.Options["provider"]);
            CollectionAssert.DoesNotContain((ICollection) field.Options.Keys, "long");
        }

        [TestMethod]
        public void CreateFormConfig_Returns_DestinationField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Destination");

            Assert.IsNotNull(field);
            Assert.AreEqual("Destination", field.Title);
            Assert.IsFalse(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.StateNames, field.Options["provider"]);
            CollectionAssert.DoesNotContain((ICollection)field.Options.Keys, "long");
        }

        [TestMethod]
        public void CreateFormConfig_Returns_SupplierField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Supplier");

            Assert.IsNotNull(field);
            Assert.AreEqual("Supplier", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.SuppliersLongTitles, field.Options["provider"]);
            CollectionAssert.DoesNotContain((ICollection)field.Options.Keys, "long");
        }

        [TestMethod]
        public void CreateFormConfig_Returns_DescriptionField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Description");

            Assert.IsNotNull(field);
            Assert.AreEqual("Description", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.MultilineText.GetDescription(), field.Options["type"]);
            Assert.AreEqual(true, field.Options["long"]);
            Assert.AreEqual(true, field.Options["multiline"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_EquipmentInitialField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "EquipmentInitial");

            Assert.IsNotNull(field);
            Assert.AreEqual("Equipment Initial", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Text.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_EquipmentNumberField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "EquipmentNumber");

            Assert.IsNotNull(field);
            Assert.AreEqual("Equipment Number", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Text.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_IssuerCodeField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "IssuerCode");

            Assert.IsNotNull(field);
            Assert.AreEqual("Issuer Code", field.Title);
            Assert.AreEqual(DataProviderNames.IssuerCodes, field.Options["provider"]);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_BillOfLadingField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "BillOfLading");

            Assert.IsNotNull(field);
            Assert.AreEqual("Bill of Lading", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Text.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_PortOfUnladingField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "PortOfUnlading");

            Assert.IsNotNull(field);
            Assert.AreEqual("Port of Unlading", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.DomesticPorts, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ManifestUnitsField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ManifestUnits");

            Assert.IsNotNull(field);
            Assert.AreEqual("Manifest Units", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Units, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_WeightField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Weight");

            Assert.IsNotNull(field);
            Assert.AreEqual("Weight", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Number.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_WeightUnitField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "WeightUnit");

            Assert.IsNotNull(field);
            Assert.AreEqual("Weight Unit", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Units, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ReferenceNumberField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ReferenceNumber");

            Assert.IsNotNull(field);
            Assert.AreEqual("Reference Number", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Text.GetDescription(), field.Options["type"]);
        }
    }
}
