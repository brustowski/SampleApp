using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Repositories;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Repositories;

namespace FilingPortal.Domain.Tests.Validators
{
    [TestClass]
    public abstract class DefValuesManualValidatorTests<T>
        where T : BaseDefValuesManualReadModel
    {
        private DefValuesManualValidator<T> validator;

        private Mock<IDefValuesManualReadModelRepository<T>> _repositoryMock;

        [TestInitialize]
        public void Init()
        {
            _repositoryMock = new Mock<IDefValuesManualReadModelRepository<T>>();

            validator = new DefValuesManualValidator<T>(_repositoryMock.Object);
        }

        [TestMethod]
        public void ValidateDatabaseModels_with_no_models_returns_empty_result()
        {
            var models = new List<T>();

            IDictionary<T, DetailedValidationResult> result = validator.ValidateDatabaseModels(models);

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void ValidateDatabaseModels_with_models_returns_result_for_each_model()
        {
            var models = new List<T>
            {
                CreateModel(45),
                CreateModel(59)
            };

            IDictionary<T, DetailedValidationResult> result = validator.ValidateDatabaseModels(models);

            Assert.AreEqual(2, result.Count);

        }

        [DataRow(1, 1, null, null, "varchar", false)]
        [DataRow(0, 1, null, null, "varchar", true)]
        [DataRow(1, 0, null, null, "varchar", false)]
        [DataRow(0, 1, "value", 3, "varchar", false)]
        [DataRow(0, 1, "value", 5, "varchar", true)]
        [DataRow(0, 1, "value", null, "varchar", true)]
        [DataRow(0, 1, "value", null, "numeric", false)]
        [DataTestMethod]
        public void ValidateDatabaseModel_returns_correct_validation(int fMandatory, int displayOnUi, string value, int? valueMaxLength, string valueType, bool valid)
        {
            var models = new List<T>();

            T model = CreateModel(45);
            model.Mandatory = Convert.ToBoolean(fMandatory);
            model.Value = value;
            model.Label = "Test label";
            model.ValueMaxLength = valueMaxLength;
            model.ValueType = valueType;
            model.DisplayOnUI = (byte)displayOnUi;

            models.Add(model);

            IDictionary<T, DetailedValidationResult> results = validator.ValidateDatabaseModels(models);

            DetailedValidationResult result = results[model];

            Assert.AreEqual(valid, result.IsValid);
        }

        [TestMethod]
        public void ValidateDatabaseModels_with_required_empty_returns_mandatory_error()
        {
            var models = new List<T>();

            T model = CreateModel(45);
            model.Mandatory = true;
            model.Value = null;
            model.Label = "Test label";

            models.Add(model);

            IDictionary<T, DetailedValidationResult> results = validator.ValidateDatabaseModels(models);

            DetailedValidationResult result = results[model];

            Assert.AreEqual("Field Test label is mandatory", result.Errors.First());
        }

        [TestMethod]
        public void ValidateDatabaseModels_with_required_empty_returns_length_error()
        {
            var models = new List<T>();

            T model = CreateModel(45);
            model.Mandatory = false;
            model.Value = "123456";
            model.Label = "Test label";
            model.ValueMaxLength = 5;

            models.Add(model);

            IDictionary<T, DetailedValidationResult> results = validator.ValidateDatabaseModels(models);

            DetailedValidationResult result = results[model];

            Assert.AreEqual("Value in Test label field exceeds its max length", result.Errors.First());
        }

        protected abstract T CreateModel(int id);
    }
}
