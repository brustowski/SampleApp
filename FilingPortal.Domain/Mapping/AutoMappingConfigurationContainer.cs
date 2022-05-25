using AutoMapper;

namespace FilingPortal.Domain.Mapping
{
    /// <summary>
    /// Class for AutoMapper configuration and initialization
    /// </summary>
    public class AutoMappingConfigurationContainer
    {
        /// <summary>
        /// Initializes the AutoMapper instance with configuration profiles
        /// </summary>
        /// <param name="config">AutoMapper configuration</param>
        public static void Configure(IMapperConfigurationExpression config)
        {
            config.AddProfile<DomainToDTOProfile>();
            config.AddProfile<DTOToDomainProfile>();
            config.AddProfile<LocalProfile>();
        }
    }
}