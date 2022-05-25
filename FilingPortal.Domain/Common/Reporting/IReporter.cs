using FilingPortal.Domain.Common.Reporting.Model;
using System.Collections.Generic;

namespace FilingPortal.Domain.Common.Reporting
{
    /// <summary>
    /// Describes methods to work with Report
    /// </summary>
    public interface IReporter
    {
        /// <summary>
        /// Adds new section to the report
        /// </summary>
        /// <param name="name">The section name</param>
        void AddSection(string name);
        
        /// <summary>
        /// Add header to the report
        /// </summary>
        /// <param name="headerRow">Header row</param>
        /// <param name="enableFilters">Enable filters</param>
        void AddHeader(Row headerRow, bool enableFilters = false);
        
        /// <summary>
        /// Add part of the data
        /// </summary>
        /// <param name="rows">Collection of rows to add</param>
        void AddPartOfData(IEnumerable<Row> rows);

        /// <summary>
        /// Save report ot file
        /// </summary>
        string SaveToFile();
    }
}
