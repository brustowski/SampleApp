using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.Imports.TruckExport.Inbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Domain.Tests.Validators.TruckExport
{
    [TestClass]
    public class TruckExportImportModelValidatorTests
    {
        private TruckExportImportModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new TruckExportImportModelValidator();
        }

        private static TruckExportImportModel CreateValidModel(Action<TruckExportImportModel> action = null)
        {
            var model = new TruckExportImportModel
            {
                Exporter = "Exporter Code",
                Importer = "Importer",
                TariffType = "TT",
                Tariff = "Tariff",
                RoutedTran = "Y",
                SoldEnRoute = "SoldEnRoute",
                MasterBill = "MasterBill",
                Origin = "Origin",
                Export = "Export",
                ExportDate = DateTime.Parse("4/30/2019"),
                ECCN = "eccn",
                GoodsDescription = "Goods Description",
                CustomsQty = 1.23M,
                Price = 1.24M,
                GrossWeight = 100M,
                GrossWeightUOM = "T",
                RowNumberInFile = 1,
                Hazardous = "Y",
                OriginIndicator = "D",
                GoodsOrigin = "Goods"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            TruckExportImportModel model = CreateValidModel();

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_ExporterBoundaryValues(int length, bool expectedResult)
        {
            TruckExportImportModel model = CreateValidModel(m => m.Exporter = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfExporterNotSpecified()
        {
            TruckExportImportModel model = CreateValidModel(x => x.Exporter = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.ExporterIsRequired, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfExporterExceedsLength()
        {
            TruckExportImportModel model = CreateValidModel(x => x.Exporter = new string('i', 129));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128), result.Errors.First().ErrorMessage);
        }

        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_ConsigneeCodeBoundaryValues(int length, bool expectedResult)
        {
            TruckExportImportModel model = CreateValidModel(m => m.Importer = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfConsigneeCodeNotSpecified()
        {
            TruckExportImportModel model = CreateValidModel(x => x.Importer = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Receiver Consignee Name"), result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfConsigneeCodeExceedsLength()
        {
            TruckExportImportModel model = CreateValidModel(x => x.Importer = new string('i', 129));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128), result.Errors.First().ErrorMessage);
        }

        [DataRow(0, false)]
        [DataRow(35, true)]
        [DataRow(36, false)]
        [DataTestMethod]
        public void Validate_TariffBoundaryValues(int length, bool expectedResult)
        {
            TruckExportImportModel model = CreateValidModel(m => m.Tariff = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfTariffNotSpecified()
        {
            TruckExportImportModel model = CreateValidModel(x => x.Tariff = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.TariffIsRequired, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfTariffExceedsLength()
        {
            TruckExportImportModel model = CreateValidModel(x => x.Tariff = new string('i', 36));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 35), result.Errors.First().ErrorMessage);
        }


        [DataRow(0, false)]
        [DataRow(1, true)]
        [DataRow(10, true)]
        [DataRow(11, false)]
        [DataTestMethod]
        public void Validate_RoutedTranBoundaryValues(int length, bool expectedResult)
        {
            TruckExportImportModel model = CreateValidModel(m => m.RoutedTran = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfRoutedTranNotSpecified()
        {
            TruckExportImportModel model = CreateValidModel(x => x.RoutedTran = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "RoutedTran"), result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfRoutedTranExceedsLength()
        {
            TruckExportImportModel model = CreateValidModel(x => x.RoutedTran = new string('i', 12));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 10), result.Errors.First().ErrorMessage);
        }

        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_ECCNBoundaryValues(int length, bool expectedResult)
        {
            TruckExportImportModel model = CreateValidModel(m => m.ECCN = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfECCNNotSpecified()
        {
            TruckExportImportModel model = CreateValidModel(x => x.ECCN = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(ValidationMessages.ECCNIsRequired, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfECCNExceedsLength()
        {
            TruckExportImportModel model = CreateValidModel(x => x.ECCN = new string('i', 129));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128), result.Errors.First().ErrorMessage);
        }

        [DataRow(0, false)]
        [DataRow(128, true)]
        [DataRow(129, false)]
        [DataTestMethod]
        public void Validate_GoodsDescriptionBoundaryValues(int length, bool expectedResult)
        {
            TruckExportImportModel model = CreateValidModel(m => m.GoodsDescription = new string('i', length));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfGoodsDescriptionNotSpecified()
        {
            TruckExportImportModel model = CreateValidModel(x => x.GoodsDescription = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Product"), result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsErrorMessage_IfGoodsDescriptionExceedsLength()
        {
            TruckExportImportModel model = CreateValidModel(x => x.GoodsDescription = new string('i', 129));

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(string.Format(ValidationMessages.ValueExceedsSpecifiedLength, 128), result.Errors.First().ErrorMessage);
        }
    }
}
