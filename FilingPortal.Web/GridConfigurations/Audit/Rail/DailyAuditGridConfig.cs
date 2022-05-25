using FilingPortal.Domain.Common;
using FilingPortal.PluginEngine.GridConfigurations;
using FilingPortal.Web.Models.Audit.Rail;
using Framework.Domain.Paging;

namespace FilingPortal.Web.GridConfigurations.Audit.Rail
{
    /// <summary>
    /// Class describing the configuration for the Rail Daily Audit grid
    /// </summary>
    public class DailyAuditGridConfig : GridConfiguration<DailyAuditViewModel>
    {
        /// <summary>
        /// Gets the name of the grid
        /// </summary>
        public override string GridName => GridNames.AuditRailDailyAudit;

        /// <summary>
        /// Configures the grid columns
        /// </summary>
        protected override void ConfigureColumns()
        {
            AddColumn(x => x.IsUnitTrain).DisplayName("Is Unit Train").EditableBoolean();
            AddColumn(x => x.JobHeaderStatus).DisplayName("Job Header Status");
            AddColumn(x => x.JobNumber).DisplayName("Job Number");
            AddColumn(x => x.Filer);
            AddColumn(x => x.EntryNumber).DisplayName("Entry No.");
            AddColumn(x => x.ImporterCode).DisplayName("Importer Code");
            AddColumn(x => x.Importer).DisplayName("Importer");
            AddColumn(x => x.ImporterNo).DisplayName("Importer No.");
            AddColumn(x => x.ExportDate).DisplayName("Export Date");
            AddColumn(x => x.ImportDate).DisplayName("Import Date");
            AddColumn(x => x.Psd).DisplayName("PSD");
            AddColumn(x => x.PaymentDueDate).DisplayName("Payment Due Date");
            AddColumn(x => x.ReleaseDate).DisplayName("Release Date");
            AddColumn(x => x.ReleaseStatus).DisplayName("Release Status Description");
            AddColumn(x => x.EnsStatusDescription).DisplayName("ENS Status Description");
            AddColumn(x => x.EntryType).DisplayName("Entry Type");
            AddColumn(x => x.EntryPort).DisplayName("Entry Port");
            AddColumn(x => x.ArrivalPort).DisplayName("Arrival Port");
            AddColumn(x => x.DestinationState).DisplayName("Destination State");
            AddColumn(x => x.LineNumber).DisplayName("Line No");
            AddColumn(x => x.CountryOfExport).DisplayName("C/E");
            AddColumn(x => x.CountryOfOrigin).DisplayName("C/O");
            AddColumn(x => x.MasterBills).DisplayName("Master Bills");
            AddColumn(x => x.ModeOfTransport).DisplayName("Trans");
            AddColumn(x => x.ContainersCount).DisplayName("Containers Count");
            AddColumn(x => x.Containers);
            AddColumn(x => x.OwnerReference).DisplayName("Owner Reference");
            AddColumn(x => x.SupplierMid).DisplayName("Supplier MID");
            AddColumn(x => x.ManufacturerMid).DisplayName("Manufacturer MID");
            AddColumn(x => x.UltimateConsigneeName).DisplayName("Ultimate Consignee Name");
            AddColumn(x => x.Carrier).DisplayName("Carrier SCAC");
            AddColumn(x => x.FirmsCode).DisplayName("FIRMS Code");
            AddColumn(x => x.Tariff);
            AddColumn(x => x.Spi).DisplayName("SPI");
            AddColumn(x => x.CustomsQty).DisplayName("Customs Qty");
            AddColumn(x => x.CustomsQtyUnit).DisplayName("Customs UQ");
            AddColumn(x => x.InvoiceQty).DisplayName("Invoice Qty");
            AddColumn(x => x.InvoiceQtyUnit).DisplayName("Invoice UQ");
            AddColumn(x => x.LinePrice).DisplayName("Line Price");
            AddColumn(x => x.UnitPrice).DisplayName("Unit Price");
            AddColumn(x => x.GrossWeight).DisplayName("Gross Weight");
            AddColumn(x => x.GrossWeightUq).DisplayName("Gross Weight UQ");
            AddColumn(x => x.Duty).DisplayName("Duty");
            AddColumn(x => x.Mpf).DisplayName("MPF");
            AddColumn(x => x.PayableMpf).DisplayName("Payable MPF");
            AddColumn(x => x.ValueRecon).DisplayName("Recon Issue");
            AddColumn(x => x.NaftaRecon).DisplayName("NAFTA Recon");
            AddColumn(x => x.GoodsDescription).DisplayName("Goods Description");
            AddColumn(x => x.CustomsAttrib1).DisplayName("Custom Attrib 1");
            AddColumn(x => x.CustomsAttrib2).DisplayName("Custom Attrib 2");
            AddColumn(x => x.CustomsAttrib3).DisplayName("Custom Attrib 3");
            AddColumn(x => x.CustomsAttrib4).DisplayName("Custom Attrib 4");
            AddColumn(x => x.TransactionsRelated).DisplayName("Transaction Related");
            AddColumn(x => x.PayType).DisplayName("Pay Type");
            AddColumn(x => x.SupplierCode).DisplayName("Supplier Code");
            AddColumn(x => x.ConsigneeCode).DisplayName("Consignee Code");
            AddColumn(x => x.Chgs).DisplayName("Freight");
            AddColumn(x => x.LastModifiedBy).DisplayName("Last Modified By");
            AddColumn(x => x.LastModifiedDate).DisplayName("Last Modified Date");
        }

        /// <summary>
        /// Configures the grid filters
        /// </summary>
        protected override void ConfigureFilters()
        {
            
            TextFilterFor(x => x.Importer).Title("Importer").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.ImporterCode).Title("Importer Code").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.GoodsDescription).Title("Goods Description").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.DestinationState).Title("Destination State").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.ValueRecon).Title("Value Recon").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.NaftaRecon).Title("NAFTA Recon").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.Tariff).SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.LastModifiedDate).Title("Last Modified Date");
            TextFilterFor(x => x.LastModifiedBy).Title("Last Modified By").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.EntryNumber).Title("Entry Number").SetOperand(FilterOperands.Contains);
            TextFilterFor(x => x.JobNumber).Title("Job Number").SetOperand(FilterOperands.Contains);
            DateRangeFilterFor(x => x.PaymentDueDate).Title("Payment Due Date");
        }
    }
}