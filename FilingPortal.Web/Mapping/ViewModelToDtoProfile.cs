using System.IO;
using System.Linq;
using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.DTOs.Truck;
using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.DTOs.Vessel;
using FilingPortal.Domain.DTOs.VesselExport;
using FilingPortal.Domain.Enums;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.Web.Models;

namespace FilingPortal.Web.Mapping
{
    /// <summary>
    /// Class describing mapping of the view models to the dtos
    /// </summary>
    public class ViewModelToDtoProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelToDomainProfile"/> class
        /// with all mappings of the view models to the domain entities
        /// </summary>
        public ViewModelToDtoProfile()
        {
            CreateMap<InboundRecordDocumentEditModel, RailDocumentDto>()
                .ForMember(x => x.IsManifest, x => x.Ignore())//TODO: CBDEV-849
                .ForMember(x => x.DocumentType, x => x.MapFrom(c => c.Type))
                .ForMember(x => x.FileDesc, x => x.MapFrom(c => c.Description))
                .ForMember(x => x.FileContent, x => x.MapFrom(c => c.Data.InputStream))
                .ForMember(x => x.FileName, x => x.MapFrom(c => c.Data.FileName))
                .ForMember(x => x.FileExtension, x => x.MapFrom(c => Path.GetExtension(c.Data.FileName)))
                .ForMember(x => x.FilingHeadersFk, x => x.Ignore())
                ;

            CreateMap<InboundRecordDocumentEditModel, TruckDocumentDto>()
                .ForMember(x => x.DocumentType, x => x.MapFrom(c => c.Type))
                .ForMember(x => x.FileDesc, x => x.MapFrom(c => c.Description))
                .ForMember(x => x.FileContent, x => x.MapFrom(c => c.Data.InputStream))
                .ForMember(x => x.FileName, x => x.MapFrom(c => c.Data.FileName))
                .ForMember(x => x.FileExtension, x => x.MapFrom(c => Path.GetExtension(c.Data.FileName)))
                .ForMember(x => x.FilingHeadersFk, x => x.Ignore())
                ;

            CreateMap<InboundRecordDocumentEditModel, PipelineDocumentDto>()
                .ForMember(x => x.DocumentType, x => x.MapFrom(c => c.Type))
                .ForMember(x => x.FileDesc, x => x.MapFrom(c => c.Description))
                .ForMember(x => x.FileContent, x => x.MapFrom(c => c.Data.InputStream))
                .ForMember(x => x.FileName, x => x.MapFrom(c => c.Data.FileName))
                .ForMember(x => x.FileExtension, x => x.MapFrom(c => Path.GetExtension(c.Data.FileName)))
                .ForMember(x => x.FilingHeadersFk, x => x.Ignore())
                ;
            // todo: use base class mapping instead
            CreateMap<InboundRecordDocumentEditModel, TruckExportDocumentDto>()
                .ForMember(x => x.DocumentType, x => x.MapFrom(c => c.Type))
                .ForMember(x => x.FileDesc, x => x.MapFrom(c => c.Description))
                .ForMember(x => x.FileContent, x => x.MapFrom(c => c.Data.InputStream))
                .ForMember(x => x.FileName, x => x.MapFrom(c => c.Data.FileName))
                .ForMember(x => x.FileExtension, x => x.MapFrom(c => Path.GetExtension(c.Data.FileName)))
                .ForMember(x => x.FilingHeadersFk, x => x.Ignore())
                ;
            CreateMap<InboundRecordDocumentEditModel, VesselDocumentDto>()
                .ForMember(x => x.DocumentType, x => x.MapFrom(c => c.Type))
                .ForMember(x => x.FileDesc, x => x.MapFrom(c => c.Description))
                .ForMember(x => x.FileContent, x => x.MapFrom(c => c.Data.InputStream))
                .ForMember(x => x.FileName, x => x.MapFrom(c => c.Data.FileName))
                .ForMember(x => x.FileExtension, x => x.MapFrom(c => Path.GetExtension(c.Data.FileName)))
                .ForMember(x => x.FilingHeadersFk, x => x.Ignore())
                ;
            CreateMap<InboundRecordDocumentEditModel, VesselExportDocumentDto>()
                .ForMember(x => x.DocumentType, x => x.MapFrom(c => c.Type))
                .ForMember(x => x.FileDesc, x => x.MapFrom(c => c.Description))
                .ForMember(x => x.FileContent, x => x.MapFrom(c => c.Data.InputStream))
                .ForMember(x => x.FileName, x => x.MapFrom(c => c.Data.FileName))
                .ForMember(x => x.FileExtension, x => x.MapFrom(c => Path.GetExtension(c.Data.FileName)))
                .ForMember(x => x.FilingHeadersFk, x => x.Ignore())
                ;
            CreateMap<HttpPostedData, BaseDocumentDto>()
                .Include<HttpPostedData, PipelineDocumentDto>()
                .Include<HttpPostedData, TruckExportDocumentDto>()
                .ForMember(x => x.DocumentType, opt => opt.MapFrom(x => x.Fields["docType"].Value))
                .ForMember(x => x.FileDesc, opt => opt.MapFrom(x => x.Fields["description"].Value))
                .ForMember(x => x.FileContent, opt => opt.MapFrom(x => File.ReadAllBytes(x.Files.First().Value.Path)))
                .ForMember(x => x.FileName, opt => opt.MapFrom(x => x.Files.First().Value.Name))
                .ForMember(x => x.FileExtension, opt => opt.MapFrom(x => Path.GetExtension(x.Files.First().Value.Name)))
                .ForMember(x => x.Status, opt => opt.UseValue(InboundRecordDocumentStatus.New))
                .ForMember(x => x.FilingHeadersFk, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.UseValue(0))
                ;
            CreateMap<HttpPostedData, PipelineDocumentDto>();
            CreateMap<HttpPostedData, TruckExportDocumentDto>();

        }
    }
}