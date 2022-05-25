using System.Globalization;
using AutoMapper;
using FilingPortal.Parts.Recon.Domain.Mapping;
using FilingPortal.Parts.Recon.Web.Mappings;
using DomainToDTOProfile = FilingPortal.Domain.Mapping.DomainToDTOProfile;

namespace FilingPortal.Parts.Recon.WebTests
{
    public class AutoMapperSingleTimeInitializer
    {
        private static bool _isInitialized;
        private static readonly object Lock = new object();

        public static void Init()
        {
            lock (Lock)
            {
                if (!_isInitialized)
                {
                    Mapper.Initialize(config =>
                    {
                        config.AddProfile<DomainToViewModelProfile>();
                        config.AddProfile<ViewModelToDomainProfile>();
                        config.AddProfile<DTOsToDomainProfile>();
                        config.AddProfile<DomainToDTOProfile>();
                    });
                    _isInitialized = true;
                }
            }
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }
    }
}
