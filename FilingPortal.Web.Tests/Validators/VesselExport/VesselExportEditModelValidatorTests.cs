using System;
using System.Linq;
using FilingPortal.Domain;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.Web.Models.VesselExport;
using FilingPortal.Web.Validators.VesselExport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Validators.VesselExport
{
    [TestClass]
    public class VesselExportEditModelValidatorTests
    {
        private VesselExportEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new VesselExportEditModelValidator();
        }

        private VesselExportEditModel CreateValidModel(Action<VesselExportEditModel> action = null)
        {
            var model = new VesselExportEditModel
            {
                Id = 1,
                Tariff = "Tariff",
                GoodsDescription = "GoodsDescription",
                Value = 12.24M,
                SoldEnRoute = "Y",
                ExportDate = DateTime.Now.ToShortDateString(),
                TariffType = "HTS",
                AddressId = Guid.NewGuid().ToString("D"),
                Description = "Description",
                Phone = "+1(212)363-9300",
                ImporterId = Guid.NewGuid().ToString("D"),
                ContactId = Guid.NewGuid().ToString("D"),
                CountryOfDestinationId = 1,
                Container = "container",
                UsppiId = Guid.NewGuid().ToString("D"),
                Quantity = 12.24M,
                DischargePort = "12345",
                ExportAdjustmentValue = "F",
                InBond = "67",
                LoadPort = "1234",
                OriginIndicator = "D",
                OriginalItn = "ITN",
                ReferenceNumber = "RefNum",
                RoutedTransaction = "Y",
                TransportRef = "Transport ref",
                VesselId = 12,
                Weight = 12.24m
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            VesselExportEditModel model = CreateValidModel();

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x =>
            {
                x.AddressId = string.Empty;
                x.Weight = 0;
                x.OriginalItn = string.Empty;
                x.Phone = string.Empty;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            VesselExportEditModel model = CreateValidModel(x =>
            {
                x.AddressId = string.Empty;
                x.Weight = null;
                x.OriginalItn = string.Empty;
                x.ContactId = null;
                x.Phone = string.Empty;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.Tariff = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.TariffIsRequired, result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.Tariff = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.TariffIsRequired, result.Errors.First(x => x.PropertyName == "Tariff").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfAddressIdPropertyHasWrongFormat()
        {
            VesselExportEditModel model = CreateValidModel(x => x.AddressId = "wrong-guid");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch, result.Errors.First(x => x.PropertyName == "AddressId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIdPropertyHasWrongFormat()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ImporterId = "wrong-guid");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.IdFormatMismatch, result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIdPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ImporterId = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"), result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfImporterIdPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ImporterId = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Importer"), result.Errors.First(x => x.PropertyName == "ImporterId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfVesselIdPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.VesselId = 0);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Vessel"), result.Errors.First(x => x.PropertyName == "VesselId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExportDatePropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ExportDate = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Export Date"), result.Errors.First(x => x.PropertyName == "ExportDate").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExportDatePropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ExportDate = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Export Date"), result.Errors.First(x => x.PropertyName == "ExportDate").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfLoadPortPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.LoadPort = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Load Port"), result.Errors.First(x => x.PropertyName == "LoadPort").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfLoadPortPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.LoadPort = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Load Port"), result.Errors.First(x => x.PropertyName == "LoadPort").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDischargePortPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.DischargePort = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Discharge Port");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "DischargePort").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDischargePortPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.DischargePort = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Discharge Port");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "DischargePort").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfCountryOfDestinationIdPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.CountryOfDestinationId = 0);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Country Of Destination");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "CountryOfDestinationId").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffTypePropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.TariffType = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Tariff Type"), result.Errors.First(x => x.PropertyName == "TariffType").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTariffTypePropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.TariffType = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Tariff Type"), result.Errors.First(x => x.PropertyName == "TariffType").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfGoodsDescriptionPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.GoodsDescription = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Goods Description");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "GoodsDescription").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfGoodsDescriptionPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.GoodsDescription = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Goods Description");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "GoodsDescription").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfOriginIndicatorPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.OriginIndicator = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Origin Indicator");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "OriginIndicator").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfOriginIndicatorPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.OriginIndicator = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Origin Indicator");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "OriginIndicator").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfOriginIndicatorPropertyContainsUnsupportedValue()
        {
            VesselExportEditModel model = CreateValidModel(x => x.OriginIndicator = "A");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Origin Indicator must be D of F", result.Errors.First(x => x.PropertyName == "OriginIndicator").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfQuantityPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.Quantity = 0);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Quantity");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "Quantity").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfValuePropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.Value = 0);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Value");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "Value").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTransportRefPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.TransportRef = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Transport Reference");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "TransportRef").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfTransportRefPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.TransportRef = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Transport Reference");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "TransportRef").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfContainerPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.Container = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Container");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "Container").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfContainerPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.Container = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Container");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "Container").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfInBondPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.InBond = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "In-Bond");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "InBond").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfInBondPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.InBond = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "In-Bond");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "InBond").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfInBondPropertyContainsUnsupportedValue()
        {
            VesselExportEditModel model = CreateValidModel(x => x.InBond = "90");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("In-Bond must be 67 of 70", result.Errors.First(x => x.PropertyName == "InBond").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSoldEnRoutePropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.SoldEnRoute = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Sold En Route");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "SoldEnRoute").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfSoldEnRoutePropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.SoldEnRoute = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Sold En Route");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "SoldEnRoute").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExportAdjustmentValuePropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ExportAdjustmentValue = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Export Adjustment Value");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "ExportAdjustmentValue").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfExportAdjustmentValuePropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ExportAdjustmentValue = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Export Adjustment Value");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "ExportAdjustmentValue").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfRoutedTransactionPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.RoutedTransaction = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Routed Transaction");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "RoutedTransaction").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfRoutedTransactionPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.RoutedTransaction = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Routed Transaction");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "RoutedTransaction").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfRoutedTransactionPropertyContainsUnsupportedValue()
        {
            VesselExportEditModel model = CreateValidModel(x => x.RoutedTransaction = "X");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Routed Transaction must be Y of N", result.Errors.First(x => x.PropertyName == "RoutedTransaction").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfReferenceNumberPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ReferenceNumber = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Reference Number");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "ReferenceNumber").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfReferenceNumberPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.ReferenceNumber = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Reference Number");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "ReferenceNumber").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDescriptionPropertyEmpty()
        {
            VesselExportEditModel model = CreateValidModel(x => x.Description = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Description");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "Description").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDescriptionPropertyNull()
        {
            VesselExportEditModel model = CreateValidModel(x => x.Description = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);
            var message = string.Format(ValidationMessages.PropertyRequired, "Description");

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(message, result.Errors.First(x => x.PropertyName == "Description").ErrorMessage);
        }

    }
}
