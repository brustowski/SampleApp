using Framework.Domain.Commands;

namespace FilingPortal.Parts.Recon.Domain.Commands
{
    /// <summary>
    /// Implements FTA Recon process command
    /// </summary>
    public class ProcessFtaCommand : ICommand
    {
        /// <summary>
        /// Array of identifiers to process
        /// </summary>
        public int[] Ids { get; set; }
    }
}
