using System.Globalization;
using Newtonsoft.Json.Converters;

namespace FilingPortal.PluginEngine.Common.Json
{
    /// <summary>
    /// JSON datetime converter
    /// </summary>
    public class DateFormatConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Creates a new instance of <see cref="DateFormatConverter"/>
        /// </summary>
        /// <param name="format">Date and time format</param>
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
            Culture = CultureInfo.InvariantCulture;
        }
    }
}