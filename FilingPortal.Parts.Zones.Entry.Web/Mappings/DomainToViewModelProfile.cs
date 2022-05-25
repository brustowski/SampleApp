﻿using AutoMapper;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using FilingPortal.Parts.Zones.Entry.Web.Models;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.Mapping.Converters;
using FilingPortal.PluginEngine.Models;
using FilingPortal.PluginEngine.Models.InboundRecordModels;

namespace FilingPortal.Parts.Zones.Entry.Web.Mappings
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
            CreateMap<InboundReadModel, InboundRecordViewModel>()
                .ForMember(d => d.Actions, opt => opt.Ignore())
                .ForMember(d => d.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(d => d.JobStatus,
                    opt => opt.MapFrom(ir => ir.JobStatus.HasValue ? (int)ir.JobStatus : 0))
                ;

            CreateMap<DefValueReadModel, DefValuesViewModel>()
                .ForMember(d => d.ValueDesc, opt => opt.MapFrom(s => s.Description))
                .ForMember(d => d.ValueLabel, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.UISection, opt => opt.MapFrom(s => s.SectionTitle))
                .ForMember(d => d.HasDefaultValue, opt => opt.Ignore())
                .ForMember(d => d.Errors, opt => opt.Ignore())
                .ForMember(x => x.Actions, opt => opt.Ignore())
                .ForMember(x => x.HighlightingType, opt => opt.Ignore())
                .ForMember(d => d.OverriddenType, opt => opt.Ignore());

            CreateMap<Section, FilingConfigurationSection>()
                .ForMember(d => d.IsSingleSection, opt => opt.MapFrom(s => !s.IsArray));
            CreateMap<DefValueManualReadModel, FilingConfigurationField>()
                .ForMember(d => d.IsVisibleOn7501, opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual > 0))
                .ForMember(d => d.IsVisibleOnRuleDrivenData,
                    opt => opt.MapFrom(s => s.DisplayOnUI > 0 && s.Manual == 0))
                .ForMember(d => d.Order, opt => opt.MapFrom(s => s.Manual > 0 ? s.Manual : s.DisplayOnUI))
                .ForMember(d => d.IsDisabled, opt => opt.MapFrom(s => !s.Editable))
                .ForMember(d => d.IsMandatory, opt => opt.MapFrom(s => s.Mandatory))
                .ForMember(d => d.MaxLength, opt => opt.MapFrom(s => s.ValueMaxLength))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Label))
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.ColumnName))
                .ForMember(d => d.Type, opt => opt.ResolveUsing<FieldTypeValueResolver, string>(x => x.ValueType))
                .ForMember(d => d.Field, opt => opt.Ignore());

            CreateMap<Document, InboundRecordDocumentViewModel>()
                .ForMember(d => d.Status, opt => opt.Ignore())
                .ForMember(d => d.Type, opt => opt.MapFrom(d => d.DocumentType))
                .ForMember(d => d.Name, opt => opt.MapFrom(d => d.FileName))
                .ForMember(d => d.IsManifest, opt => opt.Ignore());

            CreateMap<RuleImporter, RuleImporterViewModel>()
                .ForMember(x => x.Importer, opt => opt.MapFrom(s => s.Importer.ClientCode));
        }
    }
}