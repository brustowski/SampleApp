using Reinforced.Typings.Attributes;

namespace FilingPortal.Domain.DTOs.ReviewSectionModels
{
    /// <summary>
    /// Represents the Review Section field
    /// </summary>
    [TsClass(IncludeNamespace = false, FlattenHierarchy = true)]
    public class ReviewSectionField
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }
    }
}