using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Models.Audit.Rail;

namespace FilingPortal.Domain.Services.GridExport.Maps.Audit.Rail
{
    /// <summary>
    /// Class describing report model mapping for the Audit Rail Daily Audit
    /// </summary>
    internal class AuditRailDailyAuditRulesModelMap : ReportModelMap<AuditRailDailyAuditRulesReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyAuditRulesModelMap"/> class.
        /// </summary>
        public AuditRailDailyAuditRulesModelMap()
        {
            Ignore(x => x.Id);

            Map(x => x.ImporterCode).ColumnTitle("Importer");
            Map(x => x.PortCode).ColumnTitle("Port");
            Map(x => x.ExportingCountry).ColumnTitle("Country Of Export");
            Map(x => x.FirmsCode).ColumnTitle("FIRMs Code");
            Map(x => x.ApiFrom).ColumnTitle("API from");
            Map(x => x.ApiTo).ColumnTitle("API to");
            Map(x => x.CustomsQty).ColumnTitle("Customs Qty per container");
            Map(x => x.NaftaRecon).ColumnTitle("NAFTA Recon");
            Map(x => x.ManufacturerMid).ColumnTitle("Manufacturer MID");
            Map(x => x.SupplierMid).ColumnTitle("Supplier MID");
        }
    }
}