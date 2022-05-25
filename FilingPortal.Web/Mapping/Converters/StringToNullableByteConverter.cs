using AutoMapper;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter from string to nullable byte
    /// </summary>
    public class StringToNullableByteConverter : ITypeConverter<string, byte?>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public byte? Convert(string source, byte? destination, ResolutionContext context)
        {
            return byte.TryParse(source, out byte result) ? (byte?)result : null;
        }
    }
}