using AutoMapper;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Domain.Common.Import.Models;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Parts.Common.Domain.AgileSettings;
using FilingPortal.Parts.Common.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.Base;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Validators;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.Mapping.Converters;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.Fields;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using FilingPortal.PluginEngine.Models.InboundRecordValidation;
using FilingPortal.Web.Models.AppSystem;
using FilingPortal.Web.Models.Audit.Rail;
using FilingPortal.Web.Models.ClientManagement;
using FilingPortal.Web.Models.Pipeline;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.Models.Truck;
using FilingPortal.Web.Models.TruckExport;
using FilingPortal.Web.Models.Vessel;
using FilingPortal.Web.Models.VesselExport;
using Framework.Domain.Paging;
using Framework.Infrastructure.Extensions;
using System.Linq;

namespace FilingPortal.Web.Mapping
{
    /// <summary>
    /// Class describing mapping of the domain entities to the view models used in the presentation layer
    /// </summary>
    public class DomainToViewModelProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainToViewModelProfile"/> class
        /// with all mappings of the domain entities to the view models
        /// </summary>
        public DomainToViewModelProfile()
        {
            CommonModelsMappings();
            DataProvidersMappings();
            AppSystemModelsMappings();
            RailModelsMappings();
            ClientModelsMappings();
            PipelineModelsMappings();
            TruckModelsMappings();
            VesselModelMappings();
            TruckExportModelsMappings();
            VesselExportModelsMappings();
            AuditModelMappings();
        }

        private void AuditModelMappings()
        {
            AuditRailModelMappings();
        }

        private void AuditRailModelMappings()
        {
            CreateMap<AuditRailTrainConsistSheet, TrainConsistSheetViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(x => x.Errors, opt => opt.Ignore())
                .ForMember(x => x.Actions, opt => opt.Ignore());

            CreateMap<AuditRailDailyRule, DailyAuditRuleViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.FreightComposite, opt => opt.MapFrom(s => s))
                ;
            CreateMap<AuditRailDailyRule, RailRuleFreightComposite>();

        }

        /// <summary>
        /// Provides mapping configuration for Common models
        /// </summary>
        private void CommonModelsMappings()
        {
            CreateMap<HeaderFilingStatus, LookupItem>()
                .ForMember(d => d.Value, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayValue, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.IsDefault, opt => opt.Ignore());
            CreateMap<HeaderMappingStatus, LookupItem>()
                .ForMember(d => d.Value, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayValue, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.IsDefault, opt => opt.Ignore());
            CreateMap<HeaderJobStatus, LookupItem>()
                .ForMember(d => d.Value, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayValue, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.IsDefault, opt => opt.Ignore());

            CreateMap<AgileField, ColumnConfig>()
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(s => s.DisplayName))
                .ForMember(d => d.EditType, opt => opt.MapFrom(s => s.TypeName))
                .ForMember(d => d.FieldName, opt => opt.MapFrom(s => s.FieldId.Column))
                .ForMember(d => d.MaxWidth, opt => opt.MapFrom(s => s.MaxLength))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<BaseDefValueReadModel, DefValuesViewModel>()
                .Include<RailDefValuesReadModel, DefValuesViewModel>()
                .Include<PipelineDefValueReadModel, DefValuesViewModel>()
                .Include<TruckDefValueReadModel, DefValuesViewModel>()
                .Include<TruckExportDefValueReadModel, DefValuesViewModel>()
                .Include<VesselExportDefValueReadModel, DefValuesViewModel>()
                .Include<VesselImportDefValueReadModel, DefValuesViewModel>()
                .ForMember(d => d.ValueDesc, opt => opt.MapFrom(s => s.Description))
                .ForMember(d => d.ValueLabel, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.UISection, opt => opt.MapFrom(s => s.SectionTitle))
                .ForMember(d => d.HasDefaultValue, opt => opt.Ignore())
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(x => x.Errors, opt => opt.Ignore());

        }
        /// <summary>
        /// Provides mapping configuration for Data Providers
        /// </summary>
        private void DataProvidersMappings()
        {
            CreateMap<string, LookupItem>()
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x))
                .ForMember(x => x.DisplayValue, opt => opt.MapFrom(x => x))
                .ForMember(x => x.IsDefault, opt => opt.UseValue(false));
            CreateMap<Country, LookupItem>()
                .ForMember(d => d.Value, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DisplayValue, opt => opt.MapFrom(s => $"{s.Code} - {s.Name}"))
                .ForMember(x => x.IsDefault, opt => opt.UseValue(false));

            CreateMap<ClientContact, LookupItem>()
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.DisplayValue, opt => opt.MapFrom(x => x.ContactName))
                .ForMember(x => x.IsDefault, opt => opt.UseValue(false));
            CreateMap<ClientContactAdditionalPhone, LookupItem>()
                .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Phone))
                .ForMember(x => x.DisplayValue, opt => opt.MapFrom(x => x.Phone))
                .ForMember(x => x.IsDefault, opt => opt.UseValue(false));
        }

        /// <summary>
        /// Provides mapping configuration for System models
        /// </summary>
        private void AppSystemModelsMappings()
        {
            CreateMap<AppUsersModel, AppUserViewModel>()
                .ForMember(d => d.Status, opt => opt.MapFrom(ir => ir.Status.Name))
                .ForMember(d => d.UserAccount, opt => opt.MapFrom(ir => ir.Id))
                .ForMember(d => d.Permissions,
                    opt => opt.MapFrom(ir => ir.Roles.SelectMany(x => x.Permissions.Select(p => p.Id)).Distinct()));

            CreateMap<AppAddress, AddressFieldEditModel>()
                .ForMember(x => x.Addr1, opt => opt.MapFrom(x => x.Address1))
                .ForMember(x => x.Addr2, opt => opt.MapFrom(x => x.Address2))
                .ForMember(x => x.AddressId, opt => opt.MapFrom(x => x.CwAddressId))
                .ForMember(x => x.AddressCode, opt => opt.MapFrom(x => x.CwAddress != null ? x.CwAddress.Code : null))
                .ForMember(x => x.Override, opt => opt.MapFrom(x => x.IsOverriden))
                ;
        }

        /// <summary>
        /// Provides mapping configuration for Rail models
        /// </summary>
        private void RailModelsMappings()
        {
            CreateMap<RailInboundReadModel, InboundRecordItemViewModel>()
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(d => d.FilingStatus, opt => opt.MapFrom(ir => ir.FilingStatus.HasValue ? (int)ir.FilingStatus : 0))
                .ForMember(d => d.FilingStatusTitle, opt => opt.MapFrom(ir => ir.FilingStatusTitle))
                .ForMember(d => d.MappingStatus, opt => opt.MapFrom(ir => ir.MappingStatus.HasValue ? (int)ir.MappingStatus : 0))
                .ForMember(d => d.MappingStatusTitle, opt => opt.MapFrom(ir => ir.MappingStatusTitle))
                .ForMember(d => d.HTSDescription, opt => opt.MapFrom(ir => ir.Description))
                ;

            CreateMap<RailDocument, InboundRecordDocumentViewModel>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.Description, opt => opt.MapFrom(d => d.Description))
                .ForMember(d => d.Type, opt => opt.MapFrom(d => d.DocumentType))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FileName))
                ;

            CreateMap<InboundRecordValidationResult, InboundRecordValidationViewModel>()
                .ForMember(d => d.Actions, opt => opt.Ignore());

            CreateMap<RailRuleImporterSupplier, RailRuleImporterSupplierViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.FreightComposite, opt => opt.MapFrom(s => s))
                ;

            CreateMap<RailRuleImporterSupplier, RailRuleFreightComposite>();

            CreateMap<RailRuleDescription, RailRuleDescriptionViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());
            CreateMap<RailRulePort, RailRulePortViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());
            CreateMap<RailDefValuesReadModel, DefValuesViewModel>();
            CreateMap<RailDefValuesManualReadModel, FilingConfigurationField>()
                .ForMember(d => d.IsVisibleOn7501, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual > 0))
                .ForMember(d => d.IsVisibleOnRuleDrivenData, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual == 0))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Manual > 0 ? s.Manual : s.DisplayOnUI))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Field, opt => opt.Ignore());
            CreateMap<RailSection, FilingConfigurationSection>()
                .ForMember(d => d.IsSingleSection, opt => opt.MapFrom(s => !s.IsArray));
            CreateMap<RailBdParsed, RailInboundEditModel>()
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description1))
                .ForMember(x => x.ReferenceNumber, opt => opt.MapFrom(x => x.ReferenceNumber1));
        }

        /// <summary>
        /// Provides mapping configuration for Clients models
        /// </summary>
        private void ClientModelsMappings()
        {
            CreateMap<Client, ClientViewModel>()
                .ForMember(d => d.Status, opt => opt.MapFrom(crm => crm.Status ? "Active" : "Deactivated"))
                .ForMember(d => d.ClientType, opt => opt.MapFrom(crm => crm.ClientType));
        }

        /// <summary>
        /// Provides mapping configuration for Pipeline models
        /// </summary>
        private void PipelineModelsMappings()
        {
            CreateMap<PipelineInboundReadModel, PipelineViewModel>()
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.FilingStatus, opt => opt.MapFrom(ir => ir.FilingStatus.HasValue ? (int)ir.FilingStatus : 0))
                .ForMember(d => d.FilingStatusTitle, opt => opt.MapFrom(ir => ir.FilingStatusTitle))
                .ForMember(d => d.MappingStatus, opt => opt.MapFrom(ir => ir.MappingStatus.HasValue ? (int)ir.MappingStatus : 0))
                .ForMember(d => d.MappingStatusTitle, opt => opt.MapFrom(ir => ir.MappingStatusTitle));

            CreateMap<RowError, ExcelFileValidationError>()
                .ForMember(d => d.FieldName, opt => opt.MapFrom(s => s.PropertyName))
                .ForMember(d => d.Sheet, opt => opt.MapFrom(s => s.SheetName))
                .ForMember(d => d.StringNumber, opt => opt.MapFrom(s => s.LineNumber))
                .ForMember(d => d.Error, opt => opt.MapFrom(s => s.ErrorMessage));
            CreateMap<RowError, XmlFileValidationError>()
                .ForMember(d => d.Tag, opt => opt.MapFrom(s => s.PropertyName))
                .ForMember(d => d.Error, opt => opt.MapFrom(s => s.ErrorMessage));
            CreateMap<FileProcessingResult, FileProcessingResultViewModel<ExcelFileValidationError>>();
            CreateMap<FileProcessingDetailedResult, FileProcessingDetailedResultViewModel<ExcelFileValidationError>>();
            CreateMap<FileProcessingResult, FileProcessingResultViewModel<XmlFileValidationError>>();
            CreateMap<PipelineDefValueReadModel, DefValuesViewModel>();
            CreateMap<PipelineRuleBatchCode, PipelineRuleBatchCodeViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());
            CreateMap<PipelineRuleImporter, PipelineRuleImporterViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());
            CreateMap<PipelineRuleConsigneeImporter, PipelineRuleConsigneeImporterViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());
            CreateMap<PipelineDefValueManualReadModel, FilingConfigurationField>()
                .ForMember(d => d.IsVisibleOn7501, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual > 0))
                .ForMember(d => d.IsVisibleOnRuleDrivenData, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual == 0))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Manual > 0 ? s.Manual : s.DisplayOnUI))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Field, opt => opt.Ignore());
            CreateMap<PipelineSection, FilingConfigurationSection>()
                .ForMember(d => d.IsSingleSection, opt => opt.MapFrom(s => !s.IsArray));
            CreateMap<PipelineDocument, InboundRecordDocumentViewModel>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.Type, opt => opt.MapFrom(d => d.DocumentType))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FileName))
                .ForMember(d => d.IsManifest, opt => opt.Ignore());
            CreateMap<PipelineRulePrice, PipelineRulePriceViewModel>()
                .ForMember(x => x.CrudeType, opt => opt.MapFrom(x => x.CrudeType.BatchCode))
                .ForMember(x => x.Importer, opt => opt.MapFrom(x => x.Importer.ClientCode))
                .ForMember(x => x.Facility, opt => opt.MapFrom(x => x.Facility.Facility))
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(x => x.Actions, x => x.Ignore());
        }

        /// <summary>
        /// Provides mapping configuration for Truck models
        /// </summary>
        private void TruckModelsMappings()
        {
            CreateMap<TruckInboundReadModel, TruckInboundViewModel>()
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(d => d.FilingStatus,
                    opt => opt.MapFrom(ir => ir.FilingStatus.HasValue ? (int)ir.FilingStatus : 0))
                .ForMember(d => d.FilingStatusTitle, opt => opt.MapFrom(ir => ir.FilingStatus.GetDescription()))
                .ForMember(d => d.MappingStatus,
                    opt => opt.MapFrom(ir => ir.MappingStatus.HasValue ? (int)ir.MappingStatus : 0))
                .ForMember(d => d.MappingStatusTitle, opt => opt.MapFrom(ir => ir.MappingStatus.GetDescription()));
            CreateMap<TruckDefValueReadModel, DefValuesViewModel>();
            CreateMap<TruckRuleImporter, TruckRuleImporterViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());
            CreateMap<TruckRulePort, TruckRulePortViewModel>()
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());
            CreateMap<TruckDefValueManualReadModel, FilingConfigurationField>()
                .ForMember(d => d.IsVisibleOn7501, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual > 0))
                .ForMember(d => d.IsVisibleOnRuleDrivenData, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual == 0))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Manual > 0 ? s.Manual : s.DisplayOnUI))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Field, opt => opt.Ignore());
            CreateMap<TruckSection, FilingConfigurationSection>()
                .ForMember(d => d.IsSingleSection, opt => opt.MapFrom(s => !s.IsArray));
            CreateMap<TruckDocument, InboundRecordDocumentViewModel>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.DocumentType))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FileName))
                .ForMember(d => d.IsManifest, opt => opt.Ignore())
                ;
        }

        /// <summary>
        /// Provides mapping configuration for Truck Export models
        /// </summary>
        private void TruckExportModelsMappings()
        {
            CreateMap<TruckExportReadModel, TruckExportViewModel>()
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(d => d.JobStatus,
                    opt => opt.MapFrom(ir => ir.JobStatus.HasValue ? (int)ir.JobStatus : 0))
                ;

            CreateMap<TruckExportDefValueReadModel, DefValuesViewModel>();
            CreateMap<TruckExportDocument, InboundRecordDocumentViewModel>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.Type, opt => opt.MapFrom(d => d.DocumentType))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FileName))
                .ForMember(d => d.IsManifest, opt => opt.Ignore());
            CreateMap<TruckExportDefValuesManualReadModel, InboundRecordField>()
                .ForMember(d => d.DefaultValue, opt => opt.MapFrom(s => s.Value))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Prefix, opt => opt.Ignore())
                .ForMember(d => d.DependOn, opt => opt.Ignore())
                .ForMember(d => d.Class, opt => opt.Ignore())
                .ForMember(d => d.MarkedForFiltering, opt => opt.Ignore())
                ;
            CreateMap<TruckExportDefValuesManualReadModel, FilingConfigurationField>()
                .ForMember(d => d.IsVisibleOn7501, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual > 0))
                .ForMember(d => d.IsVisibleOnRuleDrivenData, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual == 0))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Manual > 0 ? s.Manual : s.DisplayOnUI))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Field, opt => opt.Ignore());
            CreateMap<TruckExportSection, FilingConfigurationSection>()
                .ForMember(d => d.IsSingleSection, opt => opt.MapFrom(s => !s.IsArray));
        }

        /// <summary>
        /// Provides mapping configuration for Vessel Export models
        /// </summary>
        private void VesselExportModelsMappings()
        {
            CreateMap<VesselExportReadModel, VesselExportViewModel>()
                .ForMember(d => d.FilingStatus,
                    opt => opt.MapFrom(ir => ir.FilingStatus.HasValue ? (int)ir.FilingStatus : 0))
                .ForMember(d => d.FilingStatusTitle, opt => opt.MapFrom(ir => ir.FilingStatus.GetDescription()))
                .ForMember(d => d.MappingStatus,
                    opt => opt.MapFrom(ir => ir.MappingStatus.HasValue ? (int)ir.MappingStatus : 0))
                .ForMember(d => d.MappingStatusTitle, opt => opt.MapFrom(ir => ir.MappingStatus.GetDescription()))
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                ;
            CreateMap<VesselExportDefValueReadModel, DefValuesViewModel>();
            CreateMap<VesselExportDocument, InboundRecordDocumentViewModel>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.Type, opt => opt.MapFrom(d => d.DocumentType))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FileName))
                .ForMember(d => d.IsManifest, opt => opt.Ignore());
            CreateMap<VesselExportDefValuesManualReadModel, InboundRecordField>()
                .ForMember(d => d.DefaultValue, opt => opt.MapFrom(s => s.Value))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Prefix, opt => opt.Ignore())
                .ForMember(d => d.DependOn, opt => opt.Ignore())
                .ForMember(d => d.Class, opt => opt.Ignore())
                .ForMember(d => d.MarkedForFiltering, opt => opt.Ignore())
                ;
            CreateMap<VesselExportDefValuesManualReadModel, FilingConfigurationField>()
                .ForMember(d => d.IsVisibleOn7501, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual > 0))
                .ForMember(d => d.IsVisibleOnRuleDrivenData, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual == 0))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Manual > 0 ? s.Manual : s.DisplayOnUI))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Field, opt => opt.Ignore());
            CreateMap<VesselExportSection, FilingConfigurationSection>()
                .ForMember(d => d.IsSingleSection, opt => opt.MapFrom(s => !s.IsArray));
            CreateMap<VesselExportRecord, VesselExportViewModel>()
                .ForMember(d => d.Usppi, opt => opt.MapFrom(s => s.Usppi.ClientCode))
                .ForMember(d => d.Importer, opt => opt.MapFrom(s => s.Importer.ClientCode))
                .ForMember(d => d.Vessel, opt => opt.MapFrom(s => s.Vessel.Name))
                .ForMember(d => d.FilingNumber, opt => opt.Ignore())
                .ForMember(d => d.EntryStatus, opt => opt.Ignore())
                .ForMember(d => d.EntryStatusDescription, opt => opt.Ignore())
                .ForMember(d => d.JobLink, opt => opt.Ignore())
                .ForMember(d => d.FilingHeaderId, opt => opt.Ignore())
                .ForMember(d => d.MappingStatus, opt => opt.Ignore())
                .ForMember(d => d.MappingStatusTitle, opt => opt.Ignore())
                .ForMember(d => d.FilingStatus, opt => opt.Ignore())
                .ForMember(d => d.FilingStatusTitle, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.HasAllRequiredRules, opt => opt.Ignore())
                ;
            CreateMap<VesselExportRuleUsppiConsignee, VesselExportRuleUsppiConsigneeViewModel>()
                .ForMember(d => d.Usppi, opt => opt.MapFrom(x => x.Usppi.ClientCode))
                .ForMember(d => d.Consignee, opt => opt.MapFrom(x => x.Consignee.ClientCode))
                .ForMember(d => d.ConsigneeAddress, opt => opt.MapFrom(x => x.ConsigneeAddress != null ? x.ConsigneeAddress.Code : string.Empty))
                .ForMember(d => d.ConsigneeAddressId, opt => opt.MapFrom(x => x.ConsigneeAddressId.HasValue ? x.ConsigneeAddressId.ToString() : string.Empty))
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());
        }

        /// <summary>
        /// Provides mapping configuration for Vessel models
        /// </summary>
        private void VesselModelMappings()
        {
            CreateMap<VesselImportReadModel, VesselImportViewModel>()
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(d => d.FilingStatus,
                    opt => opt.MapFrom(ir => ir.FilingStatus.HasValue ? (int)ir.FilingStatus : 0))
                .ForMember(d => d.FilingStatusTitle, opt => opt.MapFrom(ir => ir.FilingStatus.GetDescription()))
                .ForMember(d => d.MappingStatus,
                    opt => opt.MapFrom(ir => ir.MappingStatus.HasValue ? (int)ir.MappingStatus : 0))
                .ForMember(d => d.MappingStatusTitle, opt => opt.MapFrom(ir => ir.MappingStatus.GetDescription()));

            CreateMap<VesselImportRecord, VesselImportEditModel>()
                .ForMember(d => d.FilerId, opt => opt.MapFrom(s => s.UserId));

            CreateMap<VesselRuleImporter, VesselRuleImporterViewModel>()
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());

            CreateMap<VesselRulePort, VesselRulePortViewModel>()
                .ForMember(d => d.FirmsCode, opt => opt.MapFrom(x => x.FirmsCode.FirmsCode))
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());

            CreateMap<VesselRuleProduct, VesselRuleProductViewModel>()
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());

            CreateMap<VesselImportDefValueReadModel, DefValuesViewModel>();

            CreateMap<VesselImportDefValuesManualReadModel, FilingConfigurationField>()
                .ForMember(d => d.IsVisibleOn7501, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual > 0))
                .ForMember(d => d.IsVisibleOnRuleDrivenData, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual == 0))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Manual > 0 ? s.Manual : s.DisplayOnUI))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Field, opt => opt.Ignore());

            CreateMap<VesselImportSection, FilingConfigurationSection>()
                .ForMember(d => d.IsSingleSection, opt => opt.MapFrom(s => !s.IsArray));

            CreateMap<VesselImportDocument, InboundRecordDocumentViewModel>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.Type, opt => opt.MapFrom(d => d.DocumentType))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FileName))
                .ForMember(d => d.IsManifest, opt => opt.Ignore());
        }
    }
}