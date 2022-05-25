using System;
using AutoMapper;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Admin;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Parts.Common.Domain.DTOs;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Models.Admin;
using FilingPortal.Web.Models.Audit.Rail;
using FilingPortal.Web.Models.Pipeline;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.Models.Truck;
using FilingPortal.Web.Models.TruckExport;
using FilingPortal.Web.Models.Vessel;
using FilingPortal.Web.Models.VesselExport;

namespace FilingPortal.Web.Mapping
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
            CreateMap<InboundRecordParameterModel, InboundRecordParameter>();
            CreateMap<InboundRecordFileModel, InboundRecordFilingParameters>();

            AppSystemMappings();
            AdminMappings();
            RailModelsMappings();
            TruckModelsMappings();
            PipelineModelsMappings();
            VesselModelMappings();
            TruckExportModelsMappings();
            VesselExportModelsMappings();
            AuditModelsMappings();
        }

        private void AdminMappings()
        {
            CreateMap<AutoCreateRecordEditModel, AutoCreateRecord>()
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore());
        }

        private void AppSystemMappings()
        {
            CreateMap<AddressFieldEditModel, AppAddress>()
                .ForMember(x => x.Address1, opt => opt.MapFrom(x => x.Addr1))
                .ForMember(x => x.Address2, opt => opt.MapFrom(x => x.Addr2))
                .ForMember(x => x.CwAddress, opt => opt.Ignore())
                .ForMember(x => x.CwAddressId, opt => opt.MapFrom(x => x.AddressId))
                .ForMember(x => x.IsOverriden, opt => opt.MapFrom(x => x.Override))
                ;
        }

        private void AuditModelsMappings()
        {
            RailAuditModelsMappings();
        }

        private void RailAuditModelsMappings()
        {
            CreateMap<DailyAuditRuleEditModel, AuditRailDailyRule>()
                .ForMember(x => x.Author, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.Freight, opt => opt.MapFrom(d => d.FreightComposite.Freight))
                .ForMember(x => x.FreightType, opt => opt.MapFrom(d => d.FreightComposite.FreightType));

            CreateMap<DailyAuditSpiRuleEditModel, AuditRailDailySpiRule>()
                .ForMember(x => x.Author, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.CreatedUser, opt => opt.Ignore());
        }

        /// <summary>
        /// Provides mapping configuration for Rail models
        /// </summary>
        private void RailModelsMappings()
        {

            CreateMap<DefValuesEditModel, RailDefValues>()
                .ForMember(r => r.ColumnName, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(r => r.Description, opt => opt.MapFrom(s => s.ValueDesc))
                .ForMember(r => r.Label, opt => opt.MapFrom(s => s.ValueLabel))
                .ForMember(r => r.SectionId, opt => opt.Ignore())
                .ForMember(r => r.Section, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(d=>d.DependsOn, opt=> opt.Ignore());

            CreateMap<RailRuleImporterSupplierEditModel, RailRuleImporterSupplier>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(r => r.Freight, opt => opt.MapFrom(s => s.FreightComposite.Freight))
                .ForMember(r => r.FreightType, opt => opt.MapFrom(s => s.FreightComposite.FreightType))
                ;

            CreateMap<RailRuleDescriptionEditModel, RailRuleDescription>()
                .ForMember(r => r.Description1, opt => opt.MapFrom(d => d.Description1.Trim()))
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<RailRulePortEditModel, RailRulePort>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<RailInboundEditModel, RailBdParsed>()
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.RailEdiMessageId, opt => opt.UseValue((int?)null))
                .ForMember(x => x.CWCreatedDateUTC, opt => opt.Ignore())
                .ForMember(x => x.Description1, opt => opt.MapFrom(d => d.Description))
                .ForMember(x => x.Deleted, opt => opt.UseValue(false))
                .ForMember(x => x.DuplicateOf, opt => opt.UseValue((int?)null))
                .ForMember(x => x.RailEdiMessage, opt => opt.UseValue((RailEdiMessage)null))
                .ForMember(x => x.FilingHeaders, opt => opt.Ignore())
                .ForMember(x => x.Documents, opt => opt.Ignore())
                .ForMember(x => x.ReferenceNumber1, opt => opt.MapFrom(d => d.ReferenceNumber))
                .ForMember(x => x.Id, opt => opt.NullSubstitute(0));
        }

        /// <summary>
        /// Provides mapping configuration for Truck models
        /// </summary>
        private void TruckModelsMappings()
        {
            CreateMap<DefValuesEditModel, TruckDefValue>()
                .ForMember(r => r.Description, opt => opt.MapFrom(s => s.ValueDesc))
                .ForMember(r => r.Label, opt => opt.MapFrom(s => s.ValueLabel))
                .ForMember(s => s.SectionId, opt => opt.Ignore())
                .ForMember(s => s.Section, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.DependsOn, opt => opt.Ignore());

            CreateMap<TruckRuleImporterEditModel, TruckRuleImporter>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<TruckRulePortEditModel, TruckRulePort>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());
        }

        /// <summary>
        /// Provides mapping configuration for Truck Export models
        /// </summary>
        private void TruckExportModelsMappings()
        {
            CreateMap<TruckExportRuleConsigneeEditModel, TruckExportRuleConsignee>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<TruckExportRuleExporterConsigneeEditModel, TruckExportRuleExporterConsignee>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<DefValuesEditModel, TruckExportDefValue>()
                .ForMember(r => r.ColumnName, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(r => r.Description, opt => opt.MapFrom(s => s.ValueDesc))
                .ForMember(r => r.Label, opt => opt.MapFrom(s => s.ValueLabel))
                .ForMember(r => r.SectionId, opt => opt.Ignore())
                .ForMember(r => r.Section, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.DependsOn, opt => opt.Ignore());
        }

        /// <summary>
        /// Provides mapping configuration for Vessel Export models
        /// </summary>
        private void VesselExportModelsMappings()
        {
            CreateMap<VesselExportRuleUsppiConsigneeEditModel, VesselExportRuleUsppiConsignee>()
                .ForMember(r => r.Usppi, opt => opt.Ignore())
                .ForMember(r => r.Consignee, opt => opt.Ignore())
                .ForMember(r => r.ConsigneeAddress, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<DefValuesEditModel, VesselExportDefValue>()
                .ForMember(r => r.ColumnName, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(r => r.Description, opt => opt.MapFrom(s => s.ValueDesc))
                .ForMember(r => r.Label, opt => opt.MapFrom(s => s.ValueLabel))
                .ForMember(r => r.SectionId, opt => opt.Ignore())
                .ForMember(r => r.Section, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.DependsOn, opt => opt.Ignore());

            CreateMap<VesselExportEditModel, VesselExportRecord>()
                .ForMember(x => x.Usppi, opt => opt.Ignore())
                .ForMember(x => x.Address, opt => opt.Ignore())
                .ForMember(x => x.Importer, opt => opt.Ignore())
                .ForMember(r => r.Vessel, opt => opt.Ignore())
                .ForMember(r => r.CountryOfDestination, opt => opt.Ignore())
                .ForMember(x => x.Deleted, opt => opt.UseValue(false))
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.FilingHeaders, opt => opt.Ignore())
                .ForMember(x => x.Documents, opt => opt.Ignore())
                .ForMember(x => x.Contact, opt => opt.Ignore())
                ;
        }

        /// <summary>
        /// Provides mapping configuration for Pipeline models
        /// </summary>
        private void PipelineModelsMappings()
        {
            CreateMap<DefValuesEditModel, PipelineDefValue>()
                .ForMember(r => r.Description, opt => opt.MapFrom(s => s.ValueDesc))
                .ForMember(r => r.Label, opt => opt.MapFrom(s => s.ValueLabel))
                .ForMember(r => r.SectionId, opt => opt.Ignore())
                .ForMember(r => r.Section, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.DependsOn, opt => opt.Ignore());

            CreateMap<PipelineRuleBatchCodeEditModel, PipelineRuleBatchCode>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(r => r.PipelinePriceRules, opt => opt.Ignore());
            CreateMap<PipelineRuleImporterEditModel, PipelineRuleImporter>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<PipelineRuleFacilityEditModel, PipelineRuleFacility>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<PipelineRuleConsigneeImporterEditModel, PipelineRuleConsigneeImporter>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<PipelineRulePriceEditModel, PipelineRulePrice>()
                .ForMember(r => r.CrudeType, opt => opt.Ignore())
                .ForMember(r => r.Importer, opt => opt.Ignore())
                .ForMember(r => r.Facility, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

        }

        /// <summary>
        /// Provides mapping configuration for Vessel models
        /// </summary>
        private void VesselModelMappings()
        {
            CreateMap<VesselRuleImporterEditModel, VesselRuleImporter>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<VesselRulePortEditModel, VesselRulePort>()
                .ForMember(r => r.DischargeName, opt => opt.Ignore())
                .ForMember(r => r.FirmsCode, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<VesselRuleProductEditModel, VesselRuleProduct>()
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore());

            CreateMap<VesselImportEditModel, VesselImportRecord>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(y => y.FilerId))
                .ForMember(x => x.FirmsCode, opt => opt.Ignore())
                .ForMember(x => x.Importer, opt => opt.Ignore())
                .ForMember(x => x.Supplier, opt => opt.Ignore())
                .ForMember(x => x.Deleted, opt => opt.UseValue(false))
                .ForMember(x => x.CreatedUser, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.Ignore())
                .ForMember(x => x.State, opt => opt.Ignore())
                .ForMember(x => x.Classification, opt => opt.Ignore())
                .ForMember(x => x.ProductDescription, opt => opt.Ignore())
                .ForMember(x => x.Filer, opt => opt.Ignore())
                .ForMember(r => r.Vessel, opt => opt.Ignore())
                .ForMember(x => x.CountryOfOrigin, opt => opt.Ignore())
                .ForMember(x => x.FilingHeaders, opt => opt.Ignore())
                .ForMember(x => x.Documents, opt => opt.Ignore());

            CreateMap<DefValuesEditModel, VesselImportDefValue>()
                .ForMember(r => r.ColumnName, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(r => r.Description, opt => opt.MapFrom(s => s.ValueDesc))
                .ForMember(r => r.Label, opt => opt.MapFrom(s => s.ValueLabel))
                .ForMember(r => r.SectionId, opt => opt.Ignore())
                .ForMember(r => r.Section, opt => opt.Ignore())
                .ForMember(r => r.CreatedDate, opt => opt.Ignore())
                .ForMember(r => r.CreatedUser, opt => opt.Ignore())
                .ForMember(d => d.DependsOn, opt => opt.Ignore());
        }
    }
}