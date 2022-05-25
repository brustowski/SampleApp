using Autofac;
using AutoMapper;
using FilingPortal.DataLayer.Mapping;
using FilingPortal.Domain.Mapping;
using FilingPortal.Web.Common.Plugins;
using FilingPortal.Web.Mapping.Converters;

namespace FilingPortal.Web.Mapping
{
    /// <summary>
    /// Main class for AutoMapper configuration and initialization
    /// </summary>
    public class AutoMapperConfiguration
    {
        /// <summary>
        /// Initializes the AutoMapper instance with configuration profiles
        /// </summary>
        /// <param name="container"></param>
        public static void Init(IContainer container)
        {
            Mapper.Initialize(config =>
            {
                config.ConstructServicesUsing(container.Resolve);

                config.AddProfile<ConvertersProfile>();
                config.AddProfile<LocalProfile>();
                config.AddProfile<DomainToViewModelProfile>();
                config.AddProfile<ViewModelToDomainProfile>();
                config.AddProfile<ViewModelToDtoProfile>();
                AutoMappingConfigurationContainer.Configure(config);
                AutoMapperDataLayerConfigurationContainer.Configure(config);
                config.AddProfiles(PluginsConfiguration.GetAssemblies());
            });
        }
    }
}