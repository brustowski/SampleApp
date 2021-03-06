using AutoMapper;
using FilingPortal.Parts.Common.DataLayer.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Mapping
{
    /// <summary>
    /// Class describing mapping of the dto to the domain entities
    /// </summary>
    public class DtoToDomainProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DtoToDomainProfile"/> class
        /// with all mappings of the dto to the domain entities
        /// </summary>
        public DtoToDomainProfile()
        {
            CreateMap<FieldConfiguration, DefValueManualReadModel>()
                .ForMember(x => x.ModificationDate, opt => opt.Ignore());
        }
    }
}
