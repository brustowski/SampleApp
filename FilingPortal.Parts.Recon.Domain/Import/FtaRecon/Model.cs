using System;
using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Parts.Recon.Domain.Import.FtaRecon
{
    /// <summary>
    /// Represents Client FTA parsing data model
    /// </summary>
    public class Model : ParsingDataModel
    {
        public string Importer { get; set; }
        public string ImporterNo { get; set; }
        public string Filer { get; set; }
        public string EntryNo { get; set; }
        public string LineNumber7501 { get; set; }
        public string JobNumber { get; set; }
        public string NaftaRecon { get; set; }
        public DateTime? Calculated520DDueDate { get; set; }
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
        public decimal? Duty { get; set; }
        public decimal? Mpf { get; set; }
        public decimal? PayableMpf { get; set; }
        public string PriorDisclosureMisc { get; set; }
        public string ProtestPetitionFiledStatMisc { get; set; }
        public string Nafta303ClaimStatMisc { get; set; }
        public string FtaEligibility { get; set; }
        public string ClientNote { get; set; }
    }
}
