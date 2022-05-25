using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Audit.Rail;
using FilingPortal.Domain.Services.AppSystem;
using FilingPortal.Domain.Validators;
using FilingPortal.Domain.Validators.Audit.Rail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Validators.Audit.Rail
{
    [TestClass]
    public class RailDailyAuditInboundValidatorTests
    {
        public RailDailyAuditInboundValidator Validator;
        private Mock<IRailDailyAuditRulesRepository> _rulesRepository;
        private Mock<ISettingsService> _settingsService;
        private Mock<IRailDailyAuditSpiRulesRepository> _spiRulesRepository;
        private Mock<IFieldsValidationResultBuilder> _fieldsValidationResultBuilder;

        public Mock<AuditRailDaily> Record { get; set; }
        public Mock<AuditRailDailyRule> Rule { get; private set; }

        [TestInitialize]
        public void Init()
        {
            _rulesRepository = new Mock<IRailDailyAuditRulesRepository>();
            _settingsService = new Mock<ISettingsService>();
            _spiRulesRepository = new Mock<IRailDailyAuditSpiRulesRepository>();
            _settingsService.Setup(x => x.Get<decimal>(SettingsNames.RailDailyAuditCustomsQtyWarningThreshold))
                .Returns(0.05m);
            _settingsService.Setup(x => x.Get<decimal>(SettingsNames.RailDailyAuditCustomsQtyErrorThreshold))
                .Returns(0.1m);
            _fieldsValidationResultBuilder = new Mock<IFieldsValidationResultBuilder>();

            Validator = new RailDailyAuditInboundValidator(
                _rulesRepository.Object, 
                _settingsService.Object, 
                _spiRulesRepository.Object,
                _fieldsValidationResultBuilder.Object
                );

            Record = new Mock<AuditRailDaily>();
            Rule = new Mock<AuditRailDailyRule>();
            _rulesRepository.Setup(x => x.FindCorrespondingRules(Record.Object))
                .Returns(Task.FromResult<IList<AuditRailDailyRule>>(new List<AuditRailDailyRule> { Rule.Object }));
        }

        [TestMethod]
        public async Task Validation_passes_if_empty_corresponding_rule_found()
        {
            // Assign
            var record = new Mock<AuditRailDaily>();
            record.Object.CustomsAttrib2 = " =20"; // Any valid format for API

            var rule = new Mock<AuditRailDailyRule>();
            _rulesRepository.Setup(x => x.FindCorrespondingRules(record.Object))
                .Returns(Task.FromResult<IList<AuditRailDailyRule>>(new List<AuditRailDailyRule> { rule.Object }));

            await AssertNoError(record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_no_rules_found()
        {
            // Assign
            var record = new Mock<AuditRailDaily>();
            _rulesRepository.Setup(x => x.FindCorrespondingRules(record.Object))
                .Returns(Task.FromResult<IList<AuditRailDailyRule>>(new List<AuditRailDailyRule>()));

            // Act
            await AssertSingleError("No corresponding rule found", record.Object);
        }

        [TestMethod]
        public async Task Validation_failes_if_multiple_corresponding_rules_found()
        {
            // Assign
            var record = new Mock<AuditRailDaily>();
            var rule = new Mock<AuditRailDailyRule>();
            _rulesRepository.Setup(x => x.FindCorrespondingRules(record.Object))
                .Returns(Task.FromResult<IList<AuditRailDailyRule>>(new List<AuditRailDailyRule> { rule.Object, rule.Object }));

            // Act
            await AssertSingleError("Several validation rules found", record.Object);
        }

        [TestMethod]
        public async Task String_comparison_is_trimming_values()
        {
            // Assign
            Record.Object.Tariff = "11001"; // without spaces
            Rule.Object.Tariff = " 11001 "; // with spaces

            // Assert
            await AssertNoError(Record.Object);
        }

        private async Task AssertSingleError(string errorMessage, AuditRailDaily record)
        {
            // Setup
            int callsCount = 0;

            _fieldsValidationResultBuilder.Setup(
                x => x.Add(
                    It.Is<string>(s=>s == errorMessage), 
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<Severity>()))
                .Callback(() => callsCount++);
            _fieldsValidationResultBuilder.Setup(
                x => x.AddIf(
                    It.Is<bool>(b => b), 
                    It.Is<string>(s => s == errorMessage), 
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<Severity>()))
                .Callback(() => callsCount++);

            // Act
            await Validator.GetErrorsAsync(record);

            // Assert
            Assert.AreEqual(1, callsCount, $"Error \"{errorMessage}\" was occured {callsCount} times");

            _fieldsValidationResultBuilder.Verify(
                x => x.AddIf(
                    true, 
                    It.Is<string>(m=>m != errorMessage),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Severity>()), Times.Never);
            _fieldsValidationResultBuilder.Verify(
                x => x.Add(
                    It.Is<string>(m => m != errorMessage),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Severity>()), Times.Never);
        }

        private async Task AssertNoError(AuditRailDaily record)
        {
            int callsCount = 0;

            _fieldsValidationResultBuilder.Setup(
                x => x.Add(It.IsAny<string>(), It
                    .IsAny<
                        string>(), It.IsAny<string>(), It.IsAny<Severity>())).Callback(() => callsCount++);
            _fieldsValidationResultBuilder.Setup(
                x => x.AddIf(It.Is<bool>(b => b), It.IsAny<string>(), It
                .IsAny<
                    string>(), It.IsAny<string>(), It.IsAny<Severity>())).Callback(() => callsCount++);

            // Act
            await Validator.GetErrorsAsync(record);

            Assert.AreEqual(0, callsCount);
        }

        [TestMethod]
        public async Task Validation_fails_if_tariff_and_rule_tariff_are_different()
        {
            // Assign
            Record.Object.Tariff = "112001";
            Rule.Object.Tariff = " 11001 ";

            // Act
            await AssertSingleError("Tariff should be  11001 ", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Goods_description_doesnt_contain()
        {
            // Assign
            Record.Object.GoodsDescription = "1";
            Rule.Object.GoodsDescription = "2";

            // Act
            await AssertSingleError("Goods description doesn't contain 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_CustomsQtyUnit_is_different()
        {
            // Assign
            Record.Object.CustomsQtyUnit = "1";
            Rule.Object.CustomsQtyUnit = "2";

            // Act
            await AssertSingleError("Customs Qty Unit should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Entry_Port_is_different_from_Port()
        {
            // Assign
            Record.Object.EntryPort = "1";
            Record.Object.ArrivalPort = "2";// Should be equal to Port
            Rule.Object.PortCode = "2";

            // Act
            await AssertSingleError(
                "Entry Port should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Arrival_Port_is_different_from_Port()
        {
            // Assign
            Record.Object.ArrivalPort = "1";
            Record.Object.EntryPort = "2"; // Should be equal to Port
            Rule.Object.PortCode = "2";

            // Act
            await AssertSingleError(
                "Arrival Port should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Destination_State_is_different()
        {
            // Assign
            Record.Object.DestinationState = "1";
            Rule.Object.DestinationState = "2";

            // Act
            await AssertSingleError(
                "Destination State should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Country_of_export_is_different()
        {
            // Assign
            Record.Object.CountryOfExport = "1";
            Rule.Object.ExportingCountry = "2";

            // Act
            await AssertSingleError(
                "Country of Export should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Country_of_origin_is_different()
        {
            // Assign
            Record.Object.CountryOfOrigin = "1";
            Rule.Object.CountryOfOrigin = "2";

            // Act
            await AssertSingleError(
                "Country of Origin should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Ultimate_Consignee_name_is_different()
        {
            // Assign
            Record.Object.UltimateConsigneeName = "1";
            Rule.Object.UltimateConsigneeName = "2";

            // Act
            await AssertSingleError(
                "Ultimate Consignee Name should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_FIRMs_Code_is_different()
        {
            // Assign
            Record.Object.FirmsCode = "1";
            Rule.Object.FirmsCode = "2";

            // Act
            await AssertSingleError(
                "FIRMs Code should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_SPI_is_different()
        {
            // Assign
            Record.Object.Spi = "1";
            Record.Object.ImportDate = new DateTime(2020, 2, 10);

            _spiRulesRepository.Setup(x => x.FindCorrespondingRules(Record.Object)).ReturnsAsync(
                new List<AuditRailDailySpiRule>()
                {
                    new AuditRailDailySpiRule()
                    {
                        Spi = "2"
                    }
                }
            );

            // Act
            await AssertSingleError(
                "SPI should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_NAFTA_Recon_is_different()
        {
            // Assign
            Record.Object.NaftaRecon = null;
            Rule.Object.NaftaRecon = "Y";

            // Act
            await AssertSingleError(
                "NAFTA Recon should be Y", Record.Object);
        }
        [TestMethod]
        public async Task Validation_passes_if_NAFTA_Recon_is_Y()
        {
            // Assign
            Record.Object.NaftaRecon = "Y";
            Rule.Object.NaftaRecon = "Y";

            // Act
            await AssertNoError(Record.Object);
        }
        [TestMethod]
        public async Task Validation_passes_if_NAFTA_Recon_is_N_and_null()
        {
            // Assign
            Record.Object.NaftaRecon = null;
            Rule.Object.NaftaRecon = "N";

            // Act
            await AssertNoError(Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_NAFTA_Recon_is_N_and_Y()
        {
            // Assign
            Record.Object.NaftaRecon = "Y";
            Rule.Object.NaftaRecon = "N";

            // Act
            await AssertSingleError(
                "NAFTA Recon should be N", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Value_Recon_is_different()
        {
            // Assign
            Record.Object.ValueRecon = "1";
            Rule.Object.ValueRecon = "2";

            // Act
            await AssertSingleError(
                "Value Recon should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Manufacturer_MID_is_different()
        {
            // Assign
            Record.Object.ManufacturerMid = "1";
            Rule.Object.ManufacturerMid = "2";

            // Act
            await AssertSingleError(
                "Manufacturer MID should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Invoice_Qty_Unit_is_different()
        {
            // Assign
            Record.Object.InvoiceQtyUnit = "1";
            Rule.Object.InvoiceQtyUnit = "2";

            // Act
            await AssertSingleError(
                "Invoice Qty Unit should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Customs_Qty_Unit_is_different()
        {
            // Assign
            Record.Object.CustomsQtyUnit = "1";
            Rule.Object.CustomsQtyUnit = "2";

            // Act
            await AssertSingleError(
                "Customs Qty Unit should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Gross_Weight_Unit_is_different()
        {
            // Assign
            Record.Object.GrossWeightUq = "1";
            Rule.Object.GrossWeightUq = "2";

            // Act
            await AssertSingleError(
                "Gross Weight Uq should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_fails_if_Customs_Attribute_1_not_equal_to_rule_value()
        {
            // Assign
            Record.Object.CustomsAttrib1 = "1";
            Rule.Object.CustomsAttrib1 = "2";

            // Act
            await AssertSingleError(
                "Customs Attribute 1 should be 2", Record.Object);
        }

        [TestMethod]
        public async Task Validation_passes_if_Customs_Attribute_1_equals_rule_value()
        {
            // Assign
            Record.Object.CustomsAttrib1 = "Hello";
            Rule.Object.CustomsAttrib1 = "Hello";

            // Act
            await AssertNoError(Record.Object);
        }

        [TestMethod]
        public async Task CustomsAttrib2_is_validating_only_for_tariff_starting_with_27()
        {
            // Assign
            Record.Object.Tariff = "27";
            Record.Object.CustomsAttrib2 = "Wrong Format";

            // Act
            await AssertSingleError(
                "Customs Attribute 2 has wrong format", Record.Object);
        }

        [TestMethod]
        public async Task Customs_Qty_validation_more_then_15_percent_error()
        {
            // Assign
            Rule.Object.CustomsQty = 10;

            Record.Object.ContainersCount = 88;
            Record.Object.CustomsQty = Rule.Object.CustomsQty * 1.15m * Record.Object.ContainersCount;

            // Act
            await AssertSingleError(
                "Customs Qty should be between 792 (9 per container) and 968 (11 per container)", Record.Object);
        }

        [TestMethod]
        public async Task Customs_Qty_validation_within_10_percent_warning()
        {
            // Assign
            Rule.Object.CustomsQty = 10;

            Record.Object.ContainersCount = 88;
            Record.Object.CustomsQty = Rule.Object.CustomsQty * 1.09m * Record.Object.ContainersCount;

            // Act
            await AssertSingleError(
                "Customs Qty should be between 836 (9.5 per container) and 924 (10.5 per container)", Record.Object);
        }

        [DataTestMethod]
        [DataRow(100d, 105.1d, Severity.Warning)]
        [DataRow(100d, 109.9d, Severity.Warning)]
        [DataRow(100d, 110d, Severity.Error)]
        [DataRow(100d, 110.1d, Severity.Error)]
        [DataRow(100d, 1000000d, Severity.Error)]
        [DataRow(100d, 90.1d, Severity.Warning)]
        [DataRow(100d, 90d, Severity.Error)]
        [DataRow(100d, 89.9d, Severity.Error)]
        [DataRow(100d, -1000d, Severity.Error)]

        public async Task Customs_Qty_invalid_data_boundary_values(double ruleValue, double recordValue, Severity severity)
        {
            // Assign
            int callsCount = 0;

            _fieldsValidationResultBuilder.Setup(
                x => x.Add(It.IsAny<string>(), It
                    .IsAny<
                        string>(), It.IsAny<string>(), severity)).Callback(() => callsCount++);
            _fieldsValidationResultBuilder.Setup(
                x => x.AddIf(It.Is<bool>(b => b), It.IsAny<string>(), It
                    .IsAny<
                        string>(), It.IsAny<string>(), severity)).Callback(() => callsCount++);

            Rule.Object.CustomsQty = (decimal?) ruleValue;
            Record.Object.CustomsQty = (decimal?) recordValue;

            Record.Object.ContainersCount = 1;
            
            // Act
            var errors = await Validator.GetErrorsAsync(Record.Object);

            // Assert

            Assert.AreEqual(1, callsCount);
        }

        [DataTestMethod]
        [DataRow(100d, 105.1d, true)]
        [DataRow(100d, 105d, false)]
        [DataRow(100d, 95d, false)]
        [DataRow(100d, 94.9d, true)]
        [DataRow(100d, 100, false)]
        public async Task Customs_Qty_valid_data_boundary_values(double ruleValue, double recordValue, bool hasError)
        {
            int callsCount = 0;

            _fieldsValidationResultBuilder.Setup(
                x => x.Add(It.IsAny<string>(), It
                    .IsAny<
                        string>(), It.IsAny<string>(), It.IsAny<Severity>())).Callback(() => callsCount++);
            _fieldsValidationResultBuilder.Setup(
                x => x.AddIf(It.Is<bool>(b => b), It.IsAny<string>(), It
                    .IsAny<
                        string>(), It.IsAny<string>(), It.IsAny<Severity>())).Callback(() => callsCount++);

            // Assign
            Rule.Object.CustomsQty = (decimal?)ruleValue;
            Record.Object.CustomsQty = (decimal?)recordValue;

            Record.Object.ContainersCount = 1;

            // Act
            await Validator.GetErrorsAsync(Record.Object);

            // Assert
            Assert.AreEqual(hasError, callsCount > 0);
        }
    }
}