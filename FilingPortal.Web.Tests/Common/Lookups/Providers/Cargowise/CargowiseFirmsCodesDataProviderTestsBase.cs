using FilingPortal.PluginEngine.Lookups;
using FilingPortal.Web.Common.Lookups;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Web.Tests.Common.Lookups.Providers.Cargowise
{
    public abstract class DataProviderTestsBase<TDataProvider> where TDataProvider: ILookupDataProvider
    {
        protected TDataProvider Provider { get; set; }

        [TestInitialize]
        public void Init()
        {
            Provider = InitProvider();
        }

        protected abstract TDataProvider InitProvider();
    }
}