using FilingPortal.Domain.Entities.Truck;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Documents.Truck.Entities
{
    [TestClass]
    public class TruckDocumentTests
    {
        TruckDocument truckDocument;

        [TestInitialize]
        public void Initialize()
        {
            truckDocument = new TruckDocument();
        }

        [TestMethod]
        public void TruckFilingHeader_is_auto_property()
        {
            truckDocument.TruckFilingHeader = new TruckFilingHeader()
            {
                Id = 41,
            };

            Assert.AreEqual(truckDocument.TruckFilingHeader.Id, 41);
        }
    }
}
