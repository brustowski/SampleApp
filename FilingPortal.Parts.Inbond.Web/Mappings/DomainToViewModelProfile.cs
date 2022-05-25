using AutoMapper;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Web.Models;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.Mapping.Converters;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordModels;
using Framework.Infrastructure.Extensions;

namespace FilingPortal.Parts.Inbond.Web.Mappings
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
            CreateMap<InboundReadModel, InbondViewModel>()
                .ForMember(d => d.MappingStatus,
                    opt => opt.MapFrom(ir => ir.MappingStatus.HasValue ? (int)ir.MappingStatus : 0))
                .ForMember(d => d.MappingStatusTitle, opt => opt.MapFrom(ir => ir.MappingStatus.GetDescription()))
                .ForMember(d => d.FilingStatus,
                    opt => opt.MapFrom(ir => ir.FilingStatus.HasValue ? (int)ir.FilingStatus : 0))
                .ForMember(d => d.FilingStatusTitle, opt => opt.MapFrom(ir => ir.FilingStatus.GetDescription()));

            CreateMap<RuleEntry, RuleEntryViewModel>()
                .ForMember(x => x.FirmsCode, opt => opt.MapFrom(s => s.FirmsCode.FirmsCode))
                .ForMember(x => x.Importer, opt => opt.MapFrom(s => s.Importer.ClientCode))
                .ForMember(d => d.ImporterAddress, opt => opt.MapFrom(x => x.ImporterAddress != null ? x.ImporterAddress.Code : string.Empty))
                .ForMember(d => d.ImporterAddressId, opt => opt.MapFrom(x => x.ImporterAddressId.HasValue ? x.ImporterAddressId.ToString() : string.Empty))
                .ForMember(x => x.Consignee, opt => opt.MapFrom(s => s.Consignee.ClientCode))
                .ForMember(d => d.ConsigneeAddress, opt => opt.MapFrom(x => x.ConsigneeAddress != null ? x.ConsigneeAddress.Code : string.Empty))
                .ForMember(d => d.ConsigneeAddressId, opt => opt.MapFrom(x => x.ConsigneeAddressId.HasValue ? x.ConsigneeAddressId.ToString() : string.Empty))
                .ForMember(x => x.Shipper, opt => opt.MapFrom(s => s.Shipper.ClientCode));

            CreateMap<DefValueReadModel, DefValuesViewModel>()
                .ForMember(d => d.ValueDesc, opt => opt.MapFrom(s => s.Description))
                .ForMember(d => d.ValueLabel, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.UISection, opt => opt.MapFrom(s => s.SectionTitle))
                .ForMember(d => d.HasDefaultValue, opt => opt.Ignore())
                .ForMember(d => d.Actions, opt => opt.Ignore());

            CreateMap<Section, FilingConfigurationSection>()
                .ForMember(d => d.IsSingleSection, opt => opt.MapFrom(s => !s.IsArray));
            CreateMap<DefValueManualReadModel, FilingConfigurationField>()
                .ForMember(d => d.IsVisibleOn7501, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual > 0))
                .ForMember(d => d.IsVisibleOnRuleDrivenData, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual == 0))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Manual > 0 ? s.Manual : s.DisplayOnUI))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver,string>(x=>x.ValueType))
                .ForMember(d => d.Field, opt => opt.Ignore());

            CreateMap<Document, InboundRecordDocumentViewModel>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.Type, opt => opt.MapFrom(d => d.DocumentType))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FileName))
                .ForMember(d => d.IsManifest, opt => opt.Ignore());
        }
    }
}