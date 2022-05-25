using System;
using System.Collections.Generic;
using FilingPortal.Web.Common.Lookups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Common.Lookups
{
    [TestClass]
    public class LookupDataProviderRegistryTest
    {
        private LookupDataProviderRegistry _registry;
        private Mock<ILookupDataProvider> _dataProviderMock;


        [TestInitialize]
        public void TestInitialize()
        {
            _dataProviderMock = new Mock<ILookupDataProvider>();
            _dataProviderMock.Setup(x => x.Name).Returns("test_data_provider");
            _registry = new LookupDataProviderRegistry(new[] { _dataProviderMock.Object });
        }

        [TestMethod]
        public void GetProvider_WithSpecifiedType_ReturnsDataProvider()
        {
            // Assign
            const string providerName = "test_data_provider";
            Type providerType = _dataProviderMock.Object.GetType();

            // Act
            ILookupDataProvider result = _registry.GetProvider(providerType);

            // Assert
            Assert.AreEqual(providerName, result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetProvider_WithSpecifiedType_ThrowException_IfDataProviderNotFound()
        {
            // Assign
            Type providerType = typeof(ILookupDataProvider);

            // Act
            _registry.GetProvider(providerType);

            // Assert - Expects exception
        }

        [TestMethod]
        public void GetProvider_WithSpecifiedName_ReturnsDataProvider()
        {
            // Assign
            const string providerName = "test_data_provider";

            // Act
            ILookupDataProvider result = _registry.GetProvider(providerName);

            // Assert
            Assert.AreEqual(providerName, result.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetProvider_WithSpecifiedName_ThrowException_IfDataProviderNotFound()
        {
            // Assign
            const string providerName = "nonexistent_data_provider";

            // Act
            _registry.GetProvider(providerName);

            // Assert - Expects exception
        }
    }
}
