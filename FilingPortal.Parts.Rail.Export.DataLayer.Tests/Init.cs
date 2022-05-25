using System.Data.Entity;
using FilingPortal.Parts.Common.DataLayer.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Tests
{
    [TestClass]
    public class InitTests
    {
        [AssemblyInitialize]
        public static void Init(TestContext context)
        {
            DbConfiguration.SetConfiguration(new FpConfiguration());
        }
    }
}
