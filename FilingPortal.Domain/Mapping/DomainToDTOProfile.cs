using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Mapping.Converters;
using FilingPortal.Domain.Services.GridExport.Models.Audit.Rail;
using FilingPortal.Domain.Services.GridExport.Models.Pipeline;
using FilingPortal.Domain.Services.GridExport.Models.Vessel;
using FilingPortal.Domain.Services.GridExport.Models.VesselExport;

namespace FilingPortal.Domain.Mapping
{
    /// <summary>
    /// Class describing mapping of the domain entities to the view models used in the presentation layer
    /// </summary>
    public class DomainToDTOProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the domain entities to the view models
        /// </summary>
        public DomainToDTOProfile()
        {
            RailModelMappings();
            PipelineModelMapping();
            VesselImportModelMapping();
            VesselExportModelMapping();
        }
        private void RailModelMappings()
        {
            CreateMap<RailBdParsed, Manifest>()
                .ForMember(dest => dest.ManifestText, opts => opts.MapFrom(src => src.RailEdiMessage.EmMessageText));
            CreateMap<RailEdiMessage, DTOs.Rail.Manifest.Manifest>().ConvertUsing<RailEdiMessageToManifestConverter>();
            CreateMap<RailBdParsed, DTOs.Rail.Manifest.Manifest>().ConvertUsing<RailBdParsedToManifestConverter>();
            CreateMap<RailInboundReadModel, RailImportFilingValidationModel>();
            CreateMap<RailInboundReadModel, EntityDto>();
            // audit
            CreateMap<AuditRailDaily, AuditRailDailyAuditReportModel>()
                .ForMember(x => x.Errors, opt => opt.ResolveUsing<RailAuditValidationErrorsResolver>())
                .ForMember(x => x.Warnings, opt => opt.ResolveUsing<RailAuditValidationWarningsResolver>())
                ;
            CreateMap<AuditRailDailyRule, AuditRailDailyAuditRulesReportModel>()
                .ForMember(x => x.LastModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            CreateMap<AuditRailDailySpiRule, AuditRailDailyAuditSpiRulesReportModel>()
                .ForMember(x=>x.LastModifiedDate, opt => opt.Ignore())
                .ForMember(x=>x.LastModifiedBy, opt => opt.Ignore());
            CreateMap<AuditRailTrainConsistSheet, AuditRailTrainConsistSheetReportModel>();
        }

        private void PipelineModelMapping()
        {
            CreateMap<PipelineRulePrice, PipelineRulePriceReportModel>()
                .ForMember(x => x.Importer, opt => opt.MapFrom(x => x.Importer.ClientCode))
                .ForMember(x => x.CrudeType, opt => opt.MapFrom(x => x.CrudeType.BatchCode))
                .ForMember(x => x.Facility, opt => opt.MapFrom(x => x.Facility.Facility));
        }
        private void VesselImportModelMapping()
        {
            CreateMap<VesselRulePort, VesselRulePortsReportModel>()
                .ForMember(x => x.FirmsCode, opt => opt.MapFrom(x => x.FirmsCode.FirmsCode));
        }

        private void VesselExportModelMapping()
        {
            CreateMap<VesselExportRuleUsppiConsignee, VesselExportUsppiConsigneeReportModel>()
                .ForMember(x => x.Usppi, opt => opt.MapFrom(x => x.Usppi.ClientCode))
                .ForMember(x => x.Consignee, opt => opt.MapFrom(x => x.Consignee.ClientCode))
                .ForMember(x => x.ConsigneeAddress, opt => opt.MapFrom(x => x.ConsigneeAddress.Code));
        }
    }
}