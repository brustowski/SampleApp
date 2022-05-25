using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests
{
    [TestClass]
    public class AutofaqModuleTests
    {

        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestMethod]
        public void BuildContainer_Returns_no_Exceptions()
        {
            try
            {
                var container = AutofacContainerFactory.BuildContainer();
            }
            catch(Exception ex)
            {
                Assert.Fail($"Expected no exceptions, but got: {ex.Message}");
            }
        }
    }
}