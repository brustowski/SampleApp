using Autofac;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Zones.Ftz214.Web
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
        }
    }
}
