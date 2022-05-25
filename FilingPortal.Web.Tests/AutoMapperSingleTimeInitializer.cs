using AutoMapper;
using FilingPortal.Web.Mapping;
using FilingPortal.Web.Mapping.Converters;
using System.Globalization;

namespace FilingPortal.Web.Tests
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
                        config.AddProfile<ConvertersProfile>();
                        config.AddProfile<LocalProfile>();
                        config.AddProfile<DomainToViewModelProfile>();
                        config.AddProfile<ViewModelToDomainProfile>();
                        config.AddProfile<ViewModelToDtoProfile>();

                        config.AddProfile<AutoMapperTestProfile>();
                    });
                    _isInitialized = true;
                }
            }
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }
    }
}
