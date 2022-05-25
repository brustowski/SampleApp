using System.Collections.Generic;

namespace FilingPortal.Domain.Validators.Common
{
    /// <summary>
    /// Provides validation result with list of errors
    /// </summary>
    public class DetailedValidationResult : ValidationResult
    {
        private readonly HashSet<string> _errors = new HashSet<string>();

        /// <summary>
        /// Errors list
        /// </summary>
        public IList<string> Errors => new List<string>(_errors);

        /// <summary>
        /// Validation result
        /// </summary>
        public override bool IsValid => _errors.Count == 0;

        /// <summary>
        /// Adds error to validation result
        /// </summary>
        /// <param name="message">Error message</param>
        public void AddError(string message) => _errors.Add(message);
    }
}