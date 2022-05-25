using System.Linq;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.FieldConfigurations.VesselExport;
using Framework.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.FieldConfigurations
{
    [TestClass]
    public class VesselExportFormConfigFactoryTest
    {
        private VesselExportFormConfigFactory _factory;

        [TestInitialize]
        public void Init()
        {
            _factory = new VesselExportFormConfigFactory();
        }

        [TestMethod]
        public void CreateFormConfig_Returns_USPPIField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "UsppiId");

            Assert.IsNotNull(field);
            Assert.AreEqual("USPPI", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Suppliers, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_AddressIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "AddressId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Address", field.Title);
            Assert.IsFalse(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.ClientAddresses, field.Options["provider"]);
            Assert.AreEqual("UsppiId", field.Options["dependsOn"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ContactField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ContactId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Contact", field.Title);
            Assert.IsFalse(field.IsMandatory);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_PhoneField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Phone");

            Assert.IsNotNull(field);
            Assert.AreEqual("Phone", field.Title);
            Assert.IsFalse(field.IsMandatory);
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
            CollectionAssert.DoesNotContain(field.Options.Keys.ToList(), "long");
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
            CollectionAssert.DoesNotContain(field.Options.Keys.ToList(), "long");
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ExportDateField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ExportDate");

            Assert.IsNotNull(field);
            Assert.AreEqual("Export Date", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Date.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_LoadPortField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "LoadPort");

            Assert.IsNotNull(field);
            Assert.AreEqual("Load Port", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.DomesticPorts, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_DischargePortField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "DischargePort");

            Assert.IsNotNull(field);
            Assert.AreEqual("Discharge Port", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.DischargePorts, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_CountryOfDestinationIdField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "CountryOfDestinationId");

            Assert.IsNotNull(field);
            Assert.AreEqual("Country Of Destination", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.DischargePortCountries, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_TariffTypeField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "TariffType");

            Assert.IsNotNull(field);
            Assert.AreEqual("Tariff Type", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.TariffTypes, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_TariffField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Tariff");

            Assert.IsNotNull(field);
            Assert.AreEqual("Tariff", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.TariffCodes, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_GoodsDescriptionField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "GoodsDescription");

            Assert.IsNotNull(field);
            Assert.AreEqual("Goods Description", field.Title);
            Assert.IsTrue(field.IsMandatory);
            CollectionAssert.DoesNotContain(field.Options.Keys.ToList(), "long");
        }

        [TestMethod]
        public void CreateFormConfig_Returns_OriginIndicatorField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "OriginIndicator");

            Assert.IsNotNull(field);
            Assert.AreEqual("Origin Indicator", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual("D", field.Value);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.OriginIndicator, field.Options["provider"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_QuantityField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Quantity");

            Assert.IsNotNull(field);
            Assert.AreEqual("Quantity", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Number.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_WeightField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Weight");

            Assert.IsNotNull(field);
            Assert.AreEqual("Weight", field.Title);
            Assert.IsFalse(field.IsMandatory);
            Assert.AreEqual(FieldType.Number.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ValueField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Value");

            Assert.IsNotNull(field);
            Assert.AreEqual("Value", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Number.GetDescription(), field.Options["type"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_TransportRefField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "TransportRef");

            Assert.IsNotNull(field);
            Assert.AreEqual("Transport Ref", field.Title);
            Assert.IsTrue(field.IsMandatory);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ContainerField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Container");

            Assert.IsNotNull(field);
            Assert.AreEqual("Container", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.Containers, field.Options["provider"]);
            Assert.AreEqual("BLK", field.Value);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_InBondField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "InBond");

            Assert.IsNotNull(field);
            Assert.AreEqual("In-Bond", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.InBond, field.Options["provider"]);
            Assert.AreEqual("70", field.Value);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_SoldEnRouteField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "SoldEnRoute");

            Assert.IsNotNull(field);
            Assert.AreEqual("Sold En Route", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.YesNoLookup, field.Options["provider"]);
            Assert.AreEqual("N", field.Value);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ExportAdjustmentValueField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ExportAdjustmentValue");

            Assert.IsNotNull(field);
            Assert.AreEqual("Export Adjustment Value", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.ExportAdjustmentValue, field.Options["provider"]);
            Assert.AreEqual("P", field.Value);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_OriginalItnField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "OriginalItn");

            Assert.IsNotNull(field);
            Assert.AreEqual("Original ITN", field.Title);
            Assert.IsFalse(field.IsMandatory);
            Assert.AreEqual("SoldEnRoute", field.Options["dependsOn"]);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_RoutedTransactionField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "RoutedTransaction");

            Assert.IsNotNull(field);
            Assert.AreEqual("Routed Transaction", field.Title);
            Assert.IsTrue(field.IsMandatory);
            Assert.AreEqual(FieldType.Lookup.GetDescription(), field.Options["type"]);
            Assert.AreEqual(DataProviderNames.YesNoLookup, field.Options["provider"]);
            Assert.AreEqual("N", field.Value);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_ReferenceNumberField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "ReferenceNumber");

            Assert.IsNotNull(field);
            Assert.AreEqual("Reference Number", field.Title);
            Assert.IsTrue(field.IsMandatory);
        }

        [TestMethod]
        public void CreateFormConfig_Returns_DescriptionField()
        {
            FieldModel field = _factory.CreateFormConfig().Fields.FirstOrDefault(x => x.Name == "Description");

            Assert.IsNotNull(field);
            Assert.AreEqual("Description", field.Title);
            Assert.IsTrue(field.IsMandatory);
            CollectionAssert.DoesNotContain(field.Options.Keys.ToList(), "long");
        }

    }
}
