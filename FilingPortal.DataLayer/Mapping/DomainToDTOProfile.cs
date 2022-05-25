using AutoMapper;
using FilingPortal.Parts.Common.DataLayer.Entities;
using FilingPortal.Parts.Common.Domain.DTOs;

namespace FilingPortal.DataLayer.Mapping
{
    /// <summary>
    /// Class describing mapping of the domain entities to the view models used in the presentation layer
    /// </summary>
    public class DomainToDTOProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the domain entities to the view models
        /// </summary>
        public DomainToDTOProfile()
        {
            CreateMap<ImportFormParameter, ImportField>()
                .ForMember(x => x.Value, opt => opt.ResolveUsing(s => string.IsNullOrWhiteSpace(s.Value) ? null : s.Value));
            CreateMap<InboundRecordParameter, UpdateField>()
                .ForMember(x => x.Value, opt => opt.ResolveUsing(s => string.IsNullOrWhiteSpace(s.Value) ? null : s.Value));
        }
    }
}