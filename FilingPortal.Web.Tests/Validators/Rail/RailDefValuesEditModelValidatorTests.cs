using System;
using System.Linq;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.Validators.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Validators.Rail
{
    [TestClass]
    public class RailDefValuesEditModelValidatorTests
    {
        private RailDefValuesEditModelValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new RailDefValuesEditModelValidator();
        }

        private RailDefValuesEditModel CreateValidModel(Action<RailDefValuesEditModel> action = null)
        {
            var model = new RailDefValuesEditModel
            {
                Id = 1,
                DisplayOnUI = "1",
                ValueLabel = "ValueLabel",
                ValueDesc = "ValueDesc",
                DefaultValue = "DefValue",
                TableName = "TableName",
                ColumnName = "ColName",
                Manual = "1",
                HasDefaultValue = true,
                Editable = true,
                UISection = "UISection",
                Mandatory = false

            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfAllFieldsValid()
        {
            RailDefValuesEditModel model = CreateValidModel();

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsEmpty()
        {
            RailDefValuesEditModel model = CreateValidModel(x =>
            {
                x.ValueDesc = string.Empty;
                x.DefaultValue = string.Empty;
                x.UISection = string.Empty;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableFieldsNull()
        {
            RailDefValuesEditModel model = CreateValidModel(x =>
            {
                x.ValueDesc = null;
                x.DefaultValue = null;
                x.UISection = null;
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfIdNotSpecified()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.Id = -1);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Id is required", result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfNullableStringFieldsNotExceedsLength()
        {
            RailDefValuesEditModel model = CreateValidModel(x =>
            {
                x.ValueLabel = new string('i', 128);
                x.ValueDesc = new string('i', 128);
                x.DefaultValue = new string('i', 128);
                x.TableName = new string('i', 128);
                x.ColumnName = new string('i', 128);
                x.UISection = new string('i', 32);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfNullableStringFieldsExceedsLength()
        {
            RailDefValuesEditModel model = CreateValidModel(x =>
            {
                x.ValueLabel = new string('i', 129);
                x.ValueDesc = new string('i', 129);
                x.DefaultValue = new string('i', 129);
                x.TableName = new string('i', 129);
                x.ColumnName = new string('i', 129);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(5, result.Errors.Count);
            Assert.IsTrue(result.Errors.All(x => x.ErrorMessage == "The field must be up to 128 characters long"));
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfUISectionFieldExceedsLength()
        {
            RailDefValuesEditModel model = CreateValidModel(x =>
            {
                x.UISection = new string('i', 33);
            });

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("The field must be up to 32 characters long", result.Errors.First(x => x.PropertyName == "UISection").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfDisplayOnUIIsCorrectByte()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.DisplayOnUI = "1");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDisplayOnUIIsNull()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.DisplayOnUI = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDisplayOnUIIsEmpty()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.DisplayOnUI = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDisplayOnUIIsDecimal()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.DisplayOnUI = "12345.89");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - byte", result.Errors.First(x => x.PropertyName == "DisplayOnUI").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDisplayOnUIIsLetters()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.DisplayOnUI = "234sret34");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - byte", result.Errors.First(x => x.PropertyName == "DisplayOnUI").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDisplayOnUIExceedsByteLength()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.DisplayOnUI = byte.MaxValue.ToString() + "1");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - byte", result.Errors.First(x => x.PropertyName == "DisplayOnUI").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfDisplayOnUIIsNegative()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.DisplayOnUI = "-1");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - byte", result.Errors.First(x => x.PropertyName == "DisplayOnUI").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsTrue_IfManualIsCorrectByte()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.Manual = "1");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManualEmpty()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.Manual = string.Empty);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Manual is required", result.Errors.First(x => x.PropertyName == "Manual").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManualNull()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.Manual = null);

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Manual is required", result.Errors.First(x => x.PropertyName == "Manual").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManualIsDecimal()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.Manual = "12345.89");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - byte", result.Errors.First(x => x.PropertyName == "Manual").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManualIsLetters()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.Manual = "234sret34");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - byte", result.Errors.First(x => x.PropertyName == "Manual").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManualExceedsByteLength()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.Manual = byte.MaxValue.ToString() + "1");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - byte", result.Errors.First(x => x.PropertyName == "Manual").ErrorMessage);
        }

        [TestMethod]
        public void Validate_ReturnsFalse_IfManualIsNegative()
        {
            RailDefValuesEditModel model = CreateValidModel(x => x.Manual = "-1");

            FluentValidation.Results.ValidationResult result = _validator.Validate(model);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Provided value does not match field format - byte", result.Errors.First(x => x.PropertyName == "Manual").ErrorMessage);
        }
    }
}
