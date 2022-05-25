using System;
using System.Collections.Generic;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    public interface IReportModelMap
    {
        IEnumerable<IColumnMapInfo> GetColumnInfos();
        Type GetModelType { get; }
    }
}
