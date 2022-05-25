namespace FilingPortal.Domain.Validators.Common
{
    /// <summary>
    /// Provides interface for validation result
    /// </summary>
    public interface IValidationResult
    {
        /// <summary>
        /// Error message
        /// </summary>
        string CommonError { get; }
        /// <summary>
        /// Validation result
        /// </summary>
        bool IsValid { get; }
    }
}