using System;
using Framework.Domain;

namespace FilingPortal.Parts.Zones.Entry.Domain.Entities
{
    /// <summary>
    /// Represents inbound line
    /// </summary>
    public class InboundLine : Entity
    {
        /// <summary>
        /// Gets or sets Item No
        /// </summary>
        public int ItemNo { get; set; }

        /// <summary>
        /// Gets or sets Item Value
        /// </summary>
        public decimal? ItemValue { get; set; }

        /// <summary>
        /// Gets or sets HTS
        /// </summary>
        public string Hts { get; set; }

        /// <summary>
        /// Gets or sets Country of origin
        /// </summary>
        public string CountryOfOrigin { get; set; }

        /// <summary>
        /// Gets or sets Manufacturers Id NO
        /// </summary>
        public string ManufacturersIdNo { get; set; }

        /// <summary>
        /// Gets or sets FTZ Manifest Qty
        /// </summary>
        public decimal? FtzManifestQty { get; set; }

        /// <summary>
        /// Gets or sets FTZ Status
        /// </summary>
        public string FtzStatus { get; set; }

        /// <summary>
        /// Gets or sets FTZ Date
        /// </summary>
        public DateTime? FtzDate { get; set; }

        /// <summary>
        /// Gets or sets Product Name
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// Parent inbound record
        /// </summary>
        public virtual InboundRecord InboundRecord { get; set; }
        /// <summary>
        /// Parent record id
        /// </summary>
        public int InboundRecordId { get; set; }
        /// <summary>
        /// Transaction related flag
        /// </summary>
        public string TransactionRelated { get; set; }
        /// <summary>
        /// Gets or sets charges
        /// </summary>
        public decimal? Charges { get; set; }
        /// <summary>
        /// Gross weight
        /// </summary>
        public decimal? GrossWeight { get; set; }
        /// <summary>
        /// Gross weight unit
        /// </summary>
        public string GrossWeightUnit { get; set; }

        /// <summary>
        /// Gets or sets SPI
        /// </summary>
        public string Spi { get; set; }

        /// <summary>
        /// Gets or sets ProgramCode
        /// </summary>
        public string ProgramCode { get; set; }

        /// <summary>
        /// Gets or sets DeclarationCode
        /// </summary>
        public string DeclarationCode { get; set; }

        /// <summary>
        /// Gets or sets DeclarationCode
        /// </summary>
        public string Disclaimer { get; set; }

        /// <summary>
        /// Gets or sets Qty_UOM
        /// </summary>
        public string Qty_1_UOM { get; set; }


    }
}