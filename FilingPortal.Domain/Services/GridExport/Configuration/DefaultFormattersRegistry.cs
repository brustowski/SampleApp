using FilingPortal.Domain.Services.GridExport.Formatters;
using System;
using System.Collections.Generic;

namespace FilingPortal.Domain.Services.GridExport.Configuration
{
    internal class DefaultFormattersRegistry : IDefaultFormattersRegistry
    {
        private readonly Dictionary<Type, IValueFormatter> _typedFormatters;

        public DefaultFormattersRegistry()
        {
            _typedFormatters = new Dictionary<Type, IValueFormatter>
            {
                {typeof(bool), new BoolFormatter()},
                {typeof(bool?), new BoolNullableFormatter()},
                {typeof(DateTime), new DateTimeFormatter()},
                {typeof(DateTime?), new DateTimeNullableFormatter()},
                {typeof(decimal), new DecimalFormatter()},
                {typeof(decimal?), new DecimalNullableFormatter()},
            };
        }

        public IValueFormatter Get(Type type)
        {
            return _typedFormatters.ContainsKey(type) ? _typedFormatters[type] : null;
        }
    }
}
