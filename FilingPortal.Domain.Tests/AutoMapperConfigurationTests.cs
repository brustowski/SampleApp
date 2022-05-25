using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests
{
    [TestClass]
    public class AutoMapperConfigurationTests : TestWithApplicationMapping
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            AutoMapperSingleTimeInitializer.Init();
        }

        [TestMethod]
        public void AllMappingsShouldBeCorrect()
        {
            Mapper.AssertConfigurationIsValid();
        }

    }
}
