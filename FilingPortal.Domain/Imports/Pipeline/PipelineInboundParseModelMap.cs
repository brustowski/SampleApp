using FilingPortal.Domain.Common.Parsing;
using FilingPortal.Domain.DTOs.Pipeline;

namespace FilingPortal.Domain.Imports.Pipeline
{
    /// <summary>
    /// Provides Excel file data mapping on <see cref="PipelineInboundImportParsingModel"/>
    /// </summary>
    internal class PipelineInboundParseModelMap : ParseModelMap<PipelineInboundImportParsingModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundParseModelMap"/> class.
        /// </summary>
        public PipelineInboundParseModelMap()
        {
            Sheet("Batch Log");

            Map(p => p.Importer, "Importer");
            Map(p => p.Batch, "Batch");
            Map(p => p.TicketNumber, "Ticket No.");
            Map(p => p.Facility, "Facility");
            Map(p => p.SiteName, "SiteName");
            Map(p => p.Quantity, "Quantity");
            Map(p => p.API, "API");
            Map(p => p.ExportDate, "Export Date");
            Map(p => p.ImportDate, "Import Date");
            Map(p => p.EntryNumber, "Entry Number");
        }
    }
}
