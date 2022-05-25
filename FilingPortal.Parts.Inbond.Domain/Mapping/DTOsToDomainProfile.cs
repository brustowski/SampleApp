using System;
using AutoMapper;
using FilingPortal.Domain.Mapping.Converters;
using FilingPortal.Parts.Inbond.Domain.DTOs;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.Domain.Mapping
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
            CreateMap<Import.Inbound.ImportModel, InboundRecord>()
                .ForMember(x => x.FirmsCodeId, opt => opt.ResolveUsing<FirmsIdByCodeResolver, string>(x => x.FirmsCode))
                .ForMember(x => x.FirmsCode, opt => opt.Ignore())
                .ForMember(x => x.ImporterId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.ImporterCode))
                .ForMember(x => x.Importer, opt => opt.Ignore())
                .ForMember(x => x.ConsigneeId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.ConsigneeCode))
                .ForMember(x => x.Consignee, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.FilingHeaders, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.EntryDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore());

            CreateMap<DocumentDto, Document>()
                .ForMember(x => x.Content, opt => opt.MapFrom(s => s.FileContent))
                .ForMember(x => x.Description, opt => opt.MapFrom(s => s.FileDesc))
                .ForMember(x => x.Extension, opt => opt.MapFrom(s => s.FileExtension))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.InboundRecordId, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.FilingHeaderId, opt => opt.Ignore())
                .ForMember(x => x.FilingHeader, opt => opt.Ignore());

            CreateMap<Import.RuleEntry.ImportModel, RuleEntry>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.FirmsCodeId, opt => opt.ResolveUsing<FirmsIdByCodeResolver, string>(x => x.FirmsCode))
                .ForMember(d => d.FirmsCode, opt => opt.Ignore())
                .ForMember(d => d.ImporterId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Importer))
                .ForMember(d => d.Importer, opt => opt.Ignore())
                .ForMember(d => d.ImporterAddressId,
                    opt => opt.ResolveUsing<ClientAddressIdByCodeNullableResolver, string>(s => s.ImporterAddress))
                .ForMember(d => d.ImporterAddress, opt => opt.Ignore())
                .ForMember(d => d.ConsigneeId,
                    opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Consignee))
                .ForMember(d => d.Consignee, opt => opt.Ignore())
                .ForMember(d => d.ConsigneeAddressId,
                    opt => opt.ResolveUsing<ClientAddressIdByCodeNullableResolver, string>(s => s.ConsigneeAddress))
                .ForMember(d => d.ConsigneeAddress, opt => opt.Ignore())
                .ForMember(d => d.ShipperId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Shipper))
                .ForMember(d => d.Shipper, opt => opt.Ignore())
                .ForMember(d => d.ConfirmationNeeded, opt => opt.ResolveUsing<ImportStringToBoolResolver, string>(x => x.ConfirmationNeeded))
                ;
        }
    }
}