using AutoMapper;
using System.Globalization;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter from string to nullable decimal
    /// </summary>
    public class ByteToBooleanConverter : ITypeConverter<byte, bool>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public bool Convert(byte source, bool destination, ResolutionContext context)
        {
            return source > 0;
        }
    }
}