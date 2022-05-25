using Autofac;
using FilingPortal.PluginEngine.Common.Extensions;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.PluginEngine.PageConfigs;
using System.Reflection;
using Module = Autofac.Module;

namespace FilingPortal.PluginEngine.Autofac
{
    /// <summary>
    /// Base web autofac module.
    /// </summary>
    public abstract class PluginWebModule : Module
    {
        /// <summary>
        /// Assembly of derived type
        /// </summary>
        protected Assembly Assembly => GetType().Assembly;

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected sealed override void Load(ContainerBuilder builder)
        {
            builder.RegisterAllTypesThatImplementsInterface<IGridConfiguration>(Assembly);
            builder.RegisterAllTypesThatImplementsInterface<IFilterDataProvider>(Assembly);
            builder.RegisterAllTypesThatImplementsInterface<IPageConfiguration>(Assembly);
            builder.RegisterAllTypesThatImplementsInterface<ILookupDataProvider>(Assembly);

            LoadPluginTypes(builder);
        }

        /// <summary>
        /// Adds registrations to the container
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected virtual void LoadPluginTypes(ContainerBuilder builder)
        {

        }
    }
}
