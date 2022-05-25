using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.Domain.Reporting.Inbound
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
            Ignore(x=>x.HasEntryRule);
            Ignore(x=>x.MappingStatus);
            Ignore(x=>x.IsDeleted);
            Ignore(x=>x.EntryDate);
            Map(x => x.ExportConveyance).ColumnTitle("Conveyance");
        }
    }
}