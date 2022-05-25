using System;
using System.Collections.Generic;
using System.Linq;

namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Represents the data source resolver for reports
    /// </summary>
    internal class ReportDataSourceResolver : IReportDataSourceResolver
    {
        /// <summary>
        /// Collection of the data sources
        /// </summary>
        private readonly IEnumerable<IReportDatasource> _reportDataSources;

        /// <summary>
        /// Initialize a new instance of the <see cref="ReportDataSourceResolver"/> class
        /// </summary>
        /// <param name="reportDataSources"></param>
        public ReportDataSourceResolver(IEnumerable<IReportDatasource> reportDataSources)
        {
            _reportDataSources = reportDataSources;
        }
        /// <summary>
        /// Resolves data source by its name
        /// </summary>
        /// <param name="name">The data source name</param>
        public IReportDatasource Resolve(string name)
        {
            try
            {
                return _reportDataSources.First(x => x.Name == name);
            }
            catch (InvalidOperationException e)
            {
                throw new KeyNotFoundException($"Report Data Source for the grid name '{name}' was not found.", e);
            }
        }
    }
}
