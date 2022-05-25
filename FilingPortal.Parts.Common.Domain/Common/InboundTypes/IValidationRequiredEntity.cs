namespace FilingPortal.Parts.Common.Domain.Common.InboundTypes
{
    /// <summary>
    /// Public interface for records that requires db validation
    /// </summary>
    public interface IValidationRequiredEntity
    {
        /// <summary>
        /// Gets or sets value that indicates that record is valid or not
        /// </summary>
        bool ValidationPassed { get; set; }

        /// <summary>
        /// Gets or sets the validation results value
        /// </summary>
        string ValidationResult { get; set; }
    }
}
