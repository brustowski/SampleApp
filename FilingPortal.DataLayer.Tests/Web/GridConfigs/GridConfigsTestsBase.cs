using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core.Lifetime;
using AutoMapper;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Services;
using FilingPortal.Parts.Common.DataLayer.Services;
using FilingPortal.Parts.Common.Domain.Services.DB;
using FilingPortal.PluginEngine.Common.Extensions;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.PluginEngine.GridConfigurations.Columns;
using FilingPortal.PluginEngine.GridConfigurations.Filters;
using FilingPortal.Web.Mapping;
using Framework.DataLayer;
using Framework.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Web.GridConfigs
{
    [TestClass]
    public class GridConfigsTestsBase
    {
        private IEnumerable<IGridConfiguration> _configsRegistry;
        private LifetimeScope _scope;

        [TestInitialize]
        public void Init()
        {
            IContainer container = BuildContainer();

            _scope = new LifetimeScope(container.ComponentRegistry);
            _scope.BeginLifetimeScope();

            _configsRegistry = _scope.Resolve<IEnumerable<IGridConfiguration>>();

            Mapper.Reset();
            AutoMapperConfiguration.Init(container);
        }

        public virtual Assembly ConfigsAssembly => Assembly.GetAssembly(typeof(FilingPortal.Web.AutofacModule));
        public virtual Assembly RepositoriesAssembly => Assembly.GetAssembly(typeof(AutofacModule));

        public IContainer BuildContainer()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<KeyFieldsService>().AsImplementedInterfaces();
            builder.RegisterAllTypesThatImplementsInterface<IGridConfiguration>(ConfigsAssembly);
            builder.RegisterAllTypesThatImplementsInterface<IDbStructureService>(RepositoriesAssembly);
            builder.RegisterAssemblyTypes(RepositoriesAssembly)
                .Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<UniqueConstraintsRegister>().AsImplementedInterfaces();
            builder.RegisterType<SqlQueryExecutor>().AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(UnitOfWorkFactory<>)).As(typeof(IUnitOfWorkFactory<>)).InstancePerLifetimeScope();
            builder.RegisterType<TestUnitOfWorkFactory>().AsImplementedInterfaces();
            builder.RegisterType<EventBusSyncWrapper>().AsImplementedInterfaces().InstancePerLifetimeScope();

            RegisterContextFactory(builder);

            IContainer container = builder.Build();

            return container;
        }

        protected virtual void RegisterContextFactory(ContainerBuilder builder)
        {
            
        }


        [TestCleanup]
        public void Fin()
        {
            Mapper.Reset();
            _scope.Dispose();
        }

        [TestMethod]
        public void ConfigRegistry_is_not_empty()
        {
            Assert.IsTrue(_configsRegistry.Any());
        }

        [TestMethod]
        public void TestKeyColumns_Contains_Filters()
        {
            List<string> result = new List<string>();

            foreach (IGridConfiguration configuration in _configsRegistry)
            {
                configuration.Configure();

                IEnumerable<ColumnConfig> columns = configuration.GetColumns();
                List<FilterConfig> filters = configuration.GetFilters().ToList();

                foreach (ColumnConfig columnConfig in columns)
                {
                    result.AddIf(columnConfig.IsKeyField && filters.All(x => x.FieldName != columnConfig.FieldName),
                        $"Config {configuration.GetType()} doesn't contain filter for {columnConfig.FieldName}");
                }
            }

            Assert.IsFalse(result.Any(), string.Join("\n", result));
        }
    }
}
