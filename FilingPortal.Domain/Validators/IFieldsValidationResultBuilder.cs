using System.Collections.Generic;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Enums;

namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Describes validation results builders
    /// </summary>
    public interface IFieldsValidationResultBuilder
    {
        /// <summary>
        /// Resets builder to start state
        /// </summary>
        void Reset();
        /// <summary>
        /// Builds fields validation results list
        /// </summary>
        IReadOnlyCollection<FieldsValidationResult> Build();
        /// <summary>
        /// Append field validation results
        /// </summary>
        /// <param name="validationResult">Validation results</param>
        void AddRange(IEnumerable<FieldsValidationResult> validationResult);
        /// <summary>
        /// Adds validation result based on condition
        /// </summary>
        /// <param name="condition">Condition</param>
        /// <param name="message">Validation message</param>
        /// <param name="overrideId">Override Id, if overwrite is required</param>
        /// <param name="fields">Affected fields list</param>
        /// <param name="severity">Validation message severity</param>
        void AddIf(bool condition, string message, string overrideId = null, string fields = null,
            Severity severity = Severity.Error);
        /// <summary>
        /// Adds validation result
        /// </summary>
        /// <param name="message">Validation message</param>
        /// <param name="overrideId">Override Id, if overwrite is required</param>
        /// <param name="fields">Affected fields list</param>
        /// <param name="severity">Validation message severity</param>
        void Add(string message, string overrideId = null, string fields = null,
            Severity severity = Severity.Error);
    }
}