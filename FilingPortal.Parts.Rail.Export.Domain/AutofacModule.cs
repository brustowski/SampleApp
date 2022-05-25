using Autofac;
using FilingPortal.Domain.Common.Reporting.ReportDataSource;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Rail.Export.Domain.Config;
using FilingPortal.Parts.Rail.Export.Domain.Dtos;
using FilingPortal.Parts.Rail.Export.Domain.Entities;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Rail.Export.Domain
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

            builder.RegisterType<RuleRecordsDataSource<RuleConsignee>>()
                .WithParameter("gridName", GridNames.RuleConsignee).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<RuleExporterConsignee>>()
                .WithParameter("gridName", GridNames.RuleExporterConsignee).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
