using FilingPortal.Domain.Entities.Vessel;
using Framework.Domain.Commands;

namespace FilingPortal.Domain.Commands
{
    /// <summary>
    /// Implements vessel add or update command
    /// </summary>
    public class VesselImportAddOrUpdateCommand : ICommand
    {
        /// <summary>
        /// Gets or sets import record
        /// </summary>
        public virtual VesselImportRecord Record { get; set; }

        /// <summary>
        /// Id of record to update. If empty, adds new record instead
        /// </summary>
        public virtual int? RecordId { get; set; }
    }
}
