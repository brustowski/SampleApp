using AutoMapper;
using FilingPortal.Parts.Inbond.Domain.DTOs;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Web.Models;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using System.IO;

namespace FilingPortal.Parts.Inbond.Web.Mappings
{
    /// <summary>
    /// Class describing mapping of the view models to the domain entities
    /// </summary>
    public class ViewModelToDomainProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelToDomainProfile"/> class
        /// with all mappings of the view models to the domain entities
        /// </summary>
        public ViewModelToDomainProfile()
        {
            CreateMap<RuleEntryEditModel, RuleEntry>()
                .ForMember(d => d.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.FirmsCode, opt => opt.Ignore())
                .ForMember(d => d.Importer, opt => opt.Ignore())
                .ForMember(d => d.ImporterAddress, opt => opt.Ignore())
                .ForMember(d => d.Consignee, opt => opt.Ignore())
                .ForMember(d => d.ConsigneeAddress, opt => opt.Ignore())
                .ForMember(d => d.Shipper, opt => opt.Ignore());

            CreateMap<DefValuesEditModel, DefValue>()
                .ForMember(r => r.ColumnName, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(r => r.Description, opt => opt.MapFrom(s => s.ValueDesc))
                .ForMember(r => r.Label, opt => opt.MapFrom(s => s.ValueLabel))
                .ForMember(r => r.SectionId, opt => opt.Ignore())
                .ForMember(r => r.Section, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<InboundRecordDocumentEditModel, DocumentDto>()
                .ForMember(x => x.DocumentType, x => x.MapFrom(c => c.Type))
                .ForMember(x => x.FileDesc, x => x.MapFrom(c => c.Description))
                .ForMember(x => x.FileContent, x => x.MapFrom(c => c.Data.InputStream))
                .ForMember(x => x.FileName, x => x.MapFrom(c => c.Data.FileName))
                .ForMember(x => x.FileExtension, x => x.MapFrom(c => Path.GetExtension(c.Data.FileName)))
                .ForMember(x => x.FilingHeadersFk, x => x.Ignore());
        }
    }
}