using System.Globalization;
using AutoMapper;
using FilingPortal.Parts.Isf.Domain.Mapping;
using FilingPortal.Parts.Isf.Web.Mappings;

namespace FilingPortal.Parts.Isf.WebTests
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
