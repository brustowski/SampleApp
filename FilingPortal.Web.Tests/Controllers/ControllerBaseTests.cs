using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Controllers
{
    [TestClass]
    public abstract class ControllerBaseTests<TController> where TController: Controller
    {
        protected TController Controller;
        protected Mock<HttpContextBase> HttpContextMock;
        protected Mock<HttpResponseBase> HttpResponseBaseMock;
        protected Mock<HttpSessionStateBase> HttpSessionStateBaseMock;

        [TestInitialize]
        public void TestInitialize()
        {
            //TODO: this line forces EF6 to load for unit tests. EntityFramework needs to be removed from Web proj
            //var unused = typeof(System.Data.Entity.SqlServer.SqlProviderServices).ToString();

            var validPrincipal = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, GetUserIdTestInitialize()),
                        new Claim(ClaimTypes.Name, "TestUser")
                    })
            });

            HttpContextMock = new Mock<HttpContextBase>();
            HttpContextMock.Setup(x => x.User).Returns(validPrincipal);

            HttpResponseBaseMock = new Mock<HttpResponseBase>();
            HttpResponseBaseMock.SetupAllProperties();
            HttpResponseBaseMock.Object.StatusCode = 200;

            HttpContextMock.Setup(x => x.Response).Returns(HttpResponseBaseMock.Object);

            HttpSessionStateBaseMock = new Mock<HttpSessionStateBase>();
            HttpContextMock.Setup(x => x.Session).Returns(HttpSessionStateBaseMock.Object);

            Controller = CreateControllerTestInitialize();
            
            Controller.ControllerContext = new ControllerContext()
            {
                HttpContext = HttpContextMock.Object
            };
        }

        protected abstract TController CreateControllerTestInitialize();

        protected abstract string GetUserIdTestInitialize();

        protected void AssertThatReturnBadRequest()
        {
            HttpResponseBaseMock.VerifySet(x => x.StatusCode = 400);
            HttpResponseBaseMock.VerifySet(x => x.TrySkipIisCustomErrors = true);
        }

        protected void AssertThatReturnGoodRequest()
        {
            HttpResponseBaseMock.VerifySet(x => x.StatusCode = 400, Times.Never);
        }
    }
}