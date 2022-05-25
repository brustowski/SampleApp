using AutoMapper;
using FilingPortal.Domain.Entities.TruckExport;

namespace FilingPortal.Domain.Mapping
{
    /// <summary>
    /// Class describing mapping of the dto to the domain entities
    /// </summary>
    public class LocalProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the dto to the domain entities
        /// </summary>
        public LocalProfile()
        {
            //CreateMap<TruckExportUpdateRecord, TruckExportRecord>()
            //    .ForMember(x => x.GrossWeightUOM, opt => opt.MapFrom(x => x.GrossWeightUnit))
            //    .ForMember(x => x.ModifiedUser, opt => opt.MapFrom(x => x.CreatedUser))
            //    .ForMember(x => x.ModifiedDate, opt => opt.Ignore())
            //    .ForMember(x => x.Documents, opt => opt.Ignore())
            //    .ForMember(x => x.Deleted, opt => opt.UseValue(false))
            //    .ForMember(x => x.FilingHeaders, opt => opt.Ignore())
            //    ;
        }
    }
}