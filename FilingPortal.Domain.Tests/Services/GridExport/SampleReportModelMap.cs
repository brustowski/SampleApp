using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;

namespace FilingPortal.Domain.Tests.Services.GridExport
{
    class SampleReportModelMap : ReportModelMap<SampleReportModel>, IReportModelMap
    {
        public SampleReportModelMap()
        {
            Map(x => x.OverridenName).ColumnTitle("Name Overriden").UseFormatter<DecimalFormatter>();
            Map(x => x.OverridenValue).ColumnTitle("Value Overriden").UseFormatter<DecimalFormatter>();
            Ignore(x => x.Ignored);
        }
    }
}
