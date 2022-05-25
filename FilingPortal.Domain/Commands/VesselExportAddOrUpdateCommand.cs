using FilingPortal.Domain.Entities.VesselExport;
using Framework.Domain.Commands;

namespace FilingPortal.Domain.Commands
{
    /// <summary>
    /// Implements vessel export add or update command
    /// </summary>
    public class VesselExportAddOrUpdateCommand : ICommand
    {
        /// <summary>
        /// Gets or sets import record
        /// </summary>
        public virtual VesselExportRecord Record { get; set; }

        /// <summary>
        /// Id of record to update. If empty, adds new record instead
        /// </summary>
        public virtual int? RecordId { get; set; }
    }
}
