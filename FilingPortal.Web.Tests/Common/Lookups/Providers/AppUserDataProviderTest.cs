using System.Linq;
using FilingPortal.Domain.AppSystem.Repositories;
using FilingPortal.Web.Common.Lookups;
using FilingPortal.Web.Common.Lookups.Providers;
using Framework.Domain.Paging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Common.Lookups.Providers
{
    [TestClass]
    public class AppUserDataProviderTest
    {
        private AppUserInfoProvider _provider;
        private Mock<IAppUsersRepository> _repositoryMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<IAppUsersRepository>();
            _repositoryMock.Setup(x => x.GetLogins(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new[] { "first", "second", "third" });
            _provider = new AppUserInfoProvider(_repositoryMock.Object);
        }

        [TestMethod]
        public void Name_WhenCalled_ReturnsCorrectProviderName()
        {
            Assert.AreEqual("AppUser", _provider.Name);
        }

        [TestMethod]
        public void GetData_CallsRepository_WhenCalled()
        {
            var searchInfo = new SearchInfo("first", 10);

            _provider.GetData(searchInfo);

            _repositoryMock.Verify(x=>x.GetLogins("first", 10), Times.Once);
        }

        [TestMethod]
        public void GetData_Returns_ValidLookupItem()
        {
            var searchInfo = new SearchInfo(string.Empty, 0);

            LookupItem result = _provider.GetData(searchInfo).First();
            Assert.AreEqual("first", result.DisplayValue);
            Assert.AreEqual("first", result.Value);
        }
    }
}
