using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Parts.Isf.Domain.Entities;

namespace FilingPortal.Parts.Isf.Domain.Reporting.Inbound
{
    /// <summary>
    /// Class describing report model mapping for inbound record
    /// </summary>
    internal class ModelMap : ReportModelMap<InboundReadModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelMap"/> class.
        /// </summary>
        public ModelMap()
        {
            Ignore(x => x.Id);
            Ignore(x => x.FilingHeaderId);
            Map(x => x.Eta).ColumnTitle("ETA");
            Map(x => x.Etd).ColumnTitle("ETD");
        }
    }
}