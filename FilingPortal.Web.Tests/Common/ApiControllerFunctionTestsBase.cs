using FilingPortal.PluginEngine.Controllers;
using FilingPortal.Web.App_Start;
using FilingPortal.Web.Tests.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Web.Tests.Common
{
    public abstract class ApiControllerFunctionTestsBase<T> : ApiControllerTestsBase<T> where T : ApiControllerBase
    {
        private class TestPermission : AppPermission
        {

        }

        protected T Controller { get; private set; }

        [TestInitialize]
        public override void TestInitializeInternal()
        {
            Configuration = new HttpConfiguration();
            WebApiConfig.Register(Configuration);
            Configuration.EnsureInitialized();
            Controller = TestInitialize();
            if (Controller != null)
            {
                Controller.Request = new HttpRequestMessage();
                Controller.Configuration = Configuration;
                Controller.CurrentUser = new AppUsersModel
                {
                    Id = "test_user",
                    Roles = new List<AppRole>
                    {
                        new AppRole
                        {
                            Id = 1,
                            Permissions = new List<AppPermission>
                            {
                                new TestPermission {Id = 1, Name = "test",Description = "test permission"}
                            }
                        }
                    }
                };

            }
        }

        protected abstract T TestInitialize();

    }
}