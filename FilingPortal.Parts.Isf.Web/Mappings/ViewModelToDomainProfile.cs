using System.IO;
using AutoMapper;
using FilingPortal.Parts.Isf.Domain.Dtos;
using FilingPortal.Parts.Isf.Domain.Entities;
using FilingPortal.Parts.Isf.Web.Models.AddInbound;
using FilingPortal.Parts.Isf.Web.Models.Inbound;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordModels;

namespace FilingPortal.Parts.Isf.Web.Mappings
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
            CreateMap<InboundRecordEditModel, InboundRecord>()
                .ForMember(x => x.Importer, opt => opt.Ignore())
                .ForMember(x => x.Buyer, opt => opt.Ignore())
                .ForMember(x => x.Consignee, opt => opt.Ignore())
                .ForMember(x => x.Seller, opt => opt.Ignore())
                .ForMember(x => x.ShipTo, opt => opt.Ignore())
                .ForMember(x => x.ContainerStuffingLocation, opt => opt.Ignore())
                .ForMember(x => x.Consolidator, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.Deleted, opt => opt.UseValue(false))
                .ForMember(x => x.FilingHeaders, opt => opt.Ignore())
                .ForMember(x=>x.Manufacturers, opt=>opt.MapFrom(x=>x.Manufacturers))
                ;
            CreateMap<InboundManufacturerRecordEditModel, InboundManufacturerRecord>()
                .ForMember(x => x.Inbound, opt => opt.Ignore())
                .ForMember(x => x.Manufacturer, opt => opt.Ignore())
                .ForMember(x => x.ManufacturerAppAddress, opt => opt.MapFrom(x => x.Address))
                .ForMember(x => x.ManufacturerAppAddressId, opt => opt.UseValue((int?)null))
                .ForMember(x => x.ItemNumber, opt => opt.UseValue(string.Empty))
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                ;

            CreateMap<BillsRecordEditModel, InboundBillRecord>()
                .ForMember(x => x.Inbound, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.InboundRecordId, opt => opt.Ignore())
                ;

            CreateMap<ContainersRecordEditModel, InboundContainerRecord>()
                .ForMember(x => x.Inbound, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.InboundRecordId, opt => opt.Ignore())
                ;
        }
    }
}