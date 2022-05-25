using Framework.Domain.Commands;

namespace FilingPortal.Domain.Commands
{
    /// <summary>
    /// Implements rail mass delete command
    /// </summary>
    public class RailInboundMassRestoreCommand : ICommand
    {
        /// <summary>
        /// Gets or sets ids to delete
        /// </summary>
        public int[] RecordIds { get; set; }
    }
}
