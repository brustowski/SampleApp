using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using FilingPortal.Infrastructure.ManifestBuilder;
using FilingPortal.PluginEngine.Authorization;
using FilingPortal.PluginEngine.FieldConfigurations;
using FilingPortal.PluginEngine.FieldConfigurations.InboundRecordParameters;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.PluginEngine.Lookups;
using FilingPortal.PluginEngine.PageConfigs;
using FilingPortal.Web.Common.Grids;
using FilingPortal.Web.Common.Grids.Filters;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.Common.Lookups.Providers;
using FilingPortal.Web.Common.Plugins;
using FilingPortal.Web.FieldConfigurations;
using FilingPortal.Web.FieldConfigurations.InboundRecordParameters;
using FilingPortal.Web.FieldConfigurations.Pipeline;
using FilingPortal.Web.FieldConfigurations.Truck;
using FilingPortal.Web.PageConfigs.Common;
using FilingPortal.Web.Validators.TruckExport;
using FluentValidation;
using System;
using System.Linq;
using System.Reflection;
using Module = Autofac.Module;

namespace FilingPortal.Web
{
    /// <summary>
    /// Class for registering user-defined types of the Web layer in Autofac IoC-container
    /// </summary>
    public class AutofacModule : Module
    {
        /// <summary>
        /// Adds registrations to the container using the specified builder
        /// </summary>
        /// <param name="builder">The builder through which components can be registered</param>
        protected override void Load(ContainerBuilder builder)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Union(PluginsConfiguration.GetAssemblies())
                .ToArray();
            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic).As<Profile>();

            builder.RegisterControllers(assemblies);

            //FluentValidation
            AssemblyScanner.FindValidatorsInAssemblies(assemblies.Where(p => !p.IsDynamic))
                .ForEach(result =>
                {
                    builder.RegisterType(result.ValidatorType).As(result.InterfaceType).SingleInstance();
                });


            RegisterAllTypesThatImplementsInterface<IGridConfiguration>(builder);
            RegisterAllTypesThatImplementsInterface<IFilterDataProvider>(builder);
            RegisterAllTypesThatImplementsInterface<ILookupDataProvider>(builder);
            RegisterAllTypesThatImplementsInterface<IPageConfiguration>(builder);


            // Automapper
            builder.RegisterAssemblyTypes(assemblies).Where(x =>
                    x.GetTypeInfo().ImplementedInterfaces.Any(i =>
                        i.IsGenericType &&
                        (i.GetGenericTypeDefinition() == typeof(ITypeConverter<,>)
                         || i.GetGenericTypeDefinition() == typeof(IValueResolver<,,>)
                         || i.GetGenericTypeDefinition() == typeof(IMemberValueResolver<,,,>))))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(AgileComplexFieldRule<>)).AsImplementedInterfaces().SingleInstance();
            builder.RegisterGeneric(typeof(AgileDropdownFieldRule<>)).AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<HandbookDataProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<PermissionRequiredAttribute>().PropertiesAutowired();

            builder.RegisterType<FilterDataProviderRegistry>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<LookupDataProviderRegistry>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GridConfigRegistry>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Factory")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<FieldConfigurationBuilder>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ValueTypeConverter>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<InboundRecordConfigurationBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<TruckInboundFieldsConfigurationBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<PipelineInboundFieldsConfigurationBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(InboundRecordFieldBuilder<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<PipelineRecordFieldBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<PageConfigContainer>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<PdfBuilder>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TemplatesProviderService>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<TruckExportViewModelValidator>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers all types that implements specified interface
        /// </summary>
        /// <param name="builder">The autofac builder</param>
        private static void RegisterAllTypesThatImplementsInterface<T>(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
                .InstancePerLifetimeScope().AsImplementedInterfaces();
        }

        /// <summary>
        /// Registers all types that implements specified interface
        /// </summary>
        /// <param name="builder">The autofac builder</param>
        private static void RegisterAllTypesThatImplementsInterfaceAsSelf<T>(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
                .InstancePerLifetimeScope().AsSelf();
        }

    }
}