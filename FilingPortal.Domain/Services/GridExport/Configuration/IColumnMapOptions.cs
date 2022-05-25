using FilingPortal.Domain.Services.GridExport.Formatters;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    public interface IColumnMapOptions
    {
        IColumnMapOptions UseFormatter<TFormatter>() where TFormatter : IValueFormatter, new();
        IColumnMapOptions ColumnTitle(string title);
    }
}
