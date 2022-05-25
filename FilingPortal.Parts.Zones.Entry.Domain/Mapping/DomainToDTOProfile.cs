using AutoMapper;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Domain.Reporting.RuleImporter;
using FilingPortal.Parts.Zones.Entry.Domain.Reporting.Inbound;

namespace FilingPortal.Parts.Zones.Entry.Domain.Mapping
{
    /// <summary>
    /// Class describing mapping of the DTO to the domain entities
    /// </summary>
    public class DomainToDTOProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the domain entities to the DTO models
        /// </summary>
        public DomainToDTOProfile()
        {
            CreateMap<RuleImporter, RuleImporterReportModel>()
                .ForMember(x => x.ImporterCode, opt => opt.MapFrom(s => s.Importer.ClientCode));
            CreateMap<InboundReadModel, InboundReportModel>();
        }
    }
}
