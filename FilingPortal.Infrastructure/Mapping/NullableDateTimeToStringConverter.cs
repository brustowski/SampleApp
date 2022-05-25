using System;
using System.Globalization;
using AutoMapper;
using FilingPortal.Infrastructure.Common;

namespace FilingPortal.Infrastructure.Mapping
{
    /// <summary>
    /// Service for Nullable string to DateTime conversion used in mapping
    /// </summary>
    public class NullableDateTimeToStringConverter : ITypeConverter<DateTime?, string>
    {
        /// <summary>
        /// Performs conversion from source to destination type using resolution context of the mapper
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            if (source.HasValue) return source.Value.ToString(FormatsContainer.UI_DATETIME_FORMAT, CultureInfo.InvariantCulture);
            return string.Empty;
        }
    }
}