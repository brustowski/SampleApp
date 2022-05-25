using AutoMapper;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.DataLayer.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Mapping
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
