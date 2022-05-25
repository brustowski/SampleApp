using AutoMapper;
using FilingPortal.Domain.Mapping.Converters;
using FilingPortal.Parts.CanadaTruckImport.Domain.Dtos;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Import.Inbound;
using FilingPortal.Parts.CanadaTruckImport.Domain.Import.RuleProduct;
using FilingPortal.Parts.CanadaTruckImport.Domain.Mapping.Converters;
using System;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Mapping
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
                .ForMember(x => x.VendorId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Vendor))
                .ForMember(x => x.Vendor, opt => opt.Ignore())
                .ForMember(x => x.ProductCodeId, opt => opt.ResolveUsing<ProductIdByCodeResolver, string>(x => x.ProductCode))
                .ForMember(x => x.ProductCode, opt => opt.Ignore())
                .ForMember(x => x.ConsigneeId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Consignee))
                .ForMember(x => x.Consignee, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.Deleted, opt => opt.Ignore())
                .ForMember(x => x.FilingHeaders, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
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

            CreateMap<Import.RuleVendor.ImportModel, RuleVendor>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.ImporterId,
                    opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Importer))
                .ForMember(x => x.Importer, opt => opt.Ignore())
                .ForMember(x => x.ExporterId,
                    opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Exporter))
                .ForMember(x => x.Exporter, opt => opt.Ignore())
                .ForMember(x => x.VendorId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Vendor))
                .ForMember(x => x.Vendor, opt => opt.Ignore());
            CreateMap<Import.RulePort.ImportModel, RulePort>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore());
            CreateMap<ImportModel, RuleProduct>()
                .ForMember(x => x.ProductCodeId, opt => opt.ResolveUsing<ProductIdByCodeResolver, string>(x => x.ProductCode))
                .ForMember(x => x.ProductCode, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore());
        }
    }
}