using System.Collections.Generic;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders.Cargowise;
using FilingPortal.PluginEngine.Lookups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Common.Lookups.Providers.Cargowise
{
    [TestClass]
    public class CargowiseFirmsCodesDataProviderTests : DataProviderTestsBase<CargowiseFirmsCodesDataProvider>
    {
        private Mock<IFirmsCodesRepository> _repository;

        protected override CargowiseFirmsCodesDataProvider InitProvider()
        {
            _repository = new Mock<IFirmsCodesRepository>();

            return new CargowiseFirmsCodesDataProvider(_repository.Object);
        }

        [TestMethod]
        public void CargowiseFirmsCodesDataProviderTest()
        {
            Assert.AreEqual(Provider.Name, "CargowiseFirmsCodes");
        }

        [TestMethod]
        public void GetDataTest_Doesnt_Return_Null()
        {
            // Assign

            var searchData = new Mock<SearchInfo>("Test", 100, false);
            _repository.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new List<CargowiseFirmsCodes>());

            // Act
            IEnumerable<Framework.Domain.Paging.LookupItem> result = Provider.GetData(searchData.Object);
            // Assert

            Assert.IsNotNull(result);
        }
    }
}