using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Enums;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using FilingPortal.Parts.Common.Domain.Enums;

namespace FilingPortal.Domain.Services.GridExport.Maps
{
    /// <summary>
    /// Class describing  report model mapping for the Vessel Import Records grid
    /// </summary>
    internal class VesselImportRecordsModelMap : ReportModelMap<VesselImportReadModel>, IReportModelMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportRecordsModelMap"/> class.
        /// </summary>
        public VesselImportRecordsModelMap()
        {
            Map(x => x.ImporterCode).ColumnTitle("Importer");
            Map(x => x.SupplierCode).ColumnTitle("Supplier");
            Map(x => x.Vessel);
            Map(x => x.CustomsQty).ColumnTitle("Customs Qty");
            Map(x => x.UnitPrice).ColumnTitle("Unit Price");
            Map(x => x.CountryOfOrigin).ColumnTitle("Country of Origin");
            Map(x => x.OwnerRef).ColumnTitle("Owner Ref");
            Map(x => x.State);
            Map(x => x.FirmsCode).ColumnTitle("FIRMs Code");
            Map(x => x.Classification);
            Map(x => x.ProductDescription).ColumnTitle("Product Description");
            Map(x => x.Eta).ColumnTitle("ETA");
            Map(x => x.FilerId).ColumnTitle("Filer ID");
            Map(x => x.Container);
            Map(x => x.EntryType).ColumnTitle("Entry Type");
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