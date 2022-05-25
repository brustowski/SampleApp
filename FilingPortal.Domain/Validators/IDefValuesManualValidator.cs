using System.Collections.Generic;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Validators.Common;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.Base;

namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Provides field validation for DefValues_Manual models
    /// </summary>
    /// <typeparam name="TDefValuesManual">Model type</typeparam>
    public interface IDefValuesManualValidator<TDefValuesManual> where TDefValuesManual : BaseDefValuesManualReadModel
    {
        /// <summary>
        /// Validates database model
        /// </summary>
        /// <param name="models">Database model</param>
        IDictionary<TDefValuesManual, DetailedValidationResult> ValidateDatabaseModels(IEnumerable<TDefValuesManual> models);
        /// <summary>
        /// Validates user models
        /// </summary>
        /// <param name="models">User models</param>
        IDictionary<InboundRecordFilingParameters, DetailedValidationResult> ValidateUserModels(IEnumerable<InboundRecordFilingParameters> models);

        /// <summary>
        /// Disables validation checks
        /// </summary>
        /// <param name="validationGroupName">Validation group name</param>
        void DisableValidationGroup(string validationGroupName);

        /// <summary>
        /// Enables validation checks
        /// </summary>
        /// <param name="validationGroupName">Validation group name</param>
        void EnableValidationGroup(string validationGroupName);
    }
}