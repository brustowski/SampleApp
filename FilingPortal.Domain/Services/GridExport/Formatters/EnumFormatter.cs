using System;
using System.Collections.Generic;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Domain.Services.GridExport.Formatters
{
    internal class EnumFormatter<T> : IValueFormatter where T : struct
    {
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();
        public EnumFormatter()
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            foreach (Enum enumValue in Enum.GetValues(typeof(T)))
            {
                var description = enumValue.GetDescription();
                var value = string.IsNullOrEmpty(description) ? enumValue.ToString() : description;
                _values.Add(enumValue.ToString(), value);
            }
        }
        public string Format(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var enumValue = value.ToString();
            if (_values.ContainsKey(enumValue))
            {
                return _values[enumValue];
            }

            throw new KeyNotFoundException($"Enum value `{enumValue}` not found in `{typeof(T)}`.");
        }
    }
}