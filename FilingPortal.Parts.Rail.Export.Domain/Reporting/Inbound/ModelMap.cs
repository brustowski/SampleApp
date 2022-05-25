using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.Domain.Reporting.Inbound
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
        }
    }
}