using FilingPortal.Domain.Services.GridExport.Formatters;
using System;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    class ColumnMapInfo<T> : IColumnMapInfo
    {
        public ColumnMapInfo(string title, Func<T, object> getter, Type fieldType)
        {
            Title = title;
            Getter = getter;
            FieldType = fieldType;
        }

        public string Title { get; set; }

        Func<object, object> IColumnMapInfo.Getter
        {
            get { return o => Getter((T)o); }
        }
        public IValueFormatter ValueFormatter { get; set; }

        public bool IsValueFormatterSet => ValueFormatter != null;

        public Func<T, object> Getter { get; }

        public Type FieldType { get; set; }

        public string GetFormattedValue(object obj)
        {
            var value = Getter((T)obj);
            return ValueFormatter.Format(value);
        }
    }
}
