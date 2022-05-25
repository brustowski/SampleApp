using FilingPortal.Domain.Common.Reporting.Model;
using System.Collections.Generic;
using FilingPortal.Domain.Services.GridExport.Configuration;

namespace FilingPortal.Domain.Common.Reporting
{
    public interface IReportBodyBuilder
    {
        IEnumerable<Row> GetRows<T>(IEnumerable<T> models) where T : class;
        IEnumerable<Row> GetRows<T>(IEnumerable<IColumnMapInfo> columns, IEnumerable<T> models) where T : class;
        Row GetHeaderRow<T>() where T : class;

        Row GetHeaderRow(IEnumerable<IColumnMapInfo> columns);
    }
}
