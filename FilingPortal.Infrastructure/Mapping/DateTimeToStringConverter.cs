using System;
using System.Globalization;
using AutoMapper;
using FilingPortal.Infrastructure.Common;

namespace FilingPortal.Infrastructure.Mapping
{
    public class DateTimeToStringConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.ToString(FormatsContainer.UI_DATETIME_FORMAT, CultureInfo.InvariantCulture);
        }
    }
}