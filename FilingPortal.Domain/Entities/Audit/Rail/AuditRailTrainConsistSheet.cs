using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.Audit.Rail
{
    /// <summary>
    /// Represents model for rail audit train consist sheet
    /// </summary>
    public class AuditRailTrainConsistSheet : AuditableEntity
    {
        /// <summary>
        /// Gets or sets entry number
        /// </summary>
        public string EntryNumber { get; set; }
        /// <summary>
        /// Gets or sets the bill number
        /// </summary>
        public string BillNumber { get; set; }
        /// <summary>
        /// Gets or sets Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the author
        /// </summary>
        public virtual AppUsersModel Author { get; set; }
    }
}
