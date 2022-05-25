using FilingPortal.Domain.Enums;
using FilingPortal.Web.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcRouteTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using FilingPortal.PluginEngine.Authorization;

namespace FilingPortal.Web.Tests.Controllers
{
    public abstract class ApiControllerTestsBase<T> where T : ApiController
    {
        public const string EmptyData = "{}";

        [TestInitialize]
        public virtual void TestInitializeInternal()
        {
            Configuration = new HttpConfiguration();
            WebApiConfig.Register(Configuration);
            Configuration.EnsureInitialized();
        }

        protected HttpConfiguration Configuration { get; set; }

        protected void AssertRoute(HttpMethod httpMethod, string uri, Expression<Func<T, object>> lambda)
        {
            Configuration.ShouldMap(httpMethod, uri).To(lambda);
        }

        protected void AssertRoute(HttpMethod httpMethod, string uri, string jsonBody, Expression<Func<T, object>> lambda)
        {
            CheckHttpMethodForBody(httpMethod);
            Configuration.ShouldMap(httpMethod, uri).WithJsonBody(jsonBody).To<T>(lambda);
        }

        protected void AssertRoute(HttpMethod httpMethod, string uri, Expression<Action<T>> lambda)
        {
            Configuration.ShouldMap(httpMethod, uri).To<T>(lambda);
        }

        protected void AssertRoute(HttpMethod httpMethod, string uri, string jsonBody, Expression<Action<T>> lambda)
        {
            CheckHttpMethodForBody(httpMethod);
            Configuration.ShouldMap(httpMethod, uri).WithJsonBody(jsonBody).To<T>(lambda);
        }

        private void CheckHttpMethodForBody(HttpMethod httpMethod)
        {
            if (httpMethod != HttpMethod.Post && httpMethod != HttpMethod.Put)
            {
                throw new ArgumentOutOfRangeException("httpMethod", httpMethod, "In this case accept the POST and PUT only.");
            }
        }

        protected static MethodInfo MethodOf(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MethodCallExpression exp)
            {
                return exp.Method;
            }

            var unary = (UnaryExpression)expression.Body;
            return ((MethodCallExpression)unary.Operand).Method;
        }

        protected void AssertPermissions(Permissions permission, Expression<Func<T, object>> lambda)
        {
            MethodInfo methodInfo = MethodOf(lambda);
            Attribute attribute = methodInfo.GetCustomAttributes(typeof(PermissionRequiredAttribute)).SingleOrDefault();
            if (attribute is PermissionRequiredAttribute permissionAttribute)
            {
                CollectionAssert.Contains(permissionAttribute.RequiredPermissions, permission);
            }
            else
            {
                Assert.Fail("No PermissionRequired Attribute defined");
            }
        }

        /// <summary>
        /// If your controller have methods that should be opened to any request, override this method
        /// </summary>
        protected virtual IEnumerable<MethodInfo> GetOpenMethods()
        {
            return Enumerable.Empty<MethodInfo>();
        }

        [TestMethod]
        public void All_Routed_Methods_Have_Permissions_Set()
        {
            MethodInfo[] methods = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo methodInfo in methods.Except(GetOpenMethods()))
            {
                IEnumerable<Attribute> routeAttributes = methodInfo.GetCustomAttributes(typeof(RouteAttribute));
                if (routeAttributes.Any())
                {
                    IEnumerable<Attribute> permissionAttributes = methodInfo.GetCustomAttributes(typeof(PermissionRequiredAttribute));
                    if (!permissionAttributes.Any())
                    {
                        Assert.Fail($"Method {methodInfo.Name} of {typeof(T).Name} doesn't have Permissions attribute");
                    }
                }
            }
        }
    }
}
