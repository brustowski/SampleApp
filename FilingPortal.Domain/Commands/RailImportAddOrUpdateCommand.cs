using FilingPortal.Domain.Entities.Rail;
using Framework.Domain.Commands;

namespace FilingPortal.Domain.Commands
{
    /// <summary>
    /// Implements Rail import add or update command
    /// </summary>
    public class RailImportAddOrUpdateCommand : ICommand
    {
        /// <summary>
        /// Gets or sets import record
        /// </summary>
        public virtual RailBdParsed Record { get; set; }

        /// <summary>
        /// Id of record to update. If empty, adds new record instead
        /// </summary>
        public virtual int? RecordId { get; set; }
    }
}
