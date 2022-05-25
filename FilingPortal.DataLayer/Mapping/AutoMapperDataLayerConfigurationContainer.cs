using AutoMapper;

namespace FilingPortal.DataLayer.Mapping
{
    public class AutoMapperDataLayerConfigurationContainer
    {
        public static void Configure(IMapperConfigurationExpression config)
        {
            config.AddProfile<DomainToDTOProfile>();
            config.AddProfile<DTOToDomainProfile>();
        }
    }
}