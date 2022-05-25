using Autofac;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.PluginEngine.Common.Extensions;
using System.Reflection;
using Module = Autofac.Module;

namespace FilingPortal.PluginEngine.Autofac
{
    /// <summary>
    /// Base domain autofac module.
    /// </summary>
    public abstract class PluginDomainModule : Module
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
            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Validator")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAllTypesThatImplementsInterface<IReportConfig>(Assembly);
            builder.RegisterAllTypesThatImplementsInterface<IReportDatasource>(Assembly);
            builder.RegisterAllTypesThatImplementsInterface<IReportModelMap>(Assembly);
            builder.RegisterAllTypesThatImplementsInterface<IImportConfiguration>(Assembly);
            builder.RegisterAllTypesThatImplementsInterface<IParseModelMap>(Assembly);

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
