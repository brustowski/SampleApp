using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Routing;

namespace FilingPortal.Web.Tests
{
    [TestClass]
    public class RouteConfigTests
    {
        [DataRow("send-request")]
        [DataRow("imports/{*url}")]
        [DataRow("rules/{*url}")]
        [DataRow("admin/{*url}")]
        [DataRow("client-management")]
        [DataTestMethod]
        [Ignore] // todo: restore test
        public void RegisterRoutes_ContainsRoute_True(string urlTemplate)
        {
            // Assign
            RouteCollection routes = new RouteCollection();

            // Act
            RouteConfig.RegisterRoutes(routes);
            Route resultRoute = routes.OfType<Route>()
                .FirstOrDefault(route => route.Url == urlTemplate);

            // Assert
            Assert.IsNotNull(resultRoute);
        }
    }
}
