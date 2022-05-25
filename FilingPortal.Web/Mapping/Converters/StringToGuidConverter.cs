using System;
using AutoMapper;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter from string to Guid
    /// </summary>
    public class StringToGuidConverter : ITypeConverter<string, Guid>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public Guid Convert(string source, Guid destination, ResolutionContext context)
        {
            return Guid.Parse(source);
        }
    }
}