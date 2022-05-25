using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Enums;
using FilingPortal.Parts.Common.Domain.Enums;
using FilingPortal.Parts.Inbond.Domain.Entities;
using FilingPortal.Parts.Inbond.Domain.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Parts.Inbond.Domain.Tests.Validators
{
    [TestClass]
    public class InbondValidatorTests
    {
        private InbondValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new InbondValidator();
        }

        private static InboundReadModel CreateValidModel(Action<InboundReadModel> action = null)
        {
            var model = new InboundReadModel
            {
                Id = 1,
                FirmsCode = "A001",
                ImporterCode= "ADVENGTEC",
                PortOfArrival = "2240",
                PortOfDestination = "0009",
                ExportConveyance = "ec",
                ConsigneeCode = "ADVENGTEC",
                Carrier = "BUCCARHOU",
                Value = 12M,
                ManifestQty = 21M,
                ManifestQtyUnit = "T",
                Weight = 22M,
                EntryDate = DateTime.Now,
                HasEntryRule = true,
                FilingStatus = FilingStatus.Open,
                MappingStatus = MappingStatus.Open,
                CreatedDate = DateTime.Now,
                CreatedUser = "test_user"
            };
            action?.Invoke(model);
            return model;
        }

        [TestMethod]
        public void GetErrors_ReturnsEmptyListOfErrors_IfModelIsValid()
        {
            InboundReadModel model = CreateValidModel();

            List<string> result = _validator.GetErrors(model);

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void GetErrors_ReturnsError_IfHasEntryRuleFalse()
        {
            InboundReadModel model = CreateValidModel(x=>x.HasEntryRule = false);

            List<string> result = _validator.GetErrors(model);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(result.Any(x => x.Equals("Entry rule is missing")));
        }
    }
}
