using System;
using Framework.Domain;

namespace FilingPortal.Domain.Entities.Audit.Rail
{
    /// <summary>
    /// Represents model for rail daily audit
    /// </summary>
    public class AuditRailDaily : AuditableEntity
    {
        /// <summary>
        /// Gets or sets Job Header Status
        /// </summary>
        public string JobHeaderStatus { get; set; }
        /// <summary>
        /// Gets or sets Job Number
        /// </summary>
        public string JobNumber { get; set; }
        /// <summary>
        /// Gets or sets Filer
        /// </summary>
        public string Filer { get; set; }
        /// <summary>
        /// Gets or sets Entry Number
        /// </summary>
        public string EntryNumber { get; set; }
        /// <summary>
        /// Gets or sets importer name
        /// </summary>
        public string Importer { get; set; }
        /// <summary>
        /// Gets or sets importer number
        /// </summary>
        public string ImporterNo { get; set; }
        /// <summary>
        /// Gets or sets Export Date
        /// </summary>
        public DateTime? ExportDate { get; set; }
        /// <summary>
        /// Gets or sets Import Date
        /// </summary>
        public DateTime? ImportDate { get; set; }
        /// <summary>
        /// Gets or sets PSD
        /// </summary>
        public DateTime? Psd { get; set; }
        /// <summary>
        /// Gets or sets Payment Due Date
        /// </summary>
        public DateTime? PaymentDueDate { get; set; }
        /// <summary>
        /// Gets or sets Release Date
        /// </summary>
        public DateTime? ReleaseDate { get; set; }
        /// <summary>
        /// Gets or sets release status
        /// </summary>
        public string ReleaseStatus { get; set; }
        /// <summary>
        /// Gets or sets ENS status description
        /// </summary>
        public string EnsStatusDescription { get; set; }
        /// <summary>
        /// Gets or sets Entry Type
        /// </summary>
        public string EntryType { get; set; }
        /// <summary>
        /// Gets or sets Entry Port
        /// </summary>
        public string EntryPort { get; set; }
        /// <summary>
        /// Gets or sets Arrival Port
        /// </summary>
        public string ArrivalPort { get; set; }
        /// <summary>
        /// Gets or sets destination state
        /// </summary>
        public string DestinationState { get; set; }
        /// <summary>
        /// Gets or sets Line Number
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// Gets or sets Country of Export
        /// </summary>
        public string CountryOfExport { get; set; }
        /// <summary>
        /// Gets or sets Country of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets Master Bills
        /// </summary>
        public string MasterBills { get; set; }
        /// <summary>
        /// Gets or sets Mode of transport
        /// </summary>
        public string ModeOfTransport { get; set; }
        /// <summary>
        /// Gets or sets Containers Count
        /// </summary>
        public int? ContainersCount { get; set; }
        /// <summary>
        /// Gets or sets Containers description
        /// </summary>
        public string Containers { get; set; }
        /// <summary>
        /// Gets or sets owner reference
        /// </summary>
        public string OwnerReference { get; set; }
        /// <summary>
        /// Gets or sets Supplier MID
        /// </summary>
        public string SupplierMid { get; set; }
        /// <summary>
        /// Gets or sets manufacturer mid
        /// </summary>
        public string ManufacturerMid { get; set; }
        /// <summary>
        /// Gets or sets Ultimate Consignee Name
        /// </summary>
        public string UltimateConsigneeName { get; set; }
        /// <summary>
        /// Gets or sets Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets FIRMs Code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets Tariff
        /// </summary>
        public string Tariff { get; set; }
        /// <summary>
        /// Gets or sets SPI
        /// </summary>
        public string Spi { get; set; }
        /// <summary>
        /// Gets or sets Customs Quantity
        /// </summary>
        public decimal? CustomsQty { get; set; }
        /// <summary>
        /// Gets or sets Customs Quantity Unit
        /// </summary>
        public string CustomsQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity
        /// </summary>
        public decimal? InvoiceQty { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity Unit
        /// </summary>
        public string InvoiceQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets Line Price
        /// </summary>
        public decimal? LinePrice { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight
        /// </summary>
        public decimal? GrossWeight { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight Unit
        /// </summary>
        public string GrossWeightUq { get; set; }
        /// <summary>
        /// Gets or sets Duty
        /// </summary>
        public decimal? Duty { get; set; }
        /// <summary>
        /// Gets or sets HMF
        /// </summary>
        public decimal? Hmf { get; set; }
        /// <summary>
        /// Gets or sets MPF
        /// </summary>
        public decimal? Mpf { get; set; }
        /// <summary>
        /// Gets of sets Payable MPF
        /// </summary>
        public decimal? PayableMpf { get; set; }
        /// <summary>
        /// Gets or sets Value Recon
        /// </summary>
        public string ValueRecon { get; set; }
        /// <summary>
        /// Gets or sets NAFTA Recon
        /// </summary>
        public string NaftaRecon { get; set; }
        /// <summary>
        /// Gets or sets goods description
        /// </summary>
        public string GoodsDescription { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 1
        /// </summary>
        public string CustomsAttrib1 { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 2
        /// </summary>
        public string CustomsAttrib2 { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 3
        /// </summary>
        public string CustomsAttrib3 { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 4
        /// </summary>
        public string CustomsAttrib4 { get; set; }
        /// <summary>
        /// Gets or sets transactions related
        /// </summary>
        public string TransactionsRelated { get; set; }
        /// <summary>
        /// Gets or sets Pay Type
        /// </summary>
        public int? PayType { get; set; }
        /// <summary>
        /// Gets or sets Last Modified By
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets last modified date
        /// </summary>
        public DateTime LastModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets Summary Date
        /// </summary>
        public DateTime? SummaryDate { get; set; }
        /// <summary>
        /// Gets or sets Port Code
        /// </summary>
        public string PortCode { get; set; }
        /// <summary>
        /// Gets or sets Entry Date
        /// </summary>
        public DateTime? EntryDate { get; set; }
        /// <summary>
        /// Gets or sets Location of Goods
        /// </summary>
        public string LocationOfGoods { get; set; }
        /// <summary>
        /// Gets or sets Consignee No
        /// </summary>
        public string ConsigneeNo { get; set; }
        /// <summary>
        /// Gets or sets Importer code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets Supplier Code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Gets or sets Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }
        /// <summary>
        /// Gets or sets Net Quantity In HTSUS Units
        /// </summary>
        public string NetQuantityInHtsusUnits { get; set; }
        /// <summary>
        /// Gets or sets CHGS
        /// </summary>
        public decimal? Chgs { get; set; }
        /// <summary>
        /// Gets or sets Tariff Rate
        /// </summary>
        public decimal? TariffRate { get; set; }
        /// <summary>
        /// Gets or sets Duty and IR tax
        /// </summary>
        public decimal? DutyAndIrTax { get; set; }
        /// <summary>
        /// Gets or sets Other Fee Summary Block 39
        /// </summary>
        public decimal? OtherFeeSummaryBlock39 { get; set; }
        /// <summary>
        /// Gets or sets total entered value
        /// </summary>
        public decimal? TotalEnteredValue { get; set; }

        /// <summary>
        /// Gets or sets Other
        /// </summary>
        public decimal? Other { get; set; }
        /// <summary>
        /// Gets or sets Total
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// Gets or sets destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// Gets or sets unit price
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// Returns if record is Unit Train
        /// </summary>
        public bool IsUnitTrain => ContainersCount != null && ContainersCount > 1;

        /// <summary>
        /// Gets or sets validation result
        /// </summary>
        public string ValidationResult { get; set; }
    }
}
