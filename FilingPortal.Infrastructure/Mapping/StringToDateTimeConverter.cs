using AutoMapper;
using FilingPortal.Infrastructure.Common;
using System;
using System.Globalization;

namespace FilingPortal.Infrastructure.Mapping
{
    public class StringToDateTimeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            if (DateTime.TryParseExact(source, FormatsContainer.UI_DATETIME_FORMAT, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime result)) return result;


            if (DateTime.TryParseExact(source, FormatsContainer.UI_DATETIME_FORMAT_SHORT_YEAR, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out result))
                return result;

            throw new FormatException(
                $"Wrong date format. Please provide date in {FormatsContainer.UI_DATETIME_FORMAT} or {FormatsContainer.UI_DATETIME_FORMAT_SHORT_YEAR} format");
        }
    }
}