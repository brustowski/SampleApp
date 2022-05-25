using Framework.Domain.Commands;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Commands
{
    /// <summary>
    /// Implements inbound Validation command
    /// </summary>
    public class InboundValidationCommand : ICommand
    {
        /// <summary>
        /// Gets or sets ids to validate
        /// </summary>
        public int[] RecordIds { get; set; }
    }
}
