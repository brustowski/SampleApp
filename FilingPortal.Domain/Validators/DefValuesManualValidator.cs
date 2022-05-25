using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FilingPortal.Domain.Common.Validation;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Mapping;
using FilingPortal.Parts.Common.Domain.Repositories;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Provides field validation for DefValues_Manual models
    /// </summary>
    /// <typeparam name="TDefValuesManual">Model type</typeparam>
    public class DefValuesManualValidator<TDefValuesManual> : IDefValuesManualValidator<TDefValuesManual>
        where TDefValuesManual : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Filing model fields repository
        /// </summary>
        protected readonly IDefValuesManualReadModelRepository<TDefValuesManual> Repository;

        /// <summary>
        /// Dictionary of active validation groups
        /// </summary>
        protected readonly Dictionary<string, bool> ActiveValidations = new Dictionary<string, bool>();

        /// <summary>
        /// Creates a new instance of field-by-field validator
        /// </summary>
        /// <param name="repository">DefValues_Manual repository</param>
        public DefValuesManualValidator(IDefValuesManualReadModelRepository<TDefValuesManual> repository)
        {
            Repository = repository;
            RegisterValidationGroup("required");
        }

        /// <summary>
        /// Registers validation group
        /// </summary>
        /// <param name="validationGroupName"></param>
        protected void RegisterValidationGroup(string validationGroupName) => ActiveValidations.Add(validationGroupName, true);

        /// <summary>
        /// Validates database model
        /// </summary>
        /// <param name="models">Database model</param>
        public virtual IDictionary<TDefValuesManual, DetailedValidationResult> ValidateDatabaseModels(IEnumerable<TDefValuesManual> models)
        {
            var result = new Dictionary<TDefValuesManual, DetailedValidationResult>();

            models.ForEach(model => result.Add(model, IsValid(model)));

            return result;
        }

        /// <summary>
        /// Validates user models
        /// </summary>
        /// <param name="models">User models</param>
        public virtual IDictionary<InboundRecordFilingParameters, DetailedValidationResult> ValidateUserModels(IEnumerable<InboundRecordFilingParameters> models)
        {
            var result = new Dictionary<InboundRecordFilingParameters, DetailedValidationResult>();

            IEnumerable<int> ids = models.Select(x => x.FilingHeaderId);
            IEnumerable<IGrouping<int, TDefValuesManual>> dbModels = Repository.GetAllDataByFilingHeaderIds(ids).GroupBy(x => x.FilingHeaderId);

            foreach (IGrouping<int, TDefValuesManual> dbModel in dbModels)
            {
                InboundRecordFilingParameters correspondingModel = models.First(x => x.FilingHeaderId == dbModel.Key);
                correspondingModel.Parameters.ForEach(par =>
                {
                    TDefValuesManual dbField = dbModel.First(x => x.Id == par.Id);
                    dbField.Value = GetValue(par, dbField.ValueType); // Update value
                });

                ICollection<DetailedValidationResult> validationResults = ValidateDatabaseModels(dbModel).Values;

                var validationResult = new DetailedValidationResult();
                validationResults.ForEach(x =>
                {
                    if (!x.IsValid)
                        x.Errors.ForEach(err => validationResult.AddError(err));
                });

                result.Add(correspondingModel, validationResult);
            }
            return result;
        }

        private string GetValue(InboundRecordParameter parameter, string fieldType)
        {
            switch (fieldType)
            {
                case "Address":
                    AppAddress address = parameter.Value.Map<string, AppAddress>();
                    return address?.Id.ToString() ?? string.Empty;
                default:
                    return parameter.Value;
            }
        }

        /// <summary>
        /// Disables validation checks
        /// </summary>
        /// <param name="validationGroupName">Validation group name</param>
        public void DisableValidationGroup(string validationGroupName)
        {
            if (ActiveValidations.ContainsKey(validationGroupName))
                ActiveValidations[validationGroupName] = false;
        }

        /// <summary>
        /// Enables validation checks
        /// </summary>
        /// <param name="validationGroupName">Validation group name</param>
        public void EnableValidationGroup(string validationGroupName)
        {
            if (ActiveValidations.ContainsKey(validationGroupName))
                ActiveValidations[validationGroupName] = true;
        }

        /// <summary>
        /// Main validation method, returns validation result
        /// </summary>
        /// <param name="model">Model under validation</param>
        protected virtual DetailedValidationResult IsValid(TDefValuesManual model)
        {
            var validationResult = new DetailedValidationResult();

            if (ActiveValidations[DefaultValidationGroups.Required])
                if (model.Mandatory && string.IsNullOrEmpty(model.Value))
                {
                    validationResult.AddError($"Field {model.Label} is mandatory");
                    if (model.DisplayOnUI == 0)
                        validationResult.AddError($"Field {model.Label} is mandatory, but not displayed");
                }

            if (!string.IsNullOrEmpty(model.Value))
            {
                if (model.ValueMaxLength.HasValue && model.ValueMaxLength != -1 && model.Value.Length > model.ValueMaxLength.Value)
                    validationResult.AddError($"Value in {model.Label} field exceeds its max length");

                Type type = TypeExtension.ToClrType(model.ValueType);
                if (type == null)
                    validationResult.AddError($"Value in {model.Label} field has unknown type \"{model.ValueType}\"");
                else
                    try
                    {
                        object resultObject = (model.Value == null) ? null : Convert.ChangeType(model.Value, type, CultureInfo.GetCultureInfo("en-US"));
                    }
                    catch
                    {
                        validationResult.AddError($"Value {model.Value} in {model.Label} can not be converted to {type.FullName}");
                    }
            }

            return validationResult;
        }
    }

    public class DefaultValidationGroups
    {
        public static string Required = "required";
    }
}
