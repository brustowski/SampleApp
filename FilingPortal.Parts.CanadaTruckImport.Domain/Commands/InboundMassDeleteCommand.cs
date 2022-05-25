using Framework.Domain.Commands;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Commands
{
    /// <summary>
    /// Implements inbound mass delete command
    /// </summary>
    public class InboundMassDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets ids to delete
        /// </summary>
        public int[] RecordIds { get; set; }
    }
}
