using System;
using AutoMapper;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.DTOs.Audit.Rail;
using FilingPortal.Domain.DTOs.Pipeline;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.DTOs.Truck;
using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.DTOs.Vessel;
using FilingPortal.Domain.DTOs.VesselExport;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Imports.Audit.Rule;
using FilingPortal.Domain.Imports.Audit.RuleSpi;
using FilingPortal.Domain.Imports.Pipeline.RulePrice;
using FilingPortal.Domain.Mapping.Converters;
using FilingPortal.Domain.Mapping.Resolvers;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using Framework.Domain;

namespace FilingPortal.Domain.Mapping
{
    /// <summary>
    /// Class describing mapping of the dto to the domain entities
    /// </summary>
    public class DTOToDomainProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToDTOProfile"/> class
        /// with all mappings of the dto to the domain entities
        /// </summary>
        public DTOToDomainProfile()
        {
            CreateMap<BaseDocumentDto, BaseDocumentWithContent>()
                .Include<RailDocumentDto, RailDocument>()
                .Include<TruckExportDocumentDto, TruckExportDocument>()
                .Include<PipelineDocumentDto, PipelineDocument>()
                .Include<TruckDocumentDto, TruckDocument>()
                .Include<VesselDocumentDto, VesselImportDocument>()
                .Include<VesselExportDocumentDto, VesselExportDocument>()
                .ForMember(x => x.Content, opt => opt.MapFrom(s => s.FileContent))
                .ForMember(x => x.Description, opt => opt.MapFrom(s => s.FileDesc))
                .ForMember(x => x.Extension, opt => opt.MapFrom(s => s.FileExtension))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.InboundRecordId, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.FilingHeaderId, opt => opt.Ignore());

            RailModelsMapping();

            PipelineModelsMapping();

            TruckImportModelsMapping();

            TruckExportModelsMapping();

            VesselImportModelsMapping();

            VesselExportModelsMapping();

            #region Audit

            CreateMap<RailAuditTrainConsistSheetImportParsingModel, AuditRailTrainConsistSheet>()
                .ForMember(x => x.Status, opt => opt.UseValue("Open"))
                .ForMember(x => x.Author, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.UseValue(0));

            #endregion
        }

        private void VesselExportModelsMapping()
        {
            CreateMap<VesselExportDocumentDto, VesselExportDocument>()
                .ForMember(x => x.FilingHeader, opt => opt.Ignore())
                .ForMember(x => x.InboundRecord, opt => opt.Ignore());
            CreateMap<Imports.VesselExport.RuleUsppiConsignee.ImportModel, VesselExportRuleUsppiConsignee>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.UsppiId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Usppi))
                .ForMember(r => r.Usppi, opt => opt.Ignore())
                .ForMember(r => r.ConsigneeId, opt => opt.ResolveUsing<ClientIdByCodeResolver, string>(x => x.Consignee))
                .ForMember(r => r.Consignee, opt => opt.Ignore())
                .ForMember(r => r.ConsigneeAddressId, opt => opt.Ignore())
                .ForMember(r => r.ConsigneeAddress, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
        }

        private void VesselImportModelsMapping()
        {
            CreateMap<VesselDocumentDto, VesselImportDocument>()
                .ForMember(x => x.FilingHeader, opt => opt.Ignore())
                .ForMember(x => x.InboundRecord, opt => opt.Ignore());
            CreateMap<Imports.Vessel.RulePort.ImportModel, VesselRulePort>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.DischargeName, opt => opt.Ignore())
                .ForMember(r => r.FirmsCodeId, opt => opt.ResolveUsing<FirmsIdByCodeResolver, string>(x => x.FirmsCode))
                .ForMember(r => r.FirmsCode, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<Imports.Vessel.RuleProduct.ImportModel, VesselRuleProduct>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
        }

        private void TruckExportModelsMapping()
        {
            CreateMap<TruckExportDocumentDto, TruckExportDocument>()
                .ForMember(x => x.FilingHeader, opt => opt.Ignore())
                .ForMember(x => x.InboundRecord, opt => opt.Ignore());
            CreateMap<TruckExportImportModel, TruckExportRecord>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.FilingHeaders, x => x.Ignore())
                .ForMember(d => d.Deleted, x => x.Ignore())
                .ForMember(d => d.Documents, opt => opt.Ignore());
            CreateMap<TruckExportUpdateModel, TruckExportRecord>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore());
            CreateMap<Imports.TruckExport.RuleConsignee.ImportModel, TruckExportRuleConsignee>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<Imports.TruckExport.RuleExporterConsignee.ImportModel, TruckExportRuleExporterConsignee>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
        }

        private void TruckImportModelsMapping()
        {
            CreateMap<TruckDocumentDto, TruckDocument>()
                .ForMember(x => x.TruckFilingHeader, opt => opt.Ignore())
                .ForMember(x => x.TruckInbound, opt => opt.Ignore());
            CreateMap<Imports.Truck.Inbound.ImportModel, TruckInbound>()
                .ForMember(d => d.ImporterCode, opt => opt.MapFrom(s => s.Importer))
                .ForMember(d => d.SupplierCode, opt => opt.MapFrom(s => s.Supplier))
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.FilingHeaders, x => x.Ignore())
                .ForMember(d => d.Deleted, x => x.Ignore())
                .ForMember(d => d.Documents, opt => opt.Ignore());
            CreateMap<Imports.Truck.RuleImporter.ImportModel, TruckRuleImporter>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<Imports.Truck.RulePort.ImportModel, TruckRulePort>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
        }

        private void PipelineModelsMapping()
        {
            CreateMap<PipelineDocumentDto, PipelineDocument>()
                .ForMember(x => x.PipelineFilingHeader, opt => opt.Ignore())
                .ForMember(x => x.PipelineInbound, opt => opt.Ignore());
            CreateMap<PipelineInboundImportParsingModel, PipelineInbound>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.FilingHeaders, opt => opt.Ignore())
                .ForMember(d => d.Deleted, opt => opt.Ignore())
                .ForMember(d => d.CreatedDate, opt => opt.Ignore())
                .ForMember(d => d.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.Documents, opt => opt.Ignore());
            CreateMap<Imports.Pipeline.RuleBatchCode.ImportModel, PipelineRuleBatchCode>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(r => r.PipelinePriceRules, opt => opt.Ignore());
            CreateMap<Imports.Pipeline.RuleImporter.ImportModel, PipelineRuleImporter>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<Imports.Pipeline.RuleFacility.ImportModel, PipelineRuleFacility>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<Imports.Pipeline.RuleConsigneeImporter.ImportModel, PipelineRuleConsigneeImporter>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<ImportModel, PipelineRulePrice>()
                .ForMember(x => x.CrudeTypeId, opt => opt.ResolveUsing<CrudeTypeIdResolver>())
                .ForMember(x => x.ImporterId, opt => opt.ResolveUsing<ImporterIdResolver>())
                .ForMember(x => x.FacilityId, opt => opt.ResolveUsing<FacilityIdResolver>())
                .ForMember(x => x.Importer, opt => opt.Ignore())
                .ForMember(x => x.CrudeType, opt => opt.Ignore())
                .ForMember(x => x.Facility, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Imports.Admin.AutoCreate.ImportModel, AutoCreateRecord>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<ParsingDataModel, AuditableEntity>()
                .Include<DailyAuditRuleImportModel, AuditRailDailyRule>()
                .Include<DailyAuditSpiRuleImportModel, AuditRailDailySpiRule>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<DailyAuditRuleImportModel, AuditRailDailyRule>()
                .ForMember(x => x.Author, opt => opt.Ignore());
            CreateMap<DailyAuditSpiRuleImportModel, AuditRailDailySpiRule>()
                .ForMember(x => x.Author, opt => opt.Ignore()); ;
        }

        private void RailModelsMapping()
        {
            CreateMap<RailDocumentDto, RailDocument>()
                .ForMember(x => x.RailFilingHeader, opt => opt.Ignore())
                .ForMember(x => x.RailInbound, opt => opt.Ignore());
            CreateMap<Imports.Rail.RuleImporterSupplier.ImportModel, RailRuleImporterSupplier>()
                .ForMember(r => r.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<Imports.Rail.RuleDescription.ImportModel, RailRuleDescription>()
                .ForMember(r => r.Id, opt => opt.Ignore())
                .ForMember(r => r.Description1, opt => opt.MapFrom(d => d.Description1.Trim()))
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
            CreateMap<Imports.Rail.RulePort.ImportModel, RailRulePort>()
                .ForMember(r => r.Id, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
        }
    }
}