namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Represents the Default Value Read Model entity base class
    /// </summary>
    public abstract class BaseDefValueReadModel : BaseDefValue
    {
        /// <summary>
        /// Gets or sets Table Name
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets section name
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets section name
        /// </summary>
        public string SectionTitle { get; set; }

        /// <summary>
        /// Gets or sets the Depends On Column Identifier
        /// </summary>
        public int? DependsOnId { get; set; }

        /// <summary>
        /// Gets or sets the Depends On Column
        /// </summary>
        public string DependsOn { get; set; }

        /// <summary>
        /// Gets or sets the Value Max Length
        /// </summary>
        public int? ValueMaxLength { get; set; }

        /// <summary>
        /// Gets or sets the Value Type
        /// </summary>
        public string ValueType { get; set; }
        /// <summary>
        /// Gets or sets whether this field requires validation
        /// </summary>
        public bool ConfirmationNeeded { get; set; }
        /// <summary>
        /// Gets or sets field value
        /// </summary>
        public string DefaultValue { get; set; }
    }
}