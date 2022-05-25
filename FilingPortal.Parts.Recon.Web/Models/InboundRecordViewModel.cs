﻿using System;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using FilingPortal.PluginEngine.Models;
using Newtonsoft.Json;

namespace FilingPortal.Parts.Recon.Web.Models
{
    /// <summary>
    /// Defines the inbound record View Model from Cargo Wise
    /// </summary>
    public class InboundRecordViewModel : ViewModelWithActions
    {
        public string JobNumber { get; set; }
        public string Importer { get; set; }
        public string ImporterNo { get; set; }
        public string BondType { get; set; }
        public string SuretyCode { get; set; }
        public string EntryType { get; set; }
        public string Filer { get; set; }
        public string EntryNo { get; set; }
        public int? LineNo { get; set; }
        public string LineNumber7501 { get; set; }
        public string ReconIssue { get; set; }
        public string NaftaRecon { get; set; }
        public string ReconJobNumbers { get; set; }
        public string MainReconIssues { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? CalculatedValueReconDueDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? Calculated520DDueDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? CalculatedClientReconDueDate { get; set; }
        public string FtaReconFiling { get; set; }
        public string CO { get; set; }
        public string Spi { get; set; }
        public string ManufacturerMid { get; set; }
        public string Tariff { get; set; }
        public decimal? CustomsQty1 { get; set; }
        public string CustomsUq1 { get; set; }
        public decimal? LineEnteredValue { get; set; }
        public decimal? Duty { get; set; }
        public decimal? Mpf { get; set; }
        public decimal? PayableMpf { get; set; }
        public decimal? Hmf { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? ImportDate { get; set; }
        public string Cancellation { get; set; }
        public string PsaReason { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? PsaFiledDate { get; set; }
        public string PsaReason520d { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? PsaFiledDate520d { get; set; }
        public string PsaFiledBy { get; set; }
        public string PscExplanation { get; set; }
        public string PscReasonCodesHeader { get; set; }
        public string PscReasonCodesLine { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? LiqDate { get; set; }
        public string LiqType { get; set; }
        public decimal? DutyLiquidated { get; set; }
        public decimal? TotalLiquidatedFees { get; set; }
        public string CbpForm28Action { get; set; }
        public string CbpForm29Action { get; set; }
        public string PriorDisclosureMisc { get; set; }
        public string ProtestPetitionFiledStatMisc { get; set; }
        public string TransportMode { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? PreliminaryStatementDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? ExportDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? ReleaseDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? DutyPaidDate { get; set; }
        [JsonConverter(typeof(DateFormatConverter), FormatsContainer.US_DATETIME_FORMAT)]
        public DateTime? PaymentDueDate { get; set; }
        public string EntryPort { get; set; }
        public string DestinationState { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string OwnerRef { get; set; }
        public string EnsStatusDescription { get; set; }
        public string GoodsDescription { get; set; }
        public string Container { get; set; }
        public string CustomsAttribute1 { get; set; }
        public string MasterBill { get; set; }
        public decimal? InvoiceLineCharges { get; set; }
        public string EnsStatus { get; set; }
        public string Nafta303ClaimStatMisc { get; set; }
        public string ReconJobNumbersVl { get; set; }
        public string ReconJobNumbersNf { get; set; }
        public string ProvProgTariff { get; set; }
        public string InvoiceNumber { get; set; }
    }
}