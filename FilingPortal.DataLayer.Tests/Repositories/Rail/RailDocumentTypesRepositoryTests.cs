using System.Data.Entity;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.DocumentTypes.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Migrations;
using System.Linq;
using Framework.DataLayer;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailDocumentTypesRepositoryTests : RepositoryTestBase
    {
        private DocumentTypesRepository _repository;

        protected override void TestInit()
        {
            _repository = new DocumentTypesRepository(UnitOfWorkFactory);
        }

        [TestMethod]
        public void GetFilteredData_ChangingData_ReturnsUpdated()
        {
            var docType1 = new DocumentType { TypeName = "type1", Description = "2342" };
            var docType2 = new DocumentType { TypeName = "type2", Description = null };

            DbContext.DocumentTypes.Add(docType1);
            DbContext.DocumentTypes.Add(docType2);
            DbContext.SaveChanges();

            var result1 = _repository.GetFilteredData(null, 20).ToList();

            docType2.Description = "test";
            DbContext.DocumentTypes.AddOrUpdate(docType2);
            DbContext.SaveChanges();

            var result2 = _repository.GetFilteredData(null, 20).ToList();

            Assert.AreEqual(result2.Count, result1.Count);
            Assert.AreNotEqual(result1.First(x => x.TypeName == "type2").Description, result2.First(x => x.TypeName == "type2").Description);
            Assert.AreEqual("test", result2.First(x => x.TypeName == "type2").Description);
        }
    }
}