using AutoMapper;
using System.Globalization;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter from string to nullable decimal
    /// </summary>
    public class StringToDecimalConverter : ITypeConverter<string, decimal>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public decimal Convert(string source, decimal destination, ResolutionContext context)
        {
            return decimal.Parse(source, NumberStyles.Any, CultureInfo.InvariantCulture);
        }
    }
}