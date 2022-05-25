using FilingPortal.DataLayer.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Linq;

namespace FilingPortal.DataLayer.Tests
{
    /// <summary>
    /// Summary description for FilingHeaderExternalColumnsTests
    /// </summary>
    [TestClass]
    public class FilingHeaderExternalColumnsTests : DbTestContextBase<FilingPortalContext, UnitOfWorkFilingPortalContext>
    {
        protected override UnitOfWorkFilingPortalContext CreateUoW()
        {
            return new UnitOfWorkFilingPortalContext(new FilingPortalContextFactory(), null);
        }

        [DataRow("error_description", "exp_vessel_filing_header")]
        [DataRow("response_xml", "exp_vessel_filing_header")]
        [DataRow("request_xml", "exp_vessel_filing_header")]
        [DataRow("error_description", "imp_pipeline_filing_header")]
        [DataRow("response_xml", "imp_pipeline_filing_header")]
        [DataRow("request_xml", "imp_pipeline_filing_header")]
        [DataRow("error_description", "imp_vessel_filing_header")]
        [DataRow("response_xml", "imp_vessel_filing_header")]
        [DataRow("request_xml", "imp_vessel_filing_header")]
        [DataRow("error_description", "imp_truck_filing_header")]
        [DataRow("response_xml", "imp_truck_filing_header")]
        [DataRow("request_xml", "imp_truck_filing_header")]
        [DataRow("error_description", "exp_truck_filing_header")]
        [DataRow("response_xml", "exp_truck_filing_header")]
        [DataRow("request_xml", "exp_truck_filing_header")]
        [DataRow("error_description", "imp_rail_filing_header")]
        [DataRow("response_xml", "imp_rail_filing_header")]
        [DataRow("request_xml", "imp_rail_filing_header")]
        [DataTestMethod]
        public void FilingHeaderTable_ContainsNecessaryExternalColumns(string column, string table)
        {
            var columnName = new SqlParameter("@column", column);
            var tableName = new SqlParameter("@table", $"{DbContext.DefaultSchema}.{table}");
            const string command = "SELECT count (*) FROM sys.columns AS clmn WHERE clmn.name = @column AND clmn.object_id = OBJECT_ID(@table);";
            var result = DbContext.Database.SqlQuery<int>(command, columnName, tableName).First();
            Assert.AreEqual(1, result);
        }
    }
}
