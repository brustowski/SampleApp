using AutoMapper;
using FilingPortal.Parts.Recon.WebTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Parts.Rail.Export.WebTests
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
