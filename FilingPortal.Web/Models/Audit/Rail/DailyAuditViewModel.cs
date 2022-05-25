using System.Collections.Generic;
using FilingPortal.Domain.DTOs;
using FilingPortal.PluginEngine.Models;

namespace FilingPortal.Web.Models.Audit.Rail
{
    /// <summary>
    /// Rail Audit Daily Audit view model
    /// </summary>
    public class DailyAuditViewModel : ViewModelWithActions, IModelWithValidation<FieldsValidationResult>
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
        /// Gets or sets Importer code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets importer number
        /// </summary>
        public string ImporterNo { get; set; }

        /// <summary>
        /// Gets or sets Export Date
        /// </summary>
        public string ExportDate { get; set; }
        /// <summary>
        /// Gets or sets Import Date
        /// </summary>
        public string ImportDate { get; set; }
        /// <summary>
        /// Gets or sets PSD
        /// </summary>
        public string Psd { get; set; }
        /// <summary>
        /// Gets or sets Payment Due Date
        /// </summary>
        public string PaymentDueDate { get; set; }
        /// <summary>
        /// Gets or sets Release Date
        /// </summary>
        public string ReleaseDate { get; set; }
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
        public string LineNumber { get; set; }
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
        public string ContainersCount { get; set; }
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
        public string CustomsQty { get; set; }
        /// <summary>
        /// Gets or sets Customs Quantity Unit
        /// </summary>
        public string CustomsQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity
        /// </summary>
        public string InvoiceQty { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity Unit
        /// </summary>
        public string InvoiceQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets Line Price
        /// </summary>
        public string LinePrice { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight
        /// </summary>
        public string GrossWeight { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight Unit
        /// </summary>
        public string GrossWeightUq { get; set; }
        /// <summary>
        /// Gets or sets Duty
        /// </summary>
        public string Duty { get; set; }
        /// <summary>
        /// Gets or sets HMF
        /// </summary>
        public string Hmf { get; set; }
        /// <summary>
        /// Gets or sets MPF
        /// </summary>
        public string Mpf { get; set; }
        /// <summary>
        /// Gets of sets Payable MPF
        /// </summary>
        public string PayableMpf { get; set; }
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
        public string PayType { get; set; }
        /// <summary>
        /// Gets or sets unit price
        /// </summary>
        public string UnitPrice { get; set; }
        /// <summary>
        /// Gets or sets whether record is Unit Train
        /// </summary>
        public bool IsUnitTrain { get; set; }
        /// <summary>
        /// Gets or sets Supplier Code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Gets or sets Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }
        /// <summary>
        /// Gets or sets CHGS (Freight)
        /// </summary>
        public string Chgs { get; set; }
        /// <summary>
        /// Gets or sets Last Modified By
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// Gets or sets last modified date
        /// </summary>
        public string LastModifiedDate { get; set; }
        /// <summary>
        /// Gets or sets the list of Errors
        /// </summary>
        public List<FieldsValidationResult> Errors { get; set; } = new List<FieldsValidationResult>();
    }
}