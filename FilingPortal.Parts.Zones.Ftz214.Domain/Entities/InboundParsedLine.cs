using Framework.Domain;

namespace FilingPortal.Parts.Zones.Ftz214.Domain.Entities
{
    /// <summary>
    /// Represents inbound line
    /// </summary>
    public class InboundParsedLine : Entity
    {
        /// <summary>
        /// Parent record id
        /// </summary>
        public int InboundRecordId { get; set; }
        /// <summary>
        /// Parent inbound record
        /// </summary>
        public virtual InboundRecord InboundRecord { get; set; }

        public int? ItemNo { get; set; }
        public string SubItem { get; set; }
        public string Hts { get; set; }
        public string AdditionalHts { get; set; }
        public string Spi1 { get; set; }
        public string Spi1Country { get; set; }
        public string Spi2 { get; set; }
        public decimal? Qty1 { get; set; }
        public string Qty1uom { get; set; }
        public decimal? Qty2 { get; set; }
        public string Qty2uom { get; set; }
        public string CategoryNo { get; set; }
        public string Co { get; set; }
        public string Mid { get; set; }
        public int? Value { get; set; }
        public int? AdditionalHtsValue { get; set; }
        public int? GrossWgt { get; set; }
        public int? GrossLbs { get; set; }
        public int? Charges { get; set; }
        public string Hmf { get; set; }
        public string ZoneStatus { get; set; }
        public string MxCementLicenseNo { get; set; }
        public string PnDisclaimer { get; set; }
        public string ImporterAcctno { get; set; }
        public string ImporterAcct { get; set; }
        public string ImporterIrsno { get; set; }
        public string ProductName { get; set; }
        public string ProductCountry { get; set; }
        public decimal? ProductQty { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
    }
}