using System.Collections.Generic;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Enums;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Validators
{
    /// <summary>
    /// Implements validation results builder
    /// </summary>
    internal class FieldsValidationResultBuilder : IFieldsValidationResultBuilder
    {
        private List<FieldsValidationResult> _result = new List<FieldsValidationResult>();
        /// <summary>
        /// Resets builder to start state
        /// </summary>
        public void Reset()
        {
            _result = new List<FieldsValidationResult>();
        }
        /// <summary>
        /// Builds fields validation results list
        /// </summary>
        public IReadOnlyCollection<FieldsValidationResult> Build()
        {
            return _result.AsReadOnly();
        }
        /// <summary>
        /// Append field validation results
        /// </summary>
        /// <param name="validationResult">Validation results</param>
        public void AddRange(IEnumerable<FieldsValidationResult> validationResult) =>
            validationResult.ForEach(Add);

        private void Add(FieldsValidationResult result)
        {
            if (result.OverrideId != null)
            {
                _result.RemoveAll(x => x.OverrideId == result.OverrideId);
            }
            _result.Add(result);
        }
        /// <summary>
        /// Adds validation result based on condition
        /// </summary>
        /// <param name="condition">Condition</param>
        /// <param name="message">Validation message</param>
        /// <param name="overrideId">Override Id, if overwrite is required</param>
        /// <param name="fields">Affected fields list</param>
        /// <param name="severity">Validation message severity</param>
        public void AddIf(bool condition, string message, string overrideId = null, string fields = null,
            Severity severity = Severity.Error)
        {
            if (condition)
                Add(new FieldsValidationResult(message, fields, severity) { OverrideId = overrideId });
        }
        /// <summary>
        /// Adds validation result
        /// </summary>
        /// <param name="message">Validation message</param>
        /// <param name="overrideId">Override Id, if overwrite is required</param>
        /// <param name="fields">Affected fields list</param>
        /// <param name="severity">Validation message severity</param>
        public void Add(string message, string overrideId = null, string fields = null,
            Severity severity = Severity.Error) => Add(
            new FieldsValidationResult(message, fields, severity) { OverrideId = overrideId });
    }
}