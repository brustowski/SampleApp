using Autofac;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Validators;
using FilingPortal.Parts.Zones.Ftz214.Domain.Dtos;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;
using FilingPortal.Parts.Zones.Ftz214.Domain.Services.Refile;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Zones.Ftz214.Domain
{
    public class AutofacModule : PluginDomainModule
    {
        /// <summary>
        /// Adds registrations to the container
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void LoadPluginTypes(ContainerBuilder builder)
        {
            builder.RegisterType<FilingHeaderDocumentUpdateService<DocumentDto, Document>>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RefileAssistant>().AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
