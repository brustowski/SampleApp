using Framework.Infrastructure.Extensions;
using FilingPortal.Domain.Common.Reporting.Model;
using Framework.Domain.Paging;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Common.Reporting;

namespace FilingPortal.Domain.Services.GridExport
{
    /// <summary>
    /// Provides information about applied filters for the report
    /// </summary>
    internal class ReportFiltersBuilder : IReportFiltersBuilder
    {
        /// <summary>
        /// Gets a set of applied filters as rows for the report
        /// </summary>
        /// <param name="filters">A set of <see cref="Filter"/> objects</param>
        public IEnumerable<Row> GetRows(IEnumerable<Filter> filters)
        {
            var rows = new List<Row>();

            if (!filters.SafeAny())
            {
                return rows;
            }

            rows.Add(new Row().CreateCell("Filters:"));
            rows.AddRange(
                filters.Select(
                    filter =>
                        new Row()
                            .CreateCell(GetFilterName(filter))
                            .CreateCell(filter.Values.FirstOrDefault()?.DisplayValue)
                )
            );
            rows.Add(new Row().SkipNColumns(1));
            return rows;
        }

        private string GetFilterName(IFilter filter)
        {
            switch (filter.Operand)
            {
                case FilterOperands.DateFrom:
                    return $"{filter.FieldName} (From)";

                case FilterOperands.DateTo:
                    return $"{filter.FieldName} (To)";
            }

            return filter.FieldName;
        }
    }
}
