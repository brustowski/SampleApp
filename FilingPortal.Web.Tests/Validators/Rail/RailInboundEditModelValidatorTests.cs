using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Rail;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Web.Models.Rail;
using FilingPortal.Web.Validators.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Web.Tests.Validators.Rail
{
    [TestClass()]
    public class RailInboundEditModelValidatorTests
    {

        private RailInboundEditModelValidator _validator;
        private Mock<IRailFilingHeadersRepository> _filingHeadersRepository;

        [TestInitialize]
        public void Init()
        {
            _filingHeadersRepository = new Mock<IRailFilingHeadersRepository>();
            _validator = new RailInboundEditModelValidator(_filingHeadersRepository.Object);
        }

        public RailInboundEditModel CreateValidModel()
        {
            return new RailInboundEditModel
            {
                Importer = "Importer",
                Supplier = "Supplier",
                Consignee = "Consignee",
                Description = "Description",
                Destination = "LA",
                EquipmentInitial = "ABC1",
                EquipmentNumber = "ABC123",
                IssuerCode = "CODE1",
                BillOfLading = "084872402019",
                PortOfUnlading = "1234",
                ManifestUnits = "TNK",
                Weight = "100.56",
                WeightUnit = "KG",
                ReferenceNumber = "153805"
            };
        }

        [TestMethod]
        public void ValidModel_Passes_Validation()
        {
            // Assign
            var model = CreateValidModel();

            // Act
            var validationResult = _validator.Validate(model);

            // Assert
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestMethod]
        public void InProgress_FilingHeader_Not_Pass_Validation()
        {
            // Assign
            var model = CreateValidModel();
            model.Id = 5;

            var header = new Mock<RailFilingHeader>();
            header.Object.MappingStatus = MappingStatus.InProgress;

            var headers = new[] {header.Object};

            _filingHeadersRepository.Setup(x => x.FindByInboundRecordIds(It.Is<IEnumerable<int>>(y => y.Contains(5))))
                .Returns(headers.AsEnumerable());

            // Act
            var validationResult = _validator.Validate(model);

            // Assert
            Assert.IsFalse(validationResult.IsValid);
        }

        [TestMethod]
        public void Open_FilingHeader_Passes_Validation()
        {
            // Assign
            var model = CreateValidModel();
            model.Id = 5;

            var header = new Mock<RailFilingHeader>();
            header.Object.MappingStatus = MappingStatus.Open;

            var headers = new[] { header.Object };

            _filingHeadersRepository.Setup(x => x.FindByInboundRecordIds(It.Is<IEnumerable<int>>(y => y.Contains(5))))
                .Returns(headers.AsEnumerable());

            // Act
            var validationResult = _validator.Validate(model);

            // Assert
            Assert.IsTrue(validationResult.IsValid);
        }

        // TODO: Add other validation checks
    }
}