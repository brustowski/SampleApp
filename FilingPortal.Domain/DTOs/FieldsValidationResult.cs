using FilingPortal.Domain.Enums;
using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.DTOs
{
    /// <summary>
    /// Validation message with fields enumeration
    /// </summary>
    [TsInterface(AutoI = false, IncludeNamespace = false, FlattenHierarchy = true)]
    public class FieldsValidationResult
    {
        /// <summary>
        /// Affected fields
        /// </summary>
        public string Fields { get; set; }
        /// <summary>
        /// Validation message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Override validation rule Id
        /// </summary>
        public string OverrideId { get; set; }
        /// <summary>
        /// Gets or sets severity
        /// </summary>
        public Severity Severity { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="FieldsValidationResult"/>
        /// </summary>
        /// <param name="message">Validation message</param>
        /// <param name="fields">List of fields to check</param>
        /// <param name="severity">Validation check severity</param>
        public FieldsValidationResult(string message, string fields = null, Severity severity = Severity.Error)
        {
            Message = message;
            Fields = fields;
            Severity = severity;
        }
    }
}
