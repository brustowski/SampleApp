using Autofac;
using FilingPortal.Parts.Recon.Domain.Specifications;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Recon.Domain
{
    public class AutofacModule : PluginDomainModule
    {
        /// <summary>
        /// Adds registrations to the container
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void LoadPluginTypes(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<InboundRecordSpecification>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<FtaReconCustomSpecification>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ValueReconCustomSpecification>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
