using AutoMapper;
using FilingPortal.Domain.Enums;
using FilingPortal.Infrastructure.Mapping;
using System;
using System.IO;
using System.Linq.Expressions;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Web.Mapping.Converters
{

    /// <summary>
    /// Provides Converters configuration for AutoMapper
    /// </summary>
    public class ConvertersProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertersProfile"/> class.
        /// </summary>
        public ConvertersProfile()
        {
            CreateMap<long, MappingStatus?>().ConvertUsing<Int64ToMappingStatusConverter>();
            CreateMap<long, FilingStatus?>().ConvertUsing<Int64ToFilingStatusConverter>();
            CreateMap<long, JobStatus?>().ConvertUsing<Int64ToJobStatusConverter>();
            CreateMap<long, MappingStatus>().ConvertUsing<LongToMappingStatusConverter>();
            CreateMap<long, FilingStatus>().ConvertUsing<LongToFilingStatusConverter>();
            CreateMap<long, JobStatus>().ConvertUsing<LongToJobStatusConverter>();
            CreateMap<long, FreightType>().ConvertUsing<LongToEnumConverter<FreightType>>();
            CreateMap<long, FreightType?>().ConvertUsing<LongToNullableEnumConverter<FreightType>>();
            CreateMap<string, byte?>().ConvertUsing<StringToNullableByteConverter>();
            CreateMap<string, int?>().ConvertUsing<StringToNullableIntConverter>();
            CreateMap<string, decimal?>().ConvertUsing<StringToNullableDecimalConverter>();
            CreateMap<string, decimal>().ConvertUsing<StringToDecimalConverter>();
            CreateMap<int?, string>().ConvertUsing<NullableIntToStringConverter>();
            CreateMap<decimal?, string>().ConvertUsing<NullableDecimalToStringConverter>();
            CreateMap<decimal, string>().ConvertUsing<DecimalToStringConverter>();
            CreateMap<DateTime, string>().ConvertUsing(new DateTimeToStringConverter());
            CreateMap<DateTime?, string>().ConvertUsing(new NullableDateTimeToStringConverter());
            CreateMap<string, DateTime>().ConvertUsing(new StringToDateTimeConverter());
            CreateMap<string, DateTime?>().ConvertUsing(new StringToNulableDateTimeConverter());
            CreateMap<Stream, byte[]>().ConvertUsing(new StreamToByteArrayConverter());
            CreateMap<byte, bool>().ConvertUsing<ByteToBooleanConverter>();
            CreateMap<string, Guid?>().ConvertUsing<StringToNullableGuidConverter>();
            CreateMap<string, Guid>().ConvertUsing<StringToGuidConverter>();
            CreateMap<string, AppAddress>().ConvertUsing<StringToAddressConverter>();

            CreateMap<string, string>().ProjectUsing(_cleanupUnicode);
        }

        // CBDEV-2560: Incorrect degree sign is replaced with correct one on each string to string conversions.
        // Please note that it may cause performance issues or even incorrect system behaviour if unicode degree symbol is required in repository data
        private readonly Expression<Func<string, string>> _cleanupUnicode =
            source => source != null ? source.Replace("º", "°") : null;
    }
}
