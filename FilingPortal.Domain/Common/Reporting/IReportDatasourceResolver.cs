namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Describes the data source resolver for reports
    /// </summary>
    public interface IReportDataSourceResolver
    {
        /// <summary>
        /// Resolves data source by its name
        /// </summary>
        /// <param name="name">The data source name</param>
        IReportDatasource Resolve(string name);
    }
}
