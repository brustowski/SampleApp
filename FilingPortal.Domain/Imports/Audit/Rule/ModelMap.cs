using FilingPortal.Domain.Common.Parsing;

namespace FilingPortal.Domain.Imports.Audit.Rule
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="DailyAuditRuleImportModel"/>
    /// </summary>
    internal class ModelMap : ParseModelMap<DailyAuditRuleImportModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DailyAuditRuleImportModel"/> class.
        /// </summary>
        public ModelMap()
        {
            Map(p => p.ImporterCode, "Importer");
            Map(p => p.SupplierCode, "Supplier Code");
            Map(p => p.ConsigneeCode, "Consignee Code");
            Map(p => p.GoodsDescription, "Goods Description");
            Map(p => p.Tariff);
            Map(p => p.PortCode, "Port Code");
            Map(p => p.DestinationState, "Destination State");
            Map(p => p.Carrier);
            Map(p => p.ExportingCountry, "Country of Export");
            Map(p => p.CountryOfOrigin, "Country of Origin");
            Map(p => p.UltimateConsigneeName, "Ultimate Consignee Name");
            Map(p => p.FirmsCode, "FIRMs Code");
            Map(p => p.ApiFrom, "API from");
            Map(p => p.ApiTo, "API to");
            Map(p => p.CustomsQty, "Customs Qty per container");
            Map(p => p.CustomsQtyUnit, "Customs Qty unit");
            Map(p => p.UnitPrice, "Unit Price");
            Map(p => p.ValueRecon, "Value Recon");
            Map(p => p.NaftaRecon, "NAFTA Recon");
            Map(p => p.ManufacturerMid, "Manufacturer MID");
            Map(p => p.SupplierMid, "Supplier MID");
            Map(p => p.InvoiceQtyUnit, "Invoice Qty Unit");
            Map(p => p.GrossWeightUq, "Gross Weight Unit");
            Map(p => p.CustomsAttrib1, "Customs Attrib 1");

            Sheet("Daily Audit Rule");
        }
    }
}
