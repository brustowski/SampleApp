using System.Globalization;
using AutoMapper;
using FilingPortal.Domain.Mapping;

namespace FilingPortal.Domain.Tests
{
    public class AutoMapperSingleTimeInitializer
    {
        private static readonly object Lock = new object();

        public static void Init()
        {
            //lock (Lock)
            //{
            //    if (!_isInitialized)
            //    {
            //        Mapper.Initialize(config =>
            //        {
            //            AutoMappingConfigurationContainer.Configure(config);
            //        });
            //        _isInitialized = true;
            //    }
            //}
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
        }
    }
}
