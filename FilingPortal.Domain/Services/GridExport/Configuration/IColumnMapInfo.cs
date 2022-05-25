using FilingPortal.Domain.Services.GridExport.Formatters;
using System;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    public interface IColumnMapInfo
    {
        string Title { get; set; }
        Func<object, object> Getter { get; }
        IValueFormatter ValueFormatter { get; set; }
        bool IsValueFormatterSet { get; }
        Type FieldType { get; set; }
        string GetFormattedValue(object obj);
    }
}
