using AutoMapper;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter from string to nullable int
    /// </summary>
    public class NullableIntToStringConverter : ITypeConverter<int?, string>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public string Convert(int? source, string destination, ResolutionContext context)
        {
            return source.HasValue ? source.Value.ToString() : string.Empty;
        }
    }
}