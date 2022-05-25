using System;
using System.Globalization;
using AutoMapper;
using FilingPortal.Infrastructure.Common;

namespace FilingPortal.Infrastructure.Mapping
{
    public class StringToNulableDateTimeConverter : ITypeConverter<string, DateTime?>
    {
        /// <summary>
        /// Performs conversion from source to destination type using resolution context of the mapper
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public DateTime? Convert(string source, DateTime? destination, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source))
            {
                if (DateTime.TryParseExact(source, FormatsContainer.UI_DATETIME_FORMAT, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime result)) return result;


                if (DateTime.TryParseExact(source, FormatsContainer.UI_DATETIME_FORMAT_SHORT_YEAR, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out result))
                    return result;
            }
            return null;
        }
    }
}