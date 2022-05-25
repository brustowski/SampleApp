using Autofac;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.CanadaTruckImport.Domain.Dtos;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.CanadaTruckImport.Domain
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

            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
