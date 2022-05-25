using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using FilingPortal.Domain.Services.GridExport.Models;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing  report model mapping for the Rail Inbound Records grid
    /// </summary>
    internal class RailInboundRecordsModelMap : ReportModelMap<RailInboundRecordsReportModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailInboundRecordsReportModel"/> class.
        /// </summary>
        public RailInboundRecordsModelMap()
        {
            Map(x => x.Importer).ColumnTitle("Importer");
            Map(x => x.Supplier).ColumnTitle("Supplier");
            Map(x => x.TrainNumber).ColumnTitle("Train #");
            Map(x => x.BOLNumber).ColumnTitle("BoL #");
            Map(x => x.IssuerCode).ColumnTitle("Issuer Code");
            Map(x => x.ContainerNumber).ColumnTitle("Container #");
            Map(x => x.PortCode).ColumnTitle("Port Code");
            Map(x => x.HTS).ColumnTitle("HTS");
            Map(x => x.CreatedDate).ColumnTitle("Creation Date").UseFormatter<DateTimeFormatter>();
            Map(x => x.FilingNumber).ColumnTitle("Job #");
            Map(x => x.MappingStatus).ColumnTitle("Mapping Status").UseFormatter<EnumFormatter<MappingStatus>>();
            Map(x => x.FilingStatus).ColumnTitle("Filing Status").UseFormatter<EnumFormatter<FilingStatus>>();
            Ignore(x => x.IsDeleted);
            Ignore( x=> x.Id);
        }
    }
}