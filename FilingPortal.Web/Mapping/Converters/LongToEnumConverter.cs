using System;
using AutoMapper;

namespace FilingPortal.Web.Mapping.Converters
{
    /// <summary>
    /// Converter to enum from long
    /// </summary>
    public class LongToEnumConverter<TEnum> : ITypeConverter<long, TEnum>
    where TEnum : struct, IConvertible
    {
        /// <summary>
        /// Performs conversion from source to destination type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="destination">Destination object</param>
        /// <param name="context">Resolution context</param>
        public TEnum Convert(long source, TEnum destination, ResolutionContext context)
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), source);
        }
    }
}