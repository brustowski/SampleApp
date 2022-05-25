using System;

namespace FilingPortal.Parts.Recon.Domain.Reporting.CargoWiseClientValue
{
    /// <summary>
    /// Represents the Inbound Client Value Report Model
    /// </summary>
    public class Model
    {
        /// <summary>
        /// Gets or sets the Identifier
        /// </summary>
        public int Id { get; set; }
        public string Importer { get; set; }
        public string ImporterNo { get; set; }
        public string Filer { get; set; }
        public string EntryNo { get; set; }
        public string LineNumber7501 { get; set; }
        public string JobNumber { get; set; }
        public string ReconIssue { get; set; }
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
        public decimal? FinalUnitPrice { get; set; }
        public decimal? FinalTotalValue { get; set; }
        public string ClientNote { get; set; }
    }
}