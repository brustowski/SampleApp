namespace FilingPortal.Domain.Validators.Common
{
    /// <summary>
    /// Provides simple validation resulr
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        /// <summary>
        /// Validation result
        /// </summary>
        public virtual bool IsValid { get; }

        /// <summary>
        /// Error message
        /// </summary>
        public virtual string CommonError { get; }
    }
}
