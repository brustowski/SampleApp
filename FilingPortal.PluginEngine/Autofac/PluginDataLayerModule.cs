using System.Reflection;
using Autofac;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.Common.Extensions;
using Module = Autofac.Module;

namespace FilingPortal.PluginEngine.Autofac
{
    /// <summary>
    /// Base data layer autofac module.
    /// </summary>
    public abstract class PluginDataLayerModule : Module
    {
        /// <summary>
        /// Assembly of derived type
        /// </summary>
        protected Assembly Assembly => GetType().Assembly;

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAllTypesThatImplementsInterface<IPluginDatabaseInit>(Assembly);
        }
    }
}
