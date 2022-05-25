using System;
using AutoMapper;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter for MappingStatus from long
    /// </summary>
    public class Int64ToMappingStatusConverter : ITypeConverter<Int64, MappingStatus?>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public MappingStatus? Convert(long source, MappingStatus? destination, ResolutionContext context)
        {
            return (MappingStatus?) Enum.ToObject(typeof(MappingStatus), source);
        }
    }
}