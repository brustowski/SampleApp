using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.FieldConfigurations.Vessel;
using Framework.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass]
    public class VesselFormConfigFactoryTest
    {
        private VesselFormConfigFactory _factory;

        [TestInitialize]
        public void Init()
        {
            _factory = new VesselFormConfigFactory();
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ImporterIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ImporterId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Importer", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Importers, field.Options["provider"]);
            Assert.AreEqual(true, field.Options["long"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_SupplierIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "SupplierId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Supplier", field.Title);
            Assert.IsFalse(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Suppliers, field.Options["provider"]);
            Assert.AreEqual(true, field.Options["long"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_VesselIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "VesselId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Vessel", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Vessels, field.Options["provider"]);
            Assert.AreEqual(true, field.Options["long"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_CustomsQtyField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "CustomsQty");

            Assert.IsNotNull(field);
            Assert.AreEqual("Customs Qty", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Number.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_UnitPriceField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "UnitPrice");

            Assert.IsNotNull(field);
            Assert.AreEqual("Unit Price", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Number.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_OwnerRefField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "OwnerRef");

            Assert.IsNotNull(field);
            Assert.AreEqual("Owner Ref", field.Title);
            Assert.IsTrue(field.IsMandatory);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_CountryOfOriginIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "CountryOfOriginId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Country of Origin", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Countries, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_StateIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "StateId");

            Assert.IsNotNull(field);
            Assert.AreEqual("State", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.VesselImportsStateIds, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_FirmsCodeIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "FirmsCodeId");

            Assert.IsNotNull(field);
            Assert.AreEqual("FIRMs Code", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.CargowiseFirmsCodes, field.Options["provider"]);
            Assert.AreEqual("StateId", field.Options["dependsOn"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ClassificationIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ClassificationId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Classification", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.TariffIds, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ProductDescriptionIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ProductDescriptionId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Product Description", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.VesselProductDescriptions, field.Options["provider"]);
            Assert.AreEqual("ClassificationId", field.Options["dependsOn"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_EtaField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Eta");

            Assert.IsNotNull(field);
            Assert.AreEqual("ETA", field.Title);
            Assert.IsFalse(field.IsMandatory);
            Assert.AreEqual(FieldType.Date.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_FilerIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "FilerId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Filer ID", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.ApplicationUserData, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ContainerField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Container");

            Assert.IsNotNull(field);
            Assert.AreEqual("Container", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual("BLK", field.Value);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Containers, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_EntryTypeField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "EntryType");

            Assert.IsNotNull(field);
            Assert.AreEqual("Entry Type", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual("01", field.Value);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.EntryTypes, field.Options["provider"]);
        }
    }
}
