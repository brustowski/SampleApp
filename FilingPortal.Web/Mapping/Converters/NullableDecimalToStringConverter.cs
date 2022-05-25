using AutoMapper;
using FilingPortal.Domain.Infrastructure.Helpers;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter from string to nullable decimal
    /// </summary>
    public class NullableDecimalToStringConverter : ITypeConverter<decimal?, string>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public string Convert(decimal? source, string destination, ResolutionContext context)
        {
            return source.HasValue ? source.Value.WithoutTrailingZeroes() : string.Empty;
        }
    }
}