using System.Data.Entity;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.PluginEngine.Common;
using FilingPortal.Web.App_Start;
using FilingPortal.Web.Common;
using FilingPortal.Web.Common.Plugins;
using FilingPortal.Web.Helpers;
using FilingPortal.Web.Mapping;
using FilingPortal.Web.Validators;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(FilingPortal.Web.Startup))]
namespace FilingPortal.Web
{
    /// <summary>
    /// Class for services and the application's request pipeline configuration
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Executes before application starts
        /// </summary>
        public static void PreStart()
        {
            // Add the plugin as a reference to the application
            var assemblies = PluginsConfiguration.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                BuildManager.AddReferencedAssembly(assembly);
                BuildManager.AddCompilationDependency(assembly.FullName);
            }
        }

        /// <summary>
        /// Configures the application using the specified application builder
        /// </summary>
        /// <param name="app">The application builder</param>
        public void Configuration(IAppBuilder app)
        {
            DbConfiguration.SetConfiguration(new FpConfiguration());

            var config = new HttpConfiguration();
            config.MapInheritedAttributeRoutes();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            ViewEngines.Engines.ConfigureViewEngine();

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(config);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            config.Services.Add(typeof(IExceptionLogger), new NLogExceptionLogger());

            var container = AutofacContainerFactory.BuildContainer();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            FluentValidation.Mvc.FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new AutofacValidatorFactory(config));
            FluentValidation.WebApi.FluentValidationModelValidatorProvider.Configure(config, x => x.ValidatorFactory = new AutofacValidatorFactory(config));

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseAutofacMvc();
            app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);

            AutoMapperConfiguration.Init(container);

            // initialize plugins
            container.Resolve(typeof(PluginsEngine));
        }
    }
}