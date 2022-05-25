namespace FilingPortal.Domain.Commands
{
    using Framework.Domain.Commands;
    /// <summary>
    /// Implements vessel export mass delete command
    /// </summary>
    public class VesselExportMassDeleteCommand : ICommand
    {
        /// <summary>
        /// Gets or sets ids to delete
        /// </summary>
        public int[] RecordIds { get; set; }
    }
}
