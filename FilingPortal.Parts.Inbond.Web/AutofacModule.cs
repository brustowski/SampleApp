using Autofac;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Web.FieldConfigurations;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Inbond.Web
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
            builder.RegisterType<ComplexFieldRule>().AsImplementedInterfaces();
            
        }
    }
}
