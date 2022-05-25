using AutoMapper;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Enums;
using FilingPortal.Parts.Recon.Web.Models;

namespace FilingPortal.Parts.Recon.Web.Mappings
{
    /// <summary>
    /// Class describing mapping of the domain entities to the view models used in the presentation layer
    /// </summary>
    public class DomainToViewModelProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToViewModelProfile"/> class
        /// with all mappings of the domain entities to the view models
        /// </summary>
        public DomainToViewModelProfile()
        {
            CreateMap<InboundRecord, InboundRecordViewModel>()
                .ForMember(x => x.Actions, opt => opt.Ignore())
                .ForMember(x => x.HighlightingType, opt => opt.Ignore());

            CreateMap<InboundRecordReadModel, InboundReadModelViewModel>()
                .ForMember(d => d.AceReconIssue, opt => opt.MapFrom(s => s.AceReconIndicator))
                .ForMember(d => d.MismatchReconIssue, opt => opt.MapFrom(s => s.MismatchReconValueFlag))
                .ForMember(d => d.AceNaftaRecon, opt => opt.MapFrom(s => s.AceNaftaReconIndicator))
                .ForMember(d => d.MismatchNaftaRecon, opt => opt.MapFrom(s => s.MismatchReconFtaFlag))
                .ForMember(d => d.AceLineEnteredValue, opt => opt.MapFrom(s => s.AceLineGoodsValueAmount))
                .ForMember(d => d.MismatchLineEnteredValue, opt => opt.MapFrom(s => s.MismatchEntryValue))
                .ForMember(d => d.AceDuty, opt => opt.MapFrom(s => s.AceLineDutyAmount))
                .ForMember(d => d.MismatchDuty, opt => opt.MapFrom(s => s.MismatchDuty))
                .ForMember(d => d.AceMpf, opt => opt.MapFrom(s => s.AceLineMpfAmount))
                .ForMember(d => d.MismatchMpf, opt => opt.MapFrom(s => s.MismatchMpf))
                .ForMember(d => d.AceCustomsQty1, opt => opt.MapFrom(s => s.AceLineTariffQuantity))
                .ForMember(d => d.MismatchCustomsQty1, opt => opt.MapFrom(s => s.MismatchQuantity))
                .ForMember(d => d.AceTariff, opt => opt.MapFrom(s => s.AceHtsNumberFull))
                .ForMember(d => d.MismatchTariff, opt => opt.MapFrom(s => s.MismatchHts))
                .ForMember(d => d.AcePayableMpf, opt => opt.MapFrom(s => s.AceTotalPaidMpfAmount))
                .ForMember(d => d.MismatchPayableMpf, opt => opt.MapFrom(s => s.MismatchPayableMpf))
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Actions, opt => opt.Ignore())
                .ForMember(x => x.HighlightingType, opt => opt.Ignore());

            CreateMap<FtaReconReadModel, FtaReconViewModel>()
                .ForMember(d=> d.StatusTitle, opt => opt.MapFrom(s=>s.StatusName))
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore());

            CreateMap<ValueReconReadModel, ValueReconViewModel>()
                .ForMember(d => d.StatusTitle, opt => opt.MapFrom(s => s.StatusName))
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Actions, opt => opt.Ignore())
                .ForMember(x => x.HighlightingType, opt => opt.Ignore());
        }
    }
}