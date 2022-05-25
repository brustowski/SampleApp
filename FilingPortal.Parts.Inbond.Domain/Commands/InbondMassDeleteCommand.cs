using Framework.Domain.Commands;

namespace FilingPortal.Parts.Inbond.Domain.Commands
{
    /// <summary>
    /// Implements Inbond mass delete command
    /// </summary>
    public class InbondMassDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets ids to delete
        /// </summary>
        public int[] RecordIds { get; set; }
    }
}
