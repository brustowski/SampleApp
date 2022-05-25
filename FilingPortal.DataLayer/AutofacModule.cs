using Autofac;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Repositories;
using FilingPortal.Parts.Common.DataLayer.Repositories;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using FilingPortal.Parts.Common.DataLayer.Services;
using FilingPortal.Parts.Common.Domain.Repositories;
using FilingPortal.Parts.Common.Domain.Services.DB;
using Framework.DataLayer;

namespace FilingPortal.DataLayer
{
    /// <summary>
    /// Class for type registrations in autofac
    /// </summary>
    public class AutofacModule : Module
    {
        /// <summary>
        /// Overrides registrations to the specified container
        /// </summary>
        /// <param name="builder">The builder through which components can be registered</param>
        /// <remarks>Note that the ContainerBuilder parameter is unique to this module</remarks>
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = GetType().Assembly;

            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWorkFilingPortalContext>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWorkFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(UnitOfWorkFactory<>)).As(typeof(IUnitOfWorkFactory<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(BaseTablesRepository<>)).As(typeof(ITablesRepository<>))
                .InstancePerLifetimeScope();
            builder.RegisterType<FilingPortalContextFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<EventBusSyncWrapper>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<SqlQueryExecutor>().As<ISqlQueryExecutor>().InstancePerLifetimeScope();
            builder.RegisterType<UniqueConstraintsRegister>().AsImplementedInterfaces().SingleInstance();
        }
    }
}