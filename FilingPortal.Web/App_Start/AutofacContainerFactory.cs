using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using Autofac.Integration.WebApi;
using FilingPortal.PluginEngine.Autofac;
using FilingPortal.Web.Common.Plugins;

namespace FilingPortal.Web
{
    /// <summary>
    /// Service for Autofac container creation
    /// </summary>
    public static class AutofacContainerFactory
    {
        /// <summary>
        /// Builds the Autofac container which creates, wires dependencies and manages lifetime for a set of components
        /// </summary>
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Union(PluginsConfiguration.GetAssemblies())
                .ToArray();

            builder.RegisterApiControllers(
                assemblies
            );

            RegisterAllModules(builder);
            var container = builder.Build();

            return container;
        }

        /// <summary>
        /// Registers all modules found in the assemblies using the specified Autofac container builder
        /// </summary>
        /// <param name="builder">The Autofac container builder</param>
        public static void RegisterAllModules(ContainerBuilder builder)
        {
            // Register plugin autofac modules
            builder.RegisterAssemblyModules(PluginsConfiguration.GetAssemblies());

            builder.RegisterAssemblyModules(typeof(DataLayer.AutofacModule).Assembly);
            builder.RegisterAssemblyModules(typeof(Domain.AutofacModule).Assembly);
            builder.RegisterAssemblyModules(typeof(Infrastructure.AutofacModule).Assembly);
            builder.RegisterAssemblyModules(typeof(AutofacModule).Assembly);
            builder.RegisterAssemblyModules(typeof(Parts.Common.DataLayer.AutofacModule).Assembly);
            builder.RegisterAssemblyModules(typeof(Cargowise.DataLayer.AutofacModule).Assembly);
            builder.RegisterAssemblyModules(typeof(Local).Assembly);
        }
    }
}