using Framework.Domain;

namespace FilingPortal.Parts.Common.Domain.Entities.Base
{
    /// <summary>
    /// Base class for Table and column source
    /// </summary>
    public abstract class BaseTable: Entity
    {
        /// <summary>
        /// Gets or sets Table Name
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Gets or sets Column Name
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets Section id
        /// </summary>
        public int SectionId { get; set; }
        /// <summary>
        /// Gets or sets Section Title
        /// </summary>
        public string SectionTitle { get; set; }
    }
}
