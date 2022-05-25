using AutoMapper;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.FieldConfigurations.Common;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.PluginEngine.Mapping.Converters;
using FilingPortal.Web.Mapping.Converters;

namespace FilingPortal.Web.Mapping
{
    /// <summary>
    /// Class describing mapping of the web models
    /// </summary>
    public class LocalProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalProfile"/> class
        /// with all mappings of the domain entities to the view models
        /// </summary>
        public LocalProfile()
        {
            InboundRecordFieldMappings();
        }

        /// <summary>
        /// Provides mapping configuration for Inbound Record fields
        /// </summary>
        private void InboundRecordFieldMappings()
        {
            CreateMap<InboundRecordField, DropdownInboundRecordField>()
                .ForMember(d => d.Type, opt => opt.UseValue(UIValueTypes.Lookup))
                .ForMember(d => d.IsDynamicProvider, opt => opt.Ignore())
                .ForMember(d => d.ProviderName, opt => opt.Ignore());

            CreateMap<InboundRecordField, AddressInboundRecordField>()
                .ForMember(d=>d.DefaultValue, opt => opt.ResolveUsing<AddressFieldTypeValueResolver, string>(s => s.DefaultValue))
                .ForMember(d => d.IsDynamicProvider, opt => opt.UseValue(false))
                .ForMember(d => d.ProviderName, opt => opt.UseValue(DataProviderNames.ClientAddresses));
        }
    }
}