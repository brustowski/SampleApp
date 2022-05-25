namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest Hazardous Material
    /// </summary>
    public class HazardousMaterial
    {
        /// <summary>
        /// Gets or sets the Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets the Class
        /// </summary>
        public string Class { get; set; }
        /// <summary>
        /// Gets or sets the Code Qualifier
        /// </summary>
        public string CodeQualifier { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the Contact Number
        /// </summary>
        public string ContactNumber { get; set; }
        /// <summary>
        /// Gets or sets the Flash Point Temperature
        /// </summary>
        public string FlashPointTemperature { get; set; }
        /// <summary>
        /// Gets or sets the Detailed Description
        /// </summary>
        public string DetailedDescription { get; set; }
        /// <summary>
        /// Gets or sets the Classification or Division or Label Requirement
        /// </summary>
        public string ClassificationDivisionLabelRequirement { get; set; }
    }
}
