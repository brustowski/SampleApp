﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Tests
{
    [TestClass]
    public class DocumentsViewTests : DbTestContext
    {
        [TestMethod]
        public void FilingHeaderTable_ContainsExternalColumns()
        {
            const string command = "SELECT OBJECT_DEFINITION(OBJECT_ID('dbo.v_documents'));";
            var result = DbContext.Database.SqlQuery<string>(command).First();

            Assert.IsTrue(result.Contains($"{DbContext.DefaultSchema}.documents"));
        }
    }
}
