using System.Collections.Generic;
using FilingPortal.Domain.Common.Reporting.Model;
using Framework.Domain.Paging;

namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Provides information about applied filters for the report
    /// </summary>
    public interface IReportFiltersBuilder
    {
        /// <summary>
        /// Gets a set of applied filters as rows for the report
        /// </summary>
        /// <param name="filters">A set of <see cref="Filter"/> objects</param>
        IEnumerable<Row> GetRows(IEnumerable<Filter> filterSettingsFilters);
    }
}
