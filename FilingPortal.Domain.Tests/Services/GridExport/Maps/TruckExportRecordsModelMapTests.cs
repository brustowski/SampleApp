using FilingPortal.Domain.Services.GridExport.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FilingPortal.Domain.Tests.Services.GridExport.Maps
{
    [TestClass()]
    public class TruckExportRecordsModelMapTests
    {
        TruckExportRecordsModelMap modelMap;

        [TestInitialize]
        public void Init()
        {
            modelMap = new TruckExportRecordsModelMap();
        }

        [TestMethod()]
        public void GetColumnInfos_No_ID_field()
        {
            var columns = modelMap.GetColumnInfos();
            Assert.IsFalse(columns.Any(x => x.Title == "Id"));
        }

        [TestMethod()]
        public void GetColumnInfos_No_IsDeleted_field()
        {
            var columns = modelMap.GetColumnInfos();
            Assert.IsFalse(columns.Any(x => x.Title == "IsDeleted"));
        }

        [TestMethod()]
        public void GetColumnInfos_No_FilingHeaderId_field()
        {
            var columns = modelMap.GetColumnInfos();
            Assert.IsFalse(columns.Any(x => x.Title == "FilingHeaderId"));
        }
    }
}