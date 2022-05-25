using System;
using AutoMapper;
using FilingPortal.Parts.Rail.Export.Domain.Dtos;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Domain.Import.Inbound;

namespace FilingPortal.Parts.Rail.Export.Domain.Mapping
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
            CreateMap<InboundImportModel, InboundRecord>()
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.FilingHeaders, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.Containers, opt => opt.Ignore());

            CreateMap<InboundImportModel, InboundRecordContainer>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.InboundRecordId, opt => opt.Ignore())
                .ForMember(x => x.InboundRecord, opt => opt.Ignore())
                .ForMember(x => x.Type, opt => opt.Ignore())
                ;

            CreateMap<Import.RuleConsignee.ImportModel, RuleConsignee>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                ;
            CreateMap<Import.RuleExportConsignee.ImportModel, RuleExporterConsignee>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                ;

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
        }
    }
}