using AutoMapper;
using FilingPortal.Parts.Recon.Domain.Enums;

namespace FilingPortal.Parts.Recon.Web.Mappings.Converters
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
            CreateMap<long, FtaReconStatusValue>().ConvertUsing<Int64ToFtaReconStatusConverter>();
            CreateMap<long, ValueReconStatusValue>().ConvertUsing<Int64ToValueReconStatusConverter>();
        }
    }
}