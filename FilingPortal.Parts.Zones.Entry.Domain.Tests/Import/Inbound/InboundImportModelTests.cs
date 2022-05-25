using System;
using FilingPortal.Parts.Rail.Export.Domain.Import.Inbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Parts.Rail.Export.Domain.Tests.Import.Inbound
{
    [TestClass]
    public class InboundImportModelTests
    {
        private InboundImportModel CreateValidModel(Action<InboundImportModel> action = null)
        {
            var model = new InboundImportModel
            {
                GroupId = "1",
                Exporter = "SHETRAHOU",
                Importer = "AKTLOGTAM",
                MasterBill = "SLE200362",
                ContainerNumber = "CBTX735075",
                LoadPort = "2304",
                ExportPort = "2304",
                Carrier = "UNKN",
                TariffType = "HTS",
                Tariff = "2710.19.1106",
                GoodsDescription = "DIESEL FUEL - ULSD",
                CustomsQty = 662.833M,
                Price = 35375.01M,
                GrossWeight = 82.441M,
                GrossWeightUOM = "T"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void ContainerNumber_ReturnsValueWithoutSpaces()
        {
            InboundImportModel model = CreateValidModel(x=>x.ContainerNumber = "CBTX 735075");
            Assert.AreEqual("CBTX735075", model.ContainerNumber);
        }
    }
}
