namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest Additional Reference
    /// </summary>
    public class AdditionalReference
    {
        /// <summary>
        /// Gets or set the Reference Type
        /// </summary>
        public string ReferenceType { get; set; } // Valid qualifier codes are: ace_309 22
        /// <summary>
        /// Gets or sets the Reference Number
        /// </summary>
        public string ReferenceNumber { get; set; }
    }
}
