using System;
using AutoMapper;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter for FilingStatus from long
    /// </summary>
    public class LongToFilingStatusConverter : ITypeConverter<long, FilingStatus>
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public FilingStatus Convert(long source, FilingStatus destination, ResolutionContext context)
        {
            return (FilingStatus)Enum.ToObject(typeof(FilingStatus), source);
        }
    }
}