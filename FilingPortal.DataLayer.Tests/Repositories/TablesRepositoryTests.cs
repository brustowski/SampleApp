using System.Collections.Generic;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.DataLayer.Tests.Repositories
{
    [TestClass]
    public class TablesRepositoryTests : RepositoryTestBase
    {
        private BaseTablesRepository<RailTables> _repository;

        protected override void TestInit()
        {
            _repository = new BaseTablesRepository<RailTables>(UnitOfWorkFactory);
        }

        [TestMethod]
        public void GetTableName_ReturnsCollection()
        {
            IEnumerable<string> result = _repository.GetTableNames();

            Assert.IsInstanceOfType(result, typeof(IEnumerable<string>));
        }

        [TestMethod]
        public void GetColumnName_ReturnsCollection()
        {
            IQueryable<RailTables> tables = _repository.GetByTableName(string.Empty);
            IEnumerable<string> result = tables.Select(x => x.ColumnName);

            Assert.IsInstanceOfType(result, typeof(IEnumerable<string>));
        }

    }
}
