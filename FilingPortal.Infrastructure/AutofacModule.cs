using Autofac;
using FilingPortal.Infrastructure.ManifestBuilder;
using FilingPortal.Infrastructure.Parsing;
using FilingPortal.Infrastructure.Report;
using FilingPortal.Infrastructure.Services;
using FilingPortal.Infrastructure.TemplateEngine;
using Module = Autofac.Module;

namespace FilingPortal.Infrastructure
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReportFilenameProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ExcelReporterFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ReportingService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<EmailNotificationService>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<ExcelFileSimpleParser>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ManifestRazorFormatter>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ExcelDocumentBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<TemplateService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
