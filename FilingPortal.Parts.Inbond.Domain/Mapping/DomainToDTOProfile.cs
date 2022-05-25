using AutoMapper;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Reporting.RuleEntry;

namespace FilingPortal.Parts.Inbond.Domain.Mapping
{
    public class DomainToDTOProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the domain entities to the DTO models
        /// </summary>
        public DomainToDTOProfile()
        {
            CreateMap<RuleEntry, RuleEntryReportModel>()
                .ForMember(x => x.FirmsCode, opt => opt.MapFrom(x => x.FirmsCode.FirmsCode))
                .ForMember(x => x.Importer, opt => opt.MapFrom(x => x.Importer.ClientCode))
                .ForMember(x => x.ImporterAddress, opt => opt.MapFrom(x => x.ImporterAddress != null ? x.ImporterAddress.Code : null))
                .ForMember(x => x.Consignee, opt => opt.MapFrom(x => x.Consignee.ClientCode))
                .ForMember(x => x.ConsigneeAddress, opt => opt.MapFrom(x => x.ConsigneeAddress != null ? x.ConsigneeAddress.Code : null))
                .ForMember(x => x.Shipper, opt => opt.MapFrom(x => x.Shipper.ClientCode))
                .ForMember(x=>x.ConfirmationNeeded, opt => opt.MapFrom(x=>x.ConfirmationNeeded ? 1 : 0));
        }
    }
}
