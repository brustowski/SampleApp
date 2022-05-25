using System.Reflection;
using Autofac;
using FilingPortal.DataLayer.Tests.Web.GridConfigs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Parts.Inbond.DataLayer.Tests.Web.GridConfigs
{
    [TestClass]
    public class GridConfigsTests: GridConfigsTestsBase
    {
        public override Assembly ConfigsAssembly => Assembly.GetAssembly(typeof(Inbond.Web.AutofacModule));
        public override Assembly RepositoriesAssembly => Assembly.GetAssembly(typeof(AutofacModule));

        protected override void RegisterContextFactory(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWorkInbondContext>().AsSelf();
            builder.RegisterType<InbondContextFactory>().AsImplementedInterfaces();
        }
    }
}
