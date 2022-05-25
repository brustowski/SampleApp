using System;

namespace FilingPortal.Parts.Recon.Domain.Entities
{
    /// <summary>
    /// Base inbound record for recon
    /// </summary>
    public interface IBaseInboundRecord
    {
        string JobNumber { get; set; }
        string Importer { get; set; }
        string ImporterNo { get; set; }
        string BondType { get; set; }
        string SuretyCode { get; set; }
        string EntryType { get; set; }
        string Filer { get; set; }
        string EntryNo { get; set; }
        int? LineNo { get; set; }
        string LineNumber7501 { get; set; }
        string ReconIssue { get; set; }
        string NaftaRecon { get; set; }
        string ReconJobNumbers { get; set; }
        string MainReconIssues { get; set; }
        DateTime? CalculatedValueReconDueDate { get; set; }
        DateTime? Calculated520DDueDate { get; set; }
        DateTime? CalculatedClientReconDueDate { get; set; }
        string FtaReconFiling { get; set; }
        string CO { get; set; }
        string Spi { get; set; }
        string ManufacturerMid { get; set; }
        string Tariff { get; set; }
        decimal? CustomsQty1 { get; set; }
        string CustomsUq1 { get; set; }
        decimal? LineEnteredValue { get; set; }
        decimal? Duty { get; set; }
        decimal? Mpf { get; set; }
        decimal? PayableMpf { get; set; }
        decimal? Hmf { get; set; }
        DateTime? ImportDate { get; set; }
        string Cancellation { get; set; }
        string PsaReason { get; set; }
        DateTime? PsaFiledDate { get; set; }
        string PsaReason520d { get; set; }
        DateTime? PsaFiledDate520d { get; set; }
        string PsaFiledBy { get; set; }
        string PscExplanation { get; set; }
        string PscReasonCodesHeader { get; set; }
        string PscReasonCodesLine { get; set; }
        DateTime? LiqDate { get; set; }
        string LiqType { get; set; }
        decimal? DutyLiquidated { get; set; }
        decimal? TotalLiquidatedFees { get; set; }
        string CbpForm28Action { get; set; }
        string CbpForm29Action { get; set; }
        string PriorDisclosureMisc { get; set; }
        string ProtestPetitionFiledStatMisc { get; set; }
        string TransportMode { get; set; }
        DateTime? PreliminaryStatementDate { get; set; }
        DateTime? ExportDate { get; set; }
        DateTime? ReleaseDate { get; set; }
        DateTime? DutyPaidDate { get; set; }
        DateTime? PaymentDueDate { get; set; }
        string EntryPort { get; set; }
        string DestinationState { get; set; }
        string Vessel { get; set; }
        string Voyage { get; set; }
        string OwnerRef { get; set; }
        string EnsStatusDescription { get; set; }
        string GoodsDescription { get; set; }
        string Container { get; set; }
        string CustomsAttribute1 { get; set; }
        string MasterBill { get; set; }
        decimal? InvoiceLineCharges { get; set; }
        string EnsStatus { get; set; }
        string Nafta303ClaimStatMisc { get; set; }
        string ReconJobNumbersVl { get; set; }
        string ReconJobNumbersNf { get; set; }
        string ProvProgTariff { get; set; }
        string InvoiceNumber { get; set; }
        string ValueReconFlaggedNotFiled { get; set; }
        string FtaReconFlaggedNotFiled { get; set; }
    }
}
