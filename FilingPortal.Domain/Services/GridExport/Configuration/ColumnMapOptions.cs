using FilingPortal.Domain.Services.GridExport.Formatters;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    class ColumnMapOptions : IColumnMapOptions
    {
        private readonly IColumnMapInfo _columnMapInfo;
        public ColumnMapOptions(IColumnMapInfo columnMapInfo)
        {
            _columnMapInfo = columnMapInfo;
        }

        public IColumnMapOptions UseFormatter<TFormatter>() where TFormatter : IValueFormatter, new()
        {
            _columnMapInfo.ValueFormatter = new TFormatter();
            return this;
        }

        public IColumnMapOptions ColumnTitle(string title)
        {
            _columnMapInfo.Title = title;
            return this;
        }
    }
}
