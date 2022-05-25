using System;
using Framework.Domain;

namespace FilingPortal.Parts.Recon.Domain.Entities
{
    /// <summary>
    /// Entity for ACE report record. Fields represents columns in ACE Report
    /// </summary>
    public class AceReportRecord : AuditableEntity
    {
        public string ImporterName { get; set; }
        public string ImporterNumber { get; set; }
        public string BondType { get; set; }
        public string BondNumber { get; set; }
        public string SuretyCode { get; set; }
        public string EntryTypeCode { get; set; }
        public string EntrySummaryNumber { get; set; }
        public int EntrySummaryLineNumber { get; set; }
        public DateTime? ImportationDate { get; set; }
        public DateTime EntrySummaryDate { get; set; }
        public string ReconciliationIndicator { get; set; }
        public string ReconciliationIssueCode { get; set; }
        public DateTime? ReconciliationOtherDueDate { get; set; }
        public string ReconciliationOtherStatus { get; set; }
        public string ReconciliationOtherEntrySummaryNumber { get; set; }
        public string NaftaReconciliationIndicator { get; set; }
        public DateTime? ReconciliationFtaDueDate { get; set; }
        public string ReconciliationFtaStatus { get; set; }
        public string ReconciliationFtaEntrySummaryNumber { get; set; }
        public string ReviewTeamNumber { get; set; }
        public string CountryOfOriginCode { get; set; }
        public string LineSpiCode { get; set; }
        public string ManufacturerId { get; set; }
        public string HtsNumberFull { get; set; }
        public decimal LineTariffQuantity { get; set; }
        public decimal LineGoodsValueAmount { get; set; }
        public decimal LineDutyAmount { get; set; }
        public decimal TotalPaidMpfAmount { get; set; }
        public decimal LineMpfAmount { get; set; }
        public decimal LineHmfAmount { get; set; }
        public DateTime? LatestPscFiledDate { get; set; }
    }
}
