using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests
{
    [TestClass]
    public class AutoMapperConfigurationTests
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
