using FilingPortal.DataLayer.Repositories.Rail;
using FilingPortal.DataLayer.Tests.Common;
using FilingPortal.Domain.Entities.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using FilingPortal.DataLayer.Repositories.Common;
using FilingPortal.Parts.Common.DataLayer.Repositories.Base;

namespace FilingPortal.DataLayer.Tests.Repositories.Rail
{
    [TestClass]
    public class RailTablesRepositoryTests : RepositoryTestBase
    {
        private BaseTablesRepository<RailTables> _repository;

        protected override void TestInit()
        {
            _repository = new BaseTablesRepository<RailTables>(UnitOfWorkFactory);
        }

        private string GetTableViewQuery()
        {
            return @"SELECT
                      CAST(ROW_NUMBER() OVER (ORDER BY i.TABLE_NAME, i.COLUMN_NAME) AS INT) AS id
                     ,i.TABLE_NAME AS TableName
                     ,i.COLUMN_NAME AS ColumnName
                     ,s.id AS SectionId
                     ,s.title AS SectionTitle
                    FROM information_schema.columns i
                    INNER JOIN imp_rail_form_section_configuration s
                      ON i.TABLE_NAME = s.table_name
                    WHERE i.TABLE_SCHEMA = 'dbo'
                    AND LOWER(column_name) NOT IN ('id', 'created_date', 'created_user', 'filing_header_id', 'parent_record_id', 'operation_id', 'source_record_id', 'broker_download_id')";
        }

        [TestMethod]
        public void GetTableNames_ReturnsAllAvailableTableNames()
        {
            var sql = GetTableViewQuery();

            var tableNamesCount = DbContext.Database.SqlQuery<RailTables>(sql).Select(x => x.TableName).Distinct().Count();

            var result = _repository.GetTableNames().Count();

            Assert.AreEqual(tableNamesCount, result);
        }

        [TestMethod]
        public void GetColumnNamesByTableName_WithSpecifiedTableName_ReturnsAllAvailableColumnNames()
        {
            var sql = GetTableViewQuery();

            IQueryable<RailTables> tables = DbContext.Database.SqlQuery<RailTables>(sql).AsQueryable();
            var tableName = tables.First().TableName;
            var columns = tables.Where(x => x.TableName == tableName).Select(x=>x.ColumnName).ToList();

            var result = _repository.GetByTableName(tableName).Select(x=>x.ColumnName).ToList();

            var difference = columns.Except(result).ToList();

            Assert.IsTrue(!difference.Any(), $"This columns are absent: {string.Join("; ", difference)}");
            Assert.AreEqual(columns.Count(), result.Count());
        }

        [TestMethod]
        public void GetColumnNames_ReturnsAllAvailableColumnNames()
        {
            var sql = GetTableViewQuery();

            IQueryable<RailTables> tables = DbContext.Database.SqlQuery<RailTables>(sql).AsQueryable();
            var columnsCount = tables.Count();

            var result = _repository.GetAllAsQueryable().Count();

            Assert.AreEqual(columnsCount, result);
        }
    }
}
