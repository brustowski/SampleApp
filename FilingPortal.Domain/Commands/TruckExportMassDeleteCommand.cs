namespace FilingPortal.Domain.Commands
{
    using Framework.Domain.Commands;
    /// <summary>
    /// Implements truck export mass delete command
    /// </summary>
    public class TruckExportMassDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets ids to delete
        /// </summary>
        public int[] RecordIds { get; set; }
    }
}
