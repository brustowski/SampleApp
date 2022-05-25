using Autofac;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.Parts.Rail.Export.Web.Configs;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Rail.Export.Web
{
    public class AutofacModule : PluginWebModule
    {
        /// <summary>
        /// Adds registrations to the container
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void LoadPluginTypes(ContainerBuilder builder)
        {
            builder.RegisterType<DefValueService<DefValue, Section, Tables>>().AsImplementedInterfaces();
            builder.RegisterType<RecordFieldBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
