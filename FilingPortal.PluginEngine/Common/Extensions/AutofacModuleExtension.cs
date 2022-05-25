using System.Reflection;
using Autofac;

namespace FilingPortal.PluginEngine.Common.Extensions
{
    /// <summary>
    /// Autofac modules extensions
    /// </summary>
    public static class AutofacModuleExtension
    {
        /// <summary>
        /// Registers all types that implements specified interface
        /// </summary>
        /// <param name="builder">The autofac builder</param>
        /// <param name="assembly">Assembly where to search for types</param>
        public static void RegisterAllTypesThatImplementsInterface<T>(this ContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
                .InstancePerLifetimeScope().AsImplementedInterfaces();
        }
    }
}
