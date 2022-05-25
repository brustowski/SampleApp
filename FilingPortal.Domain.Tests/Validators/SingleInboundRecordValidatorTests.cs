using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Validators.Rail;
using FilingPortal.Parts.Common.Domain.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Domain.Tests.Validators
{
    [TestClass]
    public class SingleInboundRecordValidatorTests
    {
        private RailInboundValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new RailInboundValidator();
        }

        [TestMethod]
        public void GetErrors_ReturnsEmptyList_IfValid()
        {
            var record = new RailInboundReadModel { Importer = "i", Supplier = "s", HTS = "00001111" };

            var result = _validator.GetErrors(record);

            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetErrors_ReturnsImporterSupplierError_IfImporterEmpty()
        {
            var record = new RailInboundReadModel
            {
                Importer = "",
                Supplier = "s",
                HTS = "00001111",
                BdParsedImporterConsignee = "bdparsed i",
                BdParsedSupplier = "bdparsed s",
            };

            var result = _validator.GetErrors(record);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Rule is missed for Importer = \"bdparsed i\" and Supplier= \"bdparsed s\"", result.First());
        }

        [TestMethod]
        public void GetErrors_ReturnsImporterSupplierError_IfImporterNull()
        {
            var record = new RailInboundReadModel
            {
                Importer = null,
                Supplier = "s",
                HTS = "00001111",
                BdParsedImporterConsignee = "bdparsed i",
                BdParsedSupplier = "bdparsed s",
            };

            var result = _validator.GetErrors(record);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Rule is missed for Importer = \"bdparsed i\" and Supplier= \"bdparsed s\"", result.First());
        }

        [TestMethod]
        public void GetErrors_ReturnsImporterSupplierError_IfSupplierEmpty()
        {
            var record = new RailInboundReadModel
            {
                Importer = "i",
                Supplier = "",
                HTS = "00001111",
                BdParsedImporterConsignee = "bdparsed i",
                BdParsedSupplier = "bdparsed s",
            };

            var result = _validator.GetErrors(record);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Rule is missed for Importer = \"bdparsed i\" and Supplier= \"bdparsed s\"", result.First());
        }

        [TestMethod]
        public void GetErrors_ReturnsImporterSupplierError_IfSupplierNull()
        {
            var record = new RailInboundReadModel
            {
                Importer = "i",
                Supplier = null,
                HTS = "00001111",
                BdParsedImporterConsignee = "bdparsed i",
                BdParsedSupplier = "bdparsed s",
            };

            var result = _validator.GetErrors(record);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Rule is missed for Importer = \"bdparsed i\" and Supplier= \"bdparsed s\"", result.First());
        }

        [TestMethod]
        public void GetErrors_ReturnsHTSError_IfHTSEmpty()
        {
            var record = new RailInboundReadModel
            {
                Importer = "i",
                Supplier = "s",
                HTS = "",
                BdParsedDescription1 = "bdparsed descr1",
                PortCode = "1102",
                Destination = "LA"
            };

            var result = _validator.GetErrors(record);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Rule is missed for Description 1 = \"bdparsed descr1\", Importer = \"i\", Supplier = \"s\", Port (optional) = \"1102\" and Destination (optional) = \"LA\"", result.First());
        }

        [TestMethod]
        public void GetErrors_ReturnsHTSError_IfHTSNull()
        {
            var record = new RailInboundReadModel
            {
                Importer = "i",
                Supplier = "s",
                HTS = null,
                BdParsedDescription1 = "bdparsed descr1",
                PortCode = "1102",
                Destination = "LA"
            };

            var result = _validator.GetErrors(record);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Rule is missed for Description 1 = \"bdparsed descr1\", Importer = \"i\", Supplier = \"s\", Port (optional) = \"1102\" and Destination (optional) = \"LA\"", result.First());
        }

        [TestMethod]
        public void GetErrors_ReturnsFilingProcessError_IfFillingStatusIsError()
        {
            var record = new RailInboundReadModel
            {
                Importer = "i",
                Supplier = "s",
                HTS = "00001111",
                FilingStatus = FilingStatus.Error,
            };

            List<string> result = _validator.GetErrors(record);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual<string>(ErrorMessages.FilingProcessErrorMessage, result.First());
        }

    }
}