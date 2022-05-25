using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Tests
{
    /// <summary>
    /// Summary description for FilingHeaderExternalColumnsTests
    /// </summary>
    [TestClass]
    public class DocumentsExternalColumnsTests : DbTestContext
    {
        [DataRow("status", "documents")]
        [DataTestMethod]
        public void FilingHeaderTable_ContainsExternalColumns(string column, string table)
        {
            var columnName = new SqlParameter("@column", column);
            var tableName = new SqlParameter("@table", $"{DbContext.DefaultSchema}.{table}");
            const string command = "SELECT count (*) FROM sys.columns AS clmn WHERE clmn.name = @column AND clmn.object_id = OBJECT_ID(@table);";
            var result = DbContext.Database.SqlQuery<int>(command, columnName, tableName).First();
            Assert.AreEqual(1, result);
        }
    }
}
