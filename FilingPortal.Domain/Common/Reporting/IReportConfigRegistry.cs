namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Describes report configuration registry
    /// </summary>
    public interface IReportConfigRegistry
    {
        /// <summary>
        /// Gets report configuration
        /// </summary>
        /// <param name="gridName">Grid name</param>
        IReportConfig GetConfig(string gridName);
    }
}
