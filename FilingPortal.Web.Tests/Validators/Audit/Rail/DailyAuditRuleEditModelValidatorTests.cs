using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FilingPortal.Web.Models.Audit.Rail;
using FilingPortal.Web.Validators.Audit.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Validators.Audit.Rail
{
    [TestClass()]
    public class DailyAuditRuleEditModelValidatorTests
    {
        private DailyAuditRuleEditModelValidator _validator;

        [TestInitialize]
        public void Init()
        {
            _validator = new DailyAuditRuleEditModelValidator();
        }

        public DailyAuditRuleEditModel CreateValidModel()
        {
            return new DailyAuditRuleEditModel
            {
                ApiFrom = 10,
                GoodsDescription = "Description",
                ApiTo = 20,
                ImporterCode = "IMPORTER",
                PortCode = "1100",
                CustomsAttrib1 = "Description",
                Tariff = "100000",
                Carrier = "BSNT",
                DestinationState = "AL",
                ValueRecon = "VL",
                NaftaRecon = "Y",
                Id = 1,
                InvoiceQtyUnit = "KG",
                CountryOfOrigin = "US",
                GrossWeightUq = "KG",
                FirmsCode = "12345",
                ManufacturerMid = "MANUFACTURER",
                SupplierMid = "SUPPLIER",
                UltimateConsigneeName = "CONSIGNEE",
                UnitPrice = 10,
                CustomsQtyUnit = "KG",
                LastModifiedBy = "sa",
                ExportingCountry = "CA",
                LastModifiedDate = "1/1/2000"
            };
        }

        [TestMethod]
        public void ValidModel_Passes_Validation()
        {
            // Assign
            var model = CreateValidModel();

            // Act
            var validationResult = _validator.Validate(model);

            // Assert
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void Validate_Empty_API_for_tariff_27_Fails()
        {
            // Assign
            var model = CreateValidModel();
            model.Tariff = "27";
            model.ApiFrom = null;
            model.ApiTo = null;

            // Act
            var validationResult = _validator.Validate(model);

            // Assert
            Assert.IsFalse(validationResult.IsValid);
            Assert.IsTrue(validationResult.Errors.Any(x => x.ErrorMessage == "API from is required"));
            Assert.IsTrue(validationResult.Errors.Any(x => x.ErrorMessage == "API to is required"));
        }

        [TestMethod]
        public void Validate_API_for_tariff_27_Passes()
        {
            // Assign
            var model = CreateValidModel();
            model.Tariff = "27";

            // Act
            var validationResult = _validator.Validate(model);

            // Assert
            Assert.IsTrue(validationResult.IsValid);
            Assert.IsFalse(validationResult.Errors.Any(x => x.ErrorMessage == "API from is required"));
            Assert.IsFalse(validationResult.Errors.Any(x => x.ErrorMessage == "API to is required"));
        }
    }
}