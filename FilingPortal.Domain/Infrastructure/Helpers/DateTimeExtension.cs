using System;
using System.Globalization;
using FilingPortal.Domain.Mapping;

namespace FilingPortal.Domain.Infrastructure.Helpers
{
    public static class DateTimeExtension
    {
        public static string ToUsFormat(this DateTime dt)
        {
            return dt.ToString(FormatsContainer.US_DATETIME_FORMAT, CultureInfo.InvariantCulture);
        }
    }
}
