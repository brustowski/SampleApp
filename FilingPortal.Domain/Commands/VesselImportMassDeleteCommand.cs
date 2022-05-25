using Framework.Domain.Commands;

namespace FilingPortal.Domain.Commands
{
    /// <summary>
    /// Implements vessel mass delete command
    /// </summary>
    public class VesselImportMassDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets ids to delete
        /// </summary>
        public virtual int[] RecordIds { get; set; }
    }
}
