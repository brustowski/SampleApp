using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Parts.Common.Domain.Enums;
using Framework.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Entities
{
    [TestClass]
    public class ClientReadModelTest
    {
        private Client entity;

        [TestInitialize]
        public void TestInitialize()
        {
            entity = new Client();
        }

        [TestMethod]
        public void Type_WithSupplierFlag_ContainsSupplier()
        {
            entity.Supplier = true;
            Assert.IsTrue(entity.ClientType.Contains(ClientType.Supplier.GetDescription()));
        }

        [TestMethod]
        public void Type_WithImporterFlag_ContainsImporter()
        {
            entity.Importer = true;
            Assert.IsTrue(entity.ClientType.Contains(ClientType.Importer.GetDescription()));
        }

        [TestMethod]
        public void Type_WithoutTypeFlags_IsEmpty()
        {
            Assert.IsTrue(string.IsNullOrEmpty(entity.ClientType));
        }
    }
}
