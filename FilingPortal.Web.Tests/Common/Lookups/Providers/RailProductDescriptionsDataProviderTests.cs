using Microsoft.VisualStudio.TestTools.UnitTesting;
using FilingPortal.Web.Common.Lookups.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilingPortal.Domain.Repositories.Rail;
using Moq;
using FilingPortal.PluginEngine.Lookups;

namespace FilingPortal.Web.Common.Lookups.Providers.Tests
{
    [TestClass()]
    public class RailProductDescriptionsDataProviderTests
    {
        private Mock<IRailRuleProductDescriptionsRepository> _repository;
        RailProductDescriptionsDataProvider provider;

        [TestInitialize]
        public void Init()
        {
            _repository = new Mock<IRailRuleProductDescriptionsRepository>();

            provider = new RailProductDescriptionsDataProvider(_repository.Object);
        }

        [TestMethod()]
        public void GetDataTest_All_Data()
        {
            // assign
            SearchInfo searchInfo = new SearchInfo("test", 3, false);
            _repository.Setup(x => x.GetDescriptions("test")).Returns(new List<string>
            {
                "Autotest 1",
                "Autotest 2",
                "Autotest 3",
                "Test",
                "Test 1"
            });

            // act
            var results = provider.GetData(searchInfo);

            var stringCollection = results.Select(x => x.Value.ToString()).ToList();

            // Assert
            CollectionAssert.AreEqual(new[] { "Test", "Test 1", "Autotest 1" }, stringCollection);
        }

        [TestMethod()]
        public void GetDataTest_Only_starts_with()
        {
            // assign
            SearchInfo searchInfo = new SearchInfo("test", 3, false);
            _repository.Setup(x => x.GetDescriptions("test")).Returns(new List<string>
            {
                "Autotest 1",
                "Autotest 2",
                "Autotest 3",
                "Test 1",
                "Test 2"
            });

            // act
            var results = provider.GetData(searchInfo);

            var stringCollection = results.Select(x => x.Value.ToString()).ToList();

            // Assert
            CollectionAssert.AreEqual(new[] { "Test 1", "Test 2", "Autotest 1" }, stringCollection);
        }

        [TestMethod()]
        public void GetDataTest_Only_Contains()
        {
            // assign
            SearchInfo searchInfo = new SearchInfo("test", 3, false);
            _repository.Setup(x => x.GetDescriptions("test")).Returns(new List<string>
            {
                "Autotest 1",
                "Autotest 2",
                "Autotest 3",
                "Autotest 4",
                "Autotest 5"
            });

            // act
            var results = provider.GetData(searchInfo);

            var stringCollection = results.Select(x => x.Value.ToString()).ToList();

            // Assert
            CollectionAssert.AreEqual(new[] { "Autotest 1", "Autotest 2", "Autotest 3" }, stringCollection);
        }
    }
}