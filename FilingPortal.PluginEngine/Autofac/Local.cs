using System.Reflection;
using Autofac;
using FilingPortal.Domain.Common;
using FilingPortal.Parts.Common.Domain.Common;
using FilingPortal.PluginEngine.Common;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using Module = Autofac.Module;

namespace FilingPortal.PluginEngine.Autofac
{
    /// <summary>
    /// PluginEngine common DI register
    /// </summary>
    public class Local : Module
    {
        /// <summary>
        /// Current assembly
        /// </summary>
        protected Assembly Assembly => Assembly.GetExecutingAssembly();

        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            RegisterAllTypesThatImplementsInterface<ILookupDataProvider>(builder);
            builder.RegisterType<HandbookDataProviderRegistry>().AsImplementedInterfaces().InstancePerLifetimeScope();
            RegisterAllTypesThatImplementsInterface<IFilterDataProvider>(builder);
            builder.RegisterType<PluginsEngine>().AsSelf();
            builder.RegisterType<CompatibleDataTypeService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            RegisterAllTypesThatImplementsInterface<IFilingParametersHandler>(builder);
        }

        /// <summary>
        /// Registers all types that implements specified interface
        /// </summary>
        /// <param name="builder">The autofac builder</param>
        private void RegisterAllTypesThatImplementsInterface<T>(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly)
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
                .InstancePerLifetimeScope().AsImplementedInterfaces();
        }
    }
}
