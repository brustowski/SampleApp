using FilingPortal.Domain;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RulePort;
using FilingPortal.Parts.CanadaTruckImport.Web.Validators;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Validators;

namespace FilingPortal.Parts.CanadaTruckImport.WebTests.Validators
{
    [TestClass]
    public class RulePortEditModelValidatorTests
    {
        private RulePortEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new RulePortEditModelValidator();
        }

        private static RulePortEditModel CreateValidModel(Action<RulePortEditModel> action = null)
        {
            var model = new RulePortEditModel
            {
                Id = 1,
                PortOfClearance = "0714",
                SubLocation = "9602",
                FirstPortOfArrival = "CASUV",
                FinalDestination = "CANPR",
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            RulePortEditModel model = CreateValidModel();

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            RulePortEditModel model = CreateValidModel(x =>
            {
                x.SubLocation = null;
                x.FirstPortOfArrival = null;
                x.FinalDestination = null;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            RulePortEditModel model = CreateValidModel(x =>
            {
                x.SubLocation = string.Empty;
                x.FirstPortOfArrival = string.Empty;
                x.FinalDestination = string.Empty;
            });

            ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            RulePortEditModel model = CreateValidModel(x => x.Id = -1);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfClearancePropertyEmpty()
        {
            RulePortEditModel model = CreateValidModel(x => x.PortOfClearance= string.Empty);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Port Of Clearance"), 
                result.Errors.First(x => x.PropertyName == "PortOfClearance").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfPortOfClearancePropertyNull()
        {
            RulePortEditModel model = CreateValidModel(x => x.PortOfClearance = null);

            ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(string.Format(ValidationMessages.PropertyRequired, "Port Of Clearance"), 
                result.Errors.First(x => x.PropertyName == "PortOfClearance").ErrorMessage);
        }

        [DataRow(0, false, "Port Of Clearance is required")]
        [DataRow(4, true, "")]
        [DataRow(5, false, "The field must be up to 4 characters long")]
        [DataTestMethod]
        public void Validate_PortOfClearanceBoundaryValues(int length, bool expectedResult, string expectedMessage)
        {
            RulePortEditModel model = CreateValidModel(x =>
            {
                x.PortOfClearance = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
            if (!result.IsValid)
            {
                Assert.AreEqual(expectedMessage, result.Errors.First(x => x.PropertyName == "PortOfClearance").ErrorMessage);
            }
        }

        [DataRow(5, true, "")]
        [DataRow(6, false, "The field must be up to 5 characters long")]
        [DataTestMethod]
        public void Validate_FirstPortOfArrivalBoundaryValues(int length, bool expectedResult, string expectedMessage)
        {
            RulePortEditModel model = CreateValidModel(x =>
            {
                x.FirstPortOfArrival= new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
            if (!result.IsValid)
            {
                Assert.AreEqual(expectedMessage, result.Errors.First(x => x.PropertyName == "FirstPortOfArrival").ErrorMessage);
            }
        }

        [DataRow(5, true, "")]
        [DataRow(6, false, "The field must be up to 5 characters long")]
        [DataTestMethod]
        public void Validate_FinalDestinationBoundaryValues(int length, bool expectedResult, string expectedMessage)
        {
            RulePortEditModel model = CreateValidModel(x =>
            {
                x.FinalDestination = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
            if (!result.IsValid)
            {
                Assert.AreEqual(expectedMessage, result.Errors.First(x => x.PropertyName == "FinalDestination").ErrorMessage);
            }
        }

        [DataRow(50, true, "")]
        [DataRow(51, false, "The field must be up to 50 characters long")]
        [DataTestMethod]
        public void Validate_SubLocationBoundaryValues(int length, bool expectedResult, string expectedMessage)
        {
            RulePortEditModel model = CreateValidModel(x =>
            {
                x.SubLocation = new string('i', length);
            });

            ValidationResult result = _validator.Validate(model);

            Assert.AreEqual(expectedResult, result.IsValid);
            if (!result.IsValid)
            {
                Assert.AreEqual(expectedMessage, result.Errors.First(x => x.PropertyName == "SubLocation").ErrorMessage);
            }
        }
    }
}
