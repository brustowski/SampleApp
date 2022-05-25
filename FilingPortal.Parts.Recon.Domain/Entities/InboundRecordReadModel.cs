using System;

namespace FilingPortal.Parts.Recon.Domain.Entities
{
    /// <summary>
    /// Inbound record from Cargo Wise combined with ACE report data
    /// </summary>
    public class InboundRecordReadModel : BaseInboundRecord
    {
        public DateTime? FtaDeadline { get; set; }
        public DateTime? OtherDeadline { get; set; }

        public int? AceId { get; set; }
        public string AceEntrySummaryNumber { get; set; }
        public bool AceFound { get; set; }
        public string AceReconIndicator { get; set; }
        public bool MismatchReconValueFlag { get; set; }
        public string AceNaftaReconIndicator { get; set; }
        public bool MismatchReconFtaFlag { get; set; }
        public decimal? AceLineGoodsValueAmount { get; set; }
        public bool MismatchEntryValue { get; set; }
        public decimal? AceLineDutyAmount { get; set; }
        public bool MismatchDuty { get; set; }
        public decimal? AceLineMpfAmount { get; set; }
        public bool MismatchMpf { get; set; }
        public decimal? AceTotalPaidMpfAmount { get; set; }
        public bool MismatchPayableMpf { get; set; }
        public decimal? AceLineTariffQuantity { get; set; }
        public bool MismatchQuantity { get; set; }
        public string AceHtsNumberFull { get; set; }
        public bool MismatchHts { get; set; }
    }
}
