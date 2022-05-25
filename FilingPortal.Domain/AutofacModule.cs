using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Common.Import;
using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.Common.Refile.TruckExport;
using FilingPortal.Domain.Common.Reporting;
using FilingPortal.Domain.Common.Reporting.ReportDataSource;
using FilingPortal.Domain.DTOs.Rail;
using FilingPortal.Domain.DTOs.Truck;
using FilingPortal.Domain.DTOs.TruckExport;
using FilingPortal.Domain.DTOs.Vessel;
using FilingPortal.Domain.DTOs.VesselExport;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using FilingPortal.Domain.Services;
using FilingPortal.Domain.Services.GridExport;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using FilingPortal.Domain.Services.Pipeline;
using FilingPortal.Domain.Specifications.Clients;
using FilingPortal.Domain.Specifications.Rail;
using FilingPortal.Domain.Validators;
using Framework.Domain.Commands;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FilingPortal.Domain.Services.TruckExport;
using FilingPortal.Domain.Specifications.TruckExport;
using Module = Autofac.Module;

namespace FilingPortal.Domain
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
            System.Reflection.Assembly assembly = GetType().Assembly;

            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>().InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(RuleService<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(RuleValidator<>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(FilingWorkflow<,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            
            builder.RegisterGeneric(typeof(FilingParametersService<,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(SingleFilingGridWorker<,,>)).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DefValuesManualValidator<>)).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DefValueService<,,>)).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(TemplateProcessingService<,>)).As(typeof(ITemplateProcessingService<,>))
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(FormDataTemplateProcessingService<,>)).As(typeof(IFormDataTemplateProcessingService<,>))
                .InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(TemplateValidationService<>)).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Factory")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Validator")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(assembly).Where(t => t.Name.EndsWith("Generator")).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<ConsolidatedFilingWorkflow<RailFilingHeader, RailDefValuesManual>>()
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<SpecificationBuilder>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<CustomSpecificationsRegistry>().AsImplementedInterfaces().SingleInstance();

            // DefValues Services
            builder.RegisterType<DefValueService<RailDefValues, RailSection, RailTables>>().AsImplementedInterfaces();
            builder.RegisterType<DefValueService<PipelineDefValue, PipelineSection, PipelineTable>>()
                .AsImplementedInterfaces();
            builder.RegisterType<DefValueService<TruckDefValue, TruckSection, TruckTable>>().AsImplementedInterfaces();
            builder.RegisterType<DefValueService<TruckExportDefValue, TruckExportSection, TruckExportTable>>()
                .AsImplementedInterfaces();
            builder.RegisterType<DefValueService<VesselImportDefValue, VesselImportSection, VesselImportTable>>()
                .AsImplementedInterfaces();
            builder.RegisterType<DefValueService<VesselExportDefValue, VesselExportSection, VesselExportTable>>()
                .AsImplementedInterfaces();

            builder.RegisterType<FilingHeaderDocumentUpdateService<RailDocumentDto, RailDocument>>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FilingHeaderDocumentUpdateService<TruckDocumentDto, TruckDocument>>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<PipelineFilingHeaderDocumentUpdateService>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FilingHeaderDocumentUpdateService<TruckExportDocumentDto, TruckExportDocument>>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FilingHeaderDocumentUpdateService<VesselDocumentDto, VesselImportDocument>>()
                .AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<FilingHeaderDocumentUpdateService<VesselExportDocumentDto, VesselExportDocument>>()
                .AsImplementedInterfaces().SingleInstance();

            // grid export
            builder.RegisterType<ReportExportingService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<ReportConfigRegistry>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ReportFiltersBuilder>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ReportBodyBuilder>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ReportModelMapContainer>().AsImplementedInterfaces().SingleInstance();
            RegisterAllTypesThatImplementsInterface<IReportModelMap>(builder).SingleInstance();

            builder.RegisterType<DefaultFormattersRegistry>().AsImplementedInterfaces().SingleInstance();
            RegisterAllTypesThatImplementsInterface<IValueFormatter>(builder).SingleInstance();

            builder.RegisterType<ReportDataSourceResolver>().AsImplementedInterfaces().InstancePerLifetimeScope();
            RegisterAllTypesThatImplementsInterface<IReportDatasource>(builder).InstancePerLifetimeScope();

            RegisterExportToExcelRuleDataSources(builder);

            // import
            builder.RegisterType<ImportConfigurationRegistry>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ParseModelMapRegistry>().AsImplementedInterfaces().SingleInstance();
            RegisterAllTypesThatImplementsInterface<IParseModelMap>(builder).SingleInstance();
            RegisterAllTypesThatImplementsInterface<IImportConfiguration>(builder).SingleInstance();

            builder.RegisterType<FieldsValidationResultBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<TruckExportRefileAssistant>().AsImplementedInterfaces();
            builder.RegisterType<RailTypeSpecification>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<TruckExportHasUpdatedSpecification>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<ClientTypeSpecification>().AsImplementedInterfaces().InstancePerDependency();
        }

        private void RegisterExportToExcelRuleDataSources(ContainerBuilder builder)
        {
            // Rail
            builder.RegisterType<RuleRecordsDataSource<RailRuleImporterSupplier>>()
                .WithParameter("gridName", GridNames.RailRuleImporterSupplier).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<RailRuleDescription>>()
                .WithParameter("gridName", GridNames.RailRuleDescription).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<RailRulePort>>()
                .WithParameter("gridName", GridNames.RailRulePort).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Pipeline
            builder.RegisterType<RuleRecordsDataSource<PipelineRuleImporter>>()
                .WithParameter("gridName", GridNames.PipelineRuleImporter).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<PipelineRuleBatchCode>>()
                .WithParameter("gridName", GridNames.PipelineRuleBatchCode).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<PipelineRuleFacility>>()
                .WithParameter("gridName", GridNames.PipelineRuleFacility).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<PipelineRuleConsigneeImporter>>()
                .WithParameter("gridName", GridNames.PipelineRuleConsigneeImporter).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Truck
            builder.RegisterType<RuleRecordsDataSource<TruckRuleImporter>>()
                .WithParameter("gridName", GridNames.TruckRuleImporter).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<TruckRulePort>>()
                .WithParameter("gridName", GridNames.TruckRulePort).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<TruckExportRuleConsignee>>()
                .WithParameter("gridName", GridNames.TruckExportRuleConsignee).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<TruckExportRuleExporterConsignee>>()
                .WithParameter("gridName", GridNames.TruckExportRuleExporterConsignee).AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            // Vessel
            builder.RegisterType<RuleRecordsDataSource<VesselRuleProduct>>()
                .WithParameter("gridName", GridNames.VesselRuleProduct).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType<RuleRecordsDataSource<VesselExportRuleUsppiConsignee>>()
                .WithParameter("gridName", GridNames.VesselExportRuleUsppiConsignee).AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        private static IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle>
            RegisterAllTypesThatImplementsInterface<T>(ContainerBuilder builder, Assembly assembly = null)
        {
            Assembly searchAssembly = assembly ?? typeof(T).Assembly;

            Debug.WriteLine(string.Join(",",
                searchAssembly.GetTypes()
                    .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
            ));
            return builder.RegisterAssemblyTypes(searchAssembly)
                .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract)
                .AsImplementedInterfaces();
        }
    }
}