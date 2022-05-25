namespace FilingPortal.Domain.DTOs.Rail.Manifest
{
    /// <summary>
    /// Represents the Rail Manifest Involved Entity
    /// </summary>
    public class InvolvedEntity
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the Entity Type
        /// </summary>
        public string EntityType { get; set; } // Entity Identifier Code 25
        /// <summary>
        /// Gets or sets the Code Qualifier Identifier
        /// </summary>
        public string CodeQualifierId { get; set; } // Identification Code Qualifier 28
        /// <summary>
        /// Gets or sets the Code Identifier
        /// </summary>
        public string CodeId { get; set; }
        /// <summary>
        /// Gets or sets the Address
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the Contact Name
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// Gets or sets the Contact Number
        /// </summary>
        public string ContactNumber { get; set; }
    }
}
