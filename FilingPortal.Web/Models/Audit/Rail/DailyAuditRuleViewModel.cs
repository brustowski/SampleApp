using System;
using FilingPortal.Domain.Mapping;
using FilingPortal.PluginEngine.Common.Json;
using FilingPortal.PluginEngine.Models;
using FilingPortal.Web.Models.Rail;
using Newtonsoft.Json;

namespace FilingPortal.Web.Models.Audit.Rail
{
    /// <summary>
    /// Rail Audit Daily Audit Rules view model
    /// </summary>
    public class DailyAuditRuleViewModel : RuleViewModelWithActions
    {
        /// <summary>
        /// Gets or sets Importer code
        /// </summary>
        public string ImporterCode { get; set; }
        /// <summary>
        /// Gets or sets Carrier
        /// </summary>
        public string Carrier { get; set; }
        /// <summary>
        /// Gets or sets Port Code
        /// </summary>
        public string PortCode { get; set; }
        /// <summary>
        /// Gets or sets destination state
        /// </summary>
        public string DestinationState { get; set; }
        /// <summary>
        /// Gets or sets Exporting Country
        /// </summary>
        public string ExportingCountry { get; set; }
        /// <summary>
        /// Gets or sets Country of Origin
        /// </summary>
        public string CountryOfOrigin { get; set; }
        /// <summary>
        /// Gets or sets Ultimate Consignee Name
        /// </summary>
        public string UltimateConsigneeName { get; set; }
        /// <summary>
        /// Gets or sets FIRMs Code
        /// </summary>
        public string FirmsCode { get; set; }
        /// <summary>
        /// Gets or sets Tariff
        /// </summary>
        public string Tariff { get; set; }
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
        /// Gets or sets Supplier MID
        /// </summary>
        public string SupplierMid { get; set; }
        /// <summary>
        /// Gets or sets Manufacturer MID
        /// </summary>
        public string ManufacturerMid { get; set; }
        /// <summary>
        /// Gets or sets Invoice Quantity Unit
        /// </summary>
        public string InvoiceQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets Customs Quantity Unit
        /// </summary>
        public string CustomsQtyUnit { get; set; }
        /// <summary>
        /// Gets or sets Gross Weight Unit
        /// </summary>
        public string GrossWeightUq { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 1
        /// </summary>
        public string CustomsAttrib1 { get; set; }
        /// <summary>
        /// Gets or sets Customs Attribute 4
        /// </summary>
        public string CustomsAttrib4 { get; set; }
        /// <summary>
        /// Gets or sets transactions related
        /// </summary>
        public string TransactionsRelated { get; set; }
        /// <summary>
        /// Gets or sets unit price
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// API from value for corresponding goods description
        /// </summary>
        public decimal? ApiFrom { get; set; }
        /// <summary>
        /// API from value for corresponding goods description
        /// </summary>
        public decimal? ApiTo { get; set; }
        /// <summary>
        /// Gets or sets Customs Quantity
        /// </summary>
        public decimal? CustomsQty { get; set; }
        /// <summary>
        /// Gets or sets Supplier Code
        /// </summary>
        public string SupplierCode { get; set; }
        /// <summary>
        /// Gets or sets Consignee Code
        /// </summary>
        public string ConsigneeCode { get; set; }
        /// <summary>
        /// Gets or sets Freight
        /// </summary>
        public RailRuleFreightComposite FreightComposite { get; set; }
    }
}