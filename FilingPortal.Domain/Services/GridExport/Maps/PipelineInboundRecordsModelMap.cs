using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing  report model mapping for the Pipeline Inbound Records grid
    /// </summary>
    internal class PipelineInboundRecordsModelMap : ReportModelMap<PipelineInboundReadModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineInboundRecordsModelMap"/> class.
        /// </summary>
        public PipelineInboundRecordsModelMap()
        {
            Map(x => x.Importer);
            Map(x => x.Batch);
            Map(x => x.TicketNumber).ColumnTitle("Ticket #");
            Map(x => x.Facility);
            Map(x => x.Quantity);
            Map(x => x.API);
            Map(x => x.ExportDate).ColumnTitle("Export Date");
            Map(x => x.ImportDate).ColumnTitle("Import Date");
            Map(x => x.CreatedDate).ColumnTitle("Creation Date").UseFormatter<DateTimeFormatter>();
            Map(x => x.FilingNumber).ColumnTitle("Job #");
            Map(x => x.MappingStatus).ColumnTitle("Mapping Status").UseFormatter<EnumFormatter<MappingStatus>>();
            Map(x => x.FilingStatus).ColumnTitle("Filing Status").UseFormatter<EnumFormatter<FilingStatus>>();
            Ignore(x => x.IsDeleted);
            Ignore(x => x.FilingHeaderId);
            Ignore(x => x.Id);
        }
    }
}