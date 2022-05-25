using AutoMapper;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.CanadaTruckImport.Domain.Reporting.RuleProduct;
using FilingPortal.Parts.CanadaTruckImport.Domain.Reporting.RuleVendor;

namespace FilingPortal.Parts.CanadaTruckImport.Domain.Mapping
{
    public class DomainToDTOProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the domain entities to the DTO models
        /// </summary>
        public DomainToDTOProfile()
        {
            CreateMap<RuleVendor, RuleVendorReportModel>()
                .ForMember(x => x.Vendor, opt => opt.MapFrom(x => x.Vendor.ClientCode))
                .ForMember(x => x.Importer, opt => opt.MapFrom(x => x.Importer.ClientCode))
                .ForMember(x => x.Exporter, opt => opt.MapFrom(x => x.Exporter.ClientCode))
                .ForMember(x => x.NoPackages, opt => opt.MapFrom(x => x.NoPackages.HasValue ? x.NoPackages.ToString() : null));
            CreateMap<RuleProduct, RuleProductReportModel>()
                .ForMember(x => x.ProductCode, opt => opt.MapFrom(s => s.ProductCode.Code));
        }
    }
}
