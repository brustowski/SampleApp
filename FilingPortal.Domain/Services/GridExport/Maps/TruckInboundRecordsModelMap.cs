using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing report model mapping for the Truck Inbound Records grid
    /// </summary>
    internal class TruckInboundRecordsModelMap : ReportModelMap<TruckInboundReadModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundRecordsModelMap"/> class.
        /// </summary>
        public TruckInboundRecordsModelMap()
        {
            Map(x => x.Importer);
            Map(x => x.Supplier);
            Map(x => x.PAPs);
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