using AutoMapper;
using FilingPortal.Parts.CanadaTruckImport.Domain.Dtos;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RulePort;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleProduct;
using FilingPortal.Parts.CanadaTruckImport.Web.Models.RuleVendor;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using System.IO;

namespace FilingPortal.Parts.CanadaTruckImport.Web.Mappings
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
            CreateMap<RuleVendorEditModel, RuleVendor>()
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.Vendor, opt => opt.Ignore())
                .ForMember(x => x.Importer, opt => opt.Ignore())
                .ForMember(x => x.Exporter, opt => opt.Ignore());

            CreateMap<RulePortEditModel, RulePort>()
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore());
            CreateMap<RuleProductEditModel, RuleProduct>()
                .ForMember(x => x.ProductCode, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore());

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
                .ForMember(x => x.FilingHeadersFk, x => x.Ignore())
                ;
        }
    }
}