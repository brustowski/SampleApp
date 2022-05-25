namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Field configuration base class with sections information included
    /// </summary>
    /// <typeparam name="TSection"></typeparam>
    public abstract class BaseDefValueWithSection<TSection> : BaseDefValue
    where TSection: BaseSection
    {
        /// <summary>
        /// Gets or sets Section Id
        /// </summary>
        public int SectionId { get; set; }

        /// <summary>
        /// Gets or sets Section
        /// </summary>
        public TSection Section { get; set; }

        /// <summary>
        /// Gets or sets the Depends On field Id
        /// </summary>
        public int? DependsOnId { get; set; }

        /// <summary>
        /// Gets or sets whether this field requires validation
        /// </summary>
        public bool ConfirmationNeeded { get; set; }
    }
}