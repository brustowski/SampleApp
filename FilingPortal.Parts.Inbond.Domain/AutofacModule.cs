using Autofac;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Inbond.Domain.DTOs;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Validators;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Inbond.Domain
{
    public class AutofacModule : PluginDomainModule
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void LoadPluginTypes(ContainerBuilder builder)
        {
            builder.RegisterType<FilingHeaderDocumentUpdateService<DocumentDto, Document>>()
                .AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<DefValuesManualValidator>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
