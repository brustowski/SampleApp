using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FilingPortal.Domain.Common;
using FilingPortal.Domain.DTOs;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Repositories.Audit.Rail;
using FilingPortal.Domain.Services.AppSystem;
using Framework.Infrastructure;
using Newtonsoft.Json;

namespace FilingPortal.Domain.Validators.Audit.Rail
{
    /// <summary>
    /// Provides methods for single <see cref="AuditRailDaily"/> record validation
    /// </summary>
    public class RailDailyAuditInboundValidator : ISingleRecordTypedValidator<FieldsValidationResult, AuditRailDaily>
    {
        private readonly IRailDailyAuditRulesRepository _rulesRepository;
        private readonly IRailDailyAuditSpiRulesRepository _spiRulesRepository;
        private readonly IFieldsValidationResultBuilder _builder;
        private readonly ISettingsService _settingsService;

        /// <summary>
        /// Creates a new instance of <see cref="RailDailyAuditInboundValidator"/>
        /// </summary>
        /// <param name="rulesRepository">Daily audit rules repository</param>
        /// <param name="settingsService">Application settings service</param>
        /// <param name="spiRulesRepository">Daily audit SPI rules repository</param>
        /// <param name="fieldsValidationResultsBuilder">Fields validation results builder</param>
        public RailDailyAuditInboundValidator(IRailDailyAuditRulesRepository rulesRepository, ISettingsService settingsService, IRailDailyAuditSpiRulesRepository spiRulesRepository, IFieldsValidationResultBuilder fieldsValidationResultsBuilder)
        {
            _rulesRepository = rulesRepository;
            _settingsService = settingsService;
            _spiRulesRepository = spiRulesRepository;
            _builder = fieldsValidationResultsBuilder;
        }

        /// <summary>
        /// Adds the list of the customs errors for the specified record
        /// </summary>
        /// <param name="errors">List of errors to witch to add customs errors</param>
        /// <param name="record">The record to check</param>
        protected async Task<List<FieldsValidationResult>> AddCustomErrors(List<FieldsValidationResult> errors, AuditRailDaily record)
        {
            IList<AuditRailDailyRule> rules = await _rulesRepository.FindCorrespondingRules(record);
            IList<AuditRailDailySpiRule> spiRules = await _spiRulesRepository.FindCorrespondingRules(record);

            try
            {
                errors.AddRange(Validate(record, rules, spiRules));
            }
            catch (Exception e)
            {
                errors.Add(new FieldsValidationResult("Unknown validation error occured"));
                AppLogger.Error(e, "Unknown validation error occured");
            }

            return errors;
        }

        private IEnumerable<FieldsValidationResult> Validate(AuditRailDaily record, IList<AuditRailDailyRule> rules,
            IList<AuditRailDailySpiRule> spiRules)
        {
            _builder.Reset();

            // Database validation
            if (!string.IsNullOrWhiteSpace(record.ValidationResult))
            {
                try
                {
                    FieldsValidationResult[] validationResult =
                        JsonConvert.DeserializeObject<FieldsValidationResult[]>(record.ValidationResult);
                    _builder.AddRange(validationResult);
                }
                catch (Exception e)
                {
                    _builder.Add($"Can't parse database validation: {e.Message}");
                }
            }
            // Rules validation

            _builder.AddIf(rules == null || !rules.Any(), "No corresponding rule found");
            if (rules != null)
            {
                _builder.AddIf(rules.Count > 1, "Several validation rules found");

                if (rules.Count == 1)
                {
                    AuditRailDailyRule rule = rules.First();

                    _builder.AddIf(NotMatches(rule.PortCode, record.EntryPort),
                        $"Entry Port should be {rule.PortCode}");
                    _builder.AddIf(NotMatches(rule.PortCode, record.ArrivalPort),
                        $"Arrival Port should be {rule.PortCode}");
                    _builder.AddIf(NotMatches(rule.DestinationState, record.DestinationState), $"Destination State should be {rule.DestinationState}");
                    _builder.AddIf(NotMatches(rule.ExportingCountry, record.CountryOfExport), $"Country of Export should be {rule.ExportingCountry}");
                    _builder.AddIf(NotMatches(rule.CountryOfOrigin, record.CountryOfOrigin), $"Country of Origin should be {rule.CountryOfOrigin}");
                    _builder.AddIf(NotMatches(rule.UltimateConsigneeName, record.UltimateConsigneeName), $"Ultimate Consignee Name should be {rule.UltimateConsigneeName}");
                    _builder.AddIf(NotMatches(rule.FirmsCode, record.FirmsCode), $"FIRMs Code should be {rule.FirmsCode}");
                    _builder.AddIf(NotMatches(rule.Tariff, record.Tariff), $"Tariff should be {rule.Tariff}");
                    _builder.AddIf(NotMatches(rule.Carrier, record.Carrier), $"Carrier should be {rule.Carrier}");
                    _builder.AddIf(NotMatches(rule.NaftaRecon, NullSubstitution(record.NaftaRecon, "N")), $"NAFTA Recon should be {rule.NaftaRecon}");
                    _builder.AddIf(NotMatches(rule.ValueRecon, record.ValueRecon), $"Value Recon should be {rule.ValueRecon}");
                    _builder.AddIf(NotMatchesContains(rule.GoodsDescription, record.GoodsDescription), $"Goods description doesn't contain {rule.GoodsDescription}");
                    _builder.AddIf(NotMatches(rule.ManufacturerMid, record.ManufacturerMid), $"Manufacturer MID should be {rule.ManufacturerMid}");
                    _builder.AddIf(NotMatches(rule.SupplierMid, record.SupplierMid), $"Supplier MID should be {rule.SupplierMid}");
                    _builder.AddIf(NotMatches(rule.InvoiceQtyUnit, record.InvoiceQtyUnit), $"Invoice Qty Unit should be {rule.InvoiceQtyUnit}");
                    _builder.AddIf(NotMatches(rule.CustomsQtyUnit, record.CustomsQtyUnit), $"Customs Qty Unit should be {rule.CustomsQtyUnit}");
                    _builder.AddIf(NotMatches(rule.CustomsAttrib1, record.CustomsAttrib1), $"Customs Attribute 1 should be {rule.CustomsAttrib1}");
                    _builder.AddIf(NotMatches(rule.CustomsAttrib4, record.CustomsAttrib4), $"Customs Attribute 4 should be {rule.CustomsAttrib4}");
                    _builder.AddIf(NotMatches(rule.TransactionsRelated, record.TransactionsRelated), $"Transaction Related should be {rule.TransactionsRelated}");
                    _builder.AddIf(NotMatches(rule.UnitPrice, record.UnitPrice), $"Unit Price should be {rule.UnitPrice}");
                    _builder.AddIf(NotMatches(rule.GrossWeightUq, record.GrossWeightUq), $"Gross Weight Uq should be {rule.GrossWeightUq}", overrideId: "GrossWeightValidation");


                    if (record.Tariff != null && record.Tariff.Trim().StartsWith("27"))
                    {
                        decimal? apiValue = null;
                        try
                        {
                            apiValue = GetApi(record.CustomsAttrib2);
                        }
                        catch (FormatException)
                        {
                            _builder.Add("Customs Attribute 2 has wrong format");
                        }
                        catch (NullReferenceException)
                        {
                            _builder.Add("Customs Attribute 2 is empty");
                        }

                        if (apiValue != null)
                        {
                            if (rule.ApiFrom != null && rule.ApiTo != null)
                                _builder.AddIf(
                                    apiValue < rule.ApiFrom ||
                                    apiValue > rule.ApiTo,
                                    $"Customs Attribute 2 should be between {rule.ApiFrom:G6} and {rule.ApiTo:G6}");
                            else if (rule.ApiFrom != null)
                                _builder.AddIf(apiValue < rule.ApiFrom,
                                        $"Customs Attribute 2 should not be less then {rule.ApiFrom:G6}");
                            else
                                _builder.AddIf(apiValue > rule.ApiTo,
                                        $"Customs Attribute 2 should not be greater then {rule.ApiTo:G6}");
                        }
                    }

                    if (record.ContainersCount.HasValue && record.CustomsQty.HasValue && rule.CustomsQty.HasValue)
                    {
                        decimal warningThreshold = _settingsService.Get<decimal>(SettingsNames.RailDailyAuditCustomsQtyWarningThreshold);
                        decimal errorThreshold = _settingsService.Get<decimal>(SettingsNames.RailDailyAuditCustomsQtyErrorThreshold);
                        decimal? ruleQty = rule.CustomsQty * record.ContainersCount;

                        decimal difference = Math.Abs(1 - record.CustomsQty.Value / ruleQty.Value);
                        _builder.AddIf(difference > warningThreshold && difference < errorThreshold,
                            $"Customs Qty should be between {ruleQty.Value * (1 - warningThreshold):G6} ({ruleQty.Value * (1 - warningThreshold) / record.ContainersCount:G6} per container) and {ruleQty.Value * (1 + warningThreshold):G6} ({ruleQty.Value * (1 + warningThreshold) / record.ContainersCount:G6} per container)", severity: Severity.Warning);
                        _builder.AddIf(difference >= errorThreshold,
                                $"Customs Qty should be between {ruleQty.Value * (1 - errorThreshold):G6} ({ruleQty.Value * (1 - errorThreshold) / record.ContainersCount:G6} per container) and {ruleQty.Value * (1 + errorThreshold):G6} ({ruleQty.Value * (1 + errorThreshold) / record.ContainersCount:G6} per container)");
                    }

                    if (record.ContainersCount.HasValue && record.Chgs.HasValue && rule.Freight.HasValue && rule.FreightType.HasValue && record.InvoiceQty.HasValue)
                    {
                        decimal? ruleFreight = 0;
                        switch (rule.FreightType)
                        {
                            case FreightType.PerContainer:
                                ruleFreight = rule.Freight * record.ContainersCount;
                                break;
                            case FreightType.PerUom:
                                ruleFreight = Math.Round(rule.Freight.Value * record.InvoiceQty.Value, 2);
                                break;
                        }

                        _builder.AddIf(NotMatches(ruleFreight, Math.Round(record.Chgs.Value, 2)),  $"Freight should be {ruleFreight:F2}");
                    }
                }
                // SPI rules validation
                if (spiRules != null)
                {
                    if (spiRules.Count == 0)
                    {
                        _builder.AddIf(NotMatches("N/A", record.Spi), "SPI should be N/A");
                    }
                    else
                    {
                        if (spiRules.Count > 1)
                            _builder.Add("Several SPI validation rules found");
                        else
                        {
                            // Here we have only one SPI rule
                            AuditRailDailySpiRule spiRule = spiRules.First();
                            _builder.AddIf(NotMatches(spiRule.Spi, record.Spi), $"SPI should be {spiRule.Spi}");
                        }
                    }
                }
            }

            return _builder.Build();
        }

        /// <summary>
        /// Returns substitution value if provided value is null
        /// </summary>
        /// <param name="recordValue">Possible nullable value</param>
        /// <param name="substitution">String substitution</param>
        private string NullSubstitution(string recordValue, string substitution)
        {
            return string.IsNullOrWhiteSpace(recordValue) ? substitution : recordValue;
        }

        private decimal GetApi(string customsAttribute2)
        {
            string decimalPart = customsAttribute2.Split('=').Last();
            return Convert.ToDecimal(decimalPart, CultureInfo.GetCultureInfo("en-US"));
        }

        /// <summary>
        /// Compares rule value with records value
        /// </summary>
        /// <param name="ruleValue">Rule value</param>
        /// <param name="recordValue">Record value</param>
        private bool NotMatches(string ruleValue, string recordValue)
        {
            return !string.IsNullOrWhiteSpace(ruleValue) && ruleValue.Trim() != recordValue?.Trim();
        }

        /// <summary>
        /// Compares rule value with records value
        /// </summary>
        /// <param name="ruleValue">Rule value</param>
        /// <param name="recordValue">Record value</param>
        private bool NotMatches(decimal? ruleValue, decimal? recordValue)
        {
            return ruleValue != null && ruleValue != recordValue;
        }

        /// <summary>
        /// Compares rule value with records value by contains operator
        /// </summary>
        /// <param name="ruleValue">Rule value</param>
        /// <param name="recordValue">Record value</param>
        private bool NotMatchesContains(string ruleValue, string recordValue)
        {
            if (string.IsNullOrWhiteSpace(ruleValue)) return false;
            if (string.IsNullOrWhiteSpace(recordValue)) return true;
            return !recordValue.Trim().Contains(ruleValue.Trim());
        }

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public List<FieldsValidationResult> GetErrors(AuditRailDaily record) => GetErrorsAsync(record).Result;

        /// <summary>
        /// Gets the list of errors for the specified Single Record
        /// </summary>
        /// <param name="record">The record</param>
        public async Task<List<FieldsValidationResult>> GetErrorsAsync(AuditRailDaily record)
        {
            var errors = new List<FieldsValidationResult>();
            return await AddCustomErrors(errors, record);
        }
    }
}
