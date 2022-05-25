using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Repositories.Common;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.Common.Lookups.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;
using FilingPortal.Cargowise.Domain.Repositories;
using FilingPortal.PluginEngine.DataProviders;
using FilingPortal.PluginEngine.Lookups;
using Framework.Domain.Paging;

namespace FilingPortal.Web.Tests.Common.Lookups.Providers
{
    [TestClass]
    public class DischargePortCountryDataProviderTest
    {
        private Mock<ICountryRepository> _countryRepositoryMock;
        private Mock<IForeignPortsRepository> _foreignPortRepository;
        private DischargePortCountryDataProvider _provider;

        [TestInitialize]
        public void TestInitialize()
        {
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _foreignPortRepository = new Mock<IForeignPortsRepository>();
            _provider = new DischargePortCountryDataProvider(_countryRepositoryMock.Object, _foreignPortRepository.Object);
        }

        [TestMethod]
        public void Name_Returns_ValidDataProviderName()
        {
            Assert.AreEqual(DataProviderNames.DischargePortCountries, _provider.Name);
        }
        private static List<Country> GetCountries()
        {
            return new List<Country>()
            {
                new Country{Id = 1, Name = "Canada", Code = "CA"},
                new Country{Id = 30, Name = "USA", Code = "US"},
                new Country{Id = 73, Name = "MEXICO", Code = "MX"}
            };
        }

        [TestMethod]
        public void GetData_Returns_LookupItemWithValidDisplayValue()
        {
            List<Country> countries = GetCountries();
            _countryRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(countries);
            var searchInfo = new SearchInfo(string.Empty, 0);
            List<LookupItem> result = _provider.GetData(searchInfo).ToList();
            Assert.AreEqual($"{countries[0].Code} - {countries[0].Name}", result.First().DisplayValue);
        }

        [TestMethod]
        public void GetData_ReturnsLookupItemCollection()
        {
            List<Country> countries = GetCountries();
            _countryRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(countries);
            var searchInfo = new SearchInfo(string.Empty, 0);
            List<LookupItem> result = _provider.GetData(searchInfo).ToList();
            Assert.AreEqual(3, result.Count());
            for (int i = 0; i < countries.Count; i++)
            {
                Assert.AreEqual(countries[i].Id, result[i].Value);
            }
        }

        [TestMethod]
        public void GetData_CallsCountryRepository()
        {
            _countryRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(GetCountries());
            var searchInfo = new SearchInfo(string.Empty, 0);
            List<LookupItem> result = _provider.GetData(searchInfo).ToList();
            _countryRepositoryMock.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<int>()));
        }

        [TestMethod]
        public void GetData_CallsCountryRepository_WithValidSearchInfo()
        {
            _countryRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(GetCountries());
            var searchInfo = new SearchInfo("canada", 10);
            List<LookupItem> result = _provider.GetData(searchInfo).ToList();
            _countryRepositoryMock.Verify(x => x.Search("canada", 10));
        }

        [TestMethod]
        public void GetData_WithoutDependency_CallsOnlyCountryRepository()
        {
            _countryRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(GetCountries());
            var searchInfo = new SearchInfo("canada", 10);
            List<LookupItem> result = _provider.GetData(searchInfo).ToList();
            _countryRepositoryMock.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<int>()));
            _countryRepositoryMock.Verify(x => x.GetByCode(It.IsAny<string>()), Times.Never);
            _foreignPortRepository.Verify(x => x.GetByCode(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GetData_WithoutDependentValue_CallsOnlyCountryRepository()
        {
            _countryRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(GetCountries());
            var searchInfo = new SearchInfo("canada", 10)
            {
                DependOn = "DischargePort"
            };
            List<LookupItem> result = _provider.GetData(searchInfo).ToList();
            _countryRepositoryMock.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<int>()));
            _countryRepositoryMock.Verify(x => x.GetByCode(It.IsAny<string>()), Times.Never);
            _foreignPortRepository.Verify(x => x.GetByCode(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GetData_WhenCountryCodeNotFound_CallsOnlyCountryRepository()
        {
            _countryRepositoryMock.Setup(x => x.Search(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(GetCountries());
            _foreignPortRepository.Setup(x => x.GetByCode(It.IsAny<string>())).Returns<ForeignPort>(null);
            var searchInfo = new SearchInfo("canada", 10)
            {
                DependOn = "DischargePort",
                DependValue = "00000"
            };
            List<LookupItem> result = _provider.GetData(searchInfo).ToList();
            _countryRepositoryMock.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<int>()));
            _foreignPortRepository.Verify(x => x.GetByCode("00000"));
            _countryRepositoryMock.Verify(x => x.GetByCode(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GetData_WhenCountryCodeFound_CallsGetByCodeCountryRepository()
        {
            _foreignPortRepository.Setup(x => x.GetByCode(It.IsAny<string>()))
                .Returns(new ForeignPort{Id = 1, PortCode = "0000", Country = "CA" });
            _countryRepositoryMock.Setup(x => x.GetByCode("CA"))
                .Returns(GetCountries().First());
            var searchInfo = new SearchInfo("canada", 10)
            {
                DependOn = "DischargePort",
                DependValue = "00000"
            };
            List<LookupItem> result = _provider.GetData(searchInfo).ToList();
            _countryRepositoryMock.Verify(x => x.Search(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
            _foreignPortRepository.Verify(x => x.GetByCode("00000"));
            _countryRepositoryMock.Verify(x => x.GetByCode(It.IsAny<string>()));
        }
    }
}
