using Framework.Domain;
using System;

namespace FilingPortal.Parts.Recon.Domain.Entities
{
    /// <summary>
    /// Represents the Value Recon readonly model entity
    /// </summary>
    public class ValueReconReadModel : AuditableEntity
    {
        #region inbound
        public string Importer { get; set; }
        public string ImporterNo { get; set; }
        public string Filer { get; set; }
        public string EntryNo { get; set; }
        public string LineNumber7501 { get; set; }
        public string JobNumber { get; set; }
        public string ReconIssue { get; set; }
        public string NaftaRecon { get; set; }
        public DateTime? CalculatedClientReconDueDate { get; set; }
        public DateTime? ExportDate { get; set; }
        public DateTime? ImportDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string EntryPort { get; set; }
        public string DestinationState { get; set; }
        public string EntryType { get; set; }
        public string TransportMode { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string OwnerRef { get; set; }
        public string Spi { get; set; }
        public string CO { get; set; }
        public string ManufacturerMid { get; set; }
        public string Tariff { get; set; }
        public string GoodsDescription { get; set; }
        public string Container { get; set; }
        public string CustomsAttribute1 { get; set; }
        public decimal? CustomsQty1 { get; set; }
        public string CustomsUq1 { get; set; }
        public string MasterBill { get; set; }
        public decimal? LineEnteredValue { get; set; }
        public decimal? InvoiceLineCharges { get; set; }
        public decimal? Duty { get; set; }
        public decimal? Hmf { get; set; }
        public decimal? Mpf { get; set; }
        public decimal? PayableMpf { get; set; }
        public string PriorDisclosureMisc { get; set; }
        public string ProtestPetitionFiledStatMisc { get; set; }
        public string PsaReason { get; set; }
        public DateTime? PsaFiledDate { get; set; }
        public string PsaReason520d { get; set; }
        public DateTime? PsaFiledDate520d { get; set; }
        public string ProvProgTariff { get; set; }
        #endregion

        #region fta recon
        public string FtaEligibility { get; set; }
        public string ReconNfJobNumber { get; set; }
        public string ReconEntryLineSpi { get; set; }
        #endregion

        #region Client data
        /// <summary>
        /// Get or sets the Final Unit Price
        /// </summary>
        public decimal? FinalUnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the Final Total Value
        /// </summary>
        public decimal? FinalTotalValue { get; set; }

        /// <summary>
        /// Gets or sets the client note
        /// </summary>
        public string ClientNote { get; set; } 
        #endregion

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the Status Name
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets the Status Code
        /// </summary>
        public string StatusCode { get; set; }
    }
}
