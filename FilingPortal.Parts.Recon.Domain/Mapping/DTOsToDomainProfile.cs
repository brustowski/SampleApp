using AutoMapper;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Import.AceReport;

namespace FilingPortal.Parts.Recon.Domain.Mapping
{
    /// <summary>
    /// Class describing mapping of the domain entities to the view models used in the presentation layer
    /// </summary>
    public class DTOsToDomainProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DTOsToDomainProfile"/> class
        /// with all mappings of the domain entities to the view models
        /// </summary>
        public DTOsToDomainProfile()
        {
            CreateMap<AceReportImportModel, AceReportRecord>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                ;
            CreateMap<Import.FtaRecon.Model, FtaRecon>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.CreatedUser, opt => opt.Ignore())
                ;

            CreateMap<Import.ValueRecon.Model, ValueRecon>()
                .ForMember(d => d.FinalTotalValue, opt => opt.MapFrom(s => s.FinalUnitPrice * s.CustomsQty1 ?? s.FinalTotalValue))
                .ForMember(d => d.FinalUnitPrice, opt => opt.MapFrom(s => s.FinalUnitPrice))
                .ForMember(d => d.ClientNote, opt => opt.MapFrom(s => s.ClientNote))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}