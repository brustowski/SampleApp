using AutoMapper;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter from string to nullable int
    /// </summary>
    public class StringToNullableIntConverter : ITypeConverter<string, int?>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public int? Convert(string source, int? destination, ResolutionContext context)
        {
            return int.TryParse(source, out int result) ? (int?)result : null;
        }
    }
}