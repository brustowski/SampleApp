using Framework.Domain.Commands;

namespace FilingPortal.Parts.Recon.Domain.Commands
{
    /// <summary>
    /// Implements Value Recon process command
    /// </summary>
    public class ProcessValueCommand : ICommand
    {
        /// <summary>
        /// Array of identifiers to process
        /// </summary>
        public int[] Ids { get; set; }
    }
}
