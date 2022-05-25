using System.IO;
using AutoMapper;

namespace FilingPortal.Infrastructure.Mapping
{
    /// <summary>
    /// Converter for Stream to Byte Array conversion used in mapping
    /// </summary>
    public class StreamToByteArrayConverter : ITypeConverter<Stream, byte[]>
    {
        /// <summary>
        /// Performs conversion from source to destination type using resolution context of the mapper
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public byte[] Convert(Stream source, byte[] destination, ResolutionContext context)
        {
            if (source == null)
                return new byte[0];

            byte[] buffer = new byte[source.Length];
            source.Seek(0, SeekOrigin.Begin);
            source.Read(buffer, 0, System.Convert.ToInt32(source.Length));
            return buffer;
        }
    }
}