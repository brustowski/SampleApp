using AutoMapper;
using FilingPortal.Domain.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests
{
    public class TestWithApplicationMapping
    {
        [TestInitialize]
        public void InitMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(config =>
            {
                AutoMappingConfigurationContainer.Configure(config);
            });
        }

        [TestCleanup]
        public void CleanupMapper()
        {
            Mapper.Reset();
        }
    }
}
