using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Audit.Rail;

namespace FilingPortal.DataLayer.Models.Configurations.Audit.Rail
{
    /// <summary>
    /// Provides Model Configuration for <see cref="AuditRailDaily"/>
    /// </summary>
    public class AuditRailDailyConfiguration : EntityTypeConfiguration<AuditRailDaily>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyConfiguration"/> class.
        /// </summary>
        public AuditRailDailyConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public AuditRailDailyConfiguration(string schema)
        {
            ToTable("imp_rail_audit_daily", schema);

            Property(x => x.JobHeaderStatus).HasMaxLength(3);
            Property(x => x.JobNumber).HasMaxLength(35);
            Property(x => x.Filer).HasMaxLength(100);
            Property(x => x.EntryNumber).HasMaxLength(41);
            Property(x => x.Importer).HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.ImporterNo).HasColumnType("nvarchar").HasMaxLength(259);
            Property(x => x.ExportDate).HasColumnType("smalldatetime");
            Property(x => x.ImportDate).HasColumnType("smalldatetime");
            Property(x => x.Psd).HasColumnType("smalldatetime");
            Property(x => x.PaymentDueDate).HasColumnType("smalldatetime");
            Property(x => x.ReleaseDate).HasColumnType("smalldatetime");
            Property(x => x.ReleaseStatus).HasMaxLength(50);
            Property(x => x.EnsStatusDescription).HasMaxLength(200);
            Property(x => x.EntryType).HasMaxLength(2);
            Property(x => x.EntryPort).HasMaxLength(4);
            Property(x => x.ArrivalPort).HasMaxLength(4);
            Property(x => x.DestinationState).HasMaxLength(2);
            Property(x => x.CountryOfExport).HasMaxLength(2);
            Property(x => x.CountryOfOrigin).HasMaxLength(2);
            Property(x => x.MasterBills).IsMaxLength();
            Property(x => x.ModeOfTransport).HasMaxLength(3);
            Property(x => x.Containers).IsMaxLength();
            Property(x => x.SupplierMid).HasMaxLength(24);
            Property(x => x.ManufacturerMid).HasMaxLength(24);
            Property(x => x.Carrier).HasMaxLength(4);
            Property(x => x.FirmsCode).HasMaxLength(4);
            Property(x => x.Tariff).HasMaxLength(50);
            Property(x => x.Spi).HasMaxLength(3);
            Property(x => x.CustomsQtyUnit).HasMaxLength(3);
            Property(x => x.InvoiceQtyUnit).HasMaxLength(3);
            Property(x => x.LinePrice).HasColumnType("money");
            Property(x => x.GrossWeightUq).HasMaxLength(2);
            Property(x => x.Duty).HasColumnType("money");
            Property(x => x.Hmf).HasColumnType("money");
            Property(x => x.Mpf).HasColumnType("money");
            Property(x => x.PayableMpf).HasColumnType("money");
            Property(x => x.ValueRecon).HasMaxLength(2);
            Property(x => x.NaftaRecon).HasMaxLength(1);
            Property(x => x.GoodsDescription).HasMaxLength(525);
            Property(x => x.CustomsAttrib1).HasMaxLength(50);
            Property(x => x.CustomsAttrib2).HasMaxLength(50);
            Property(x => x.CustomsAttrib3).HasMaxLength(50);
            Property(x => x.CustomsAttrib4).HasMaxLength(50);
            Property(x => x.TransactionsRelated).HasMaxLength(1);
            Property(x => x.LastModifiedBy).HasMaxLength(50);
            Property(x => x.ValidationResult).IsMaxLength();
            Property(x => x.UnitPrice).HasColumnType("money");


            Property(x => x.SummaryDate).HasColumnType("SMALLDATETIME");
            Property(x => x.PortCode).HasMaxLength(5);
            Property(x => x.EntryDate).HasColumnType("SMALLDATETIME");
            Property(x => x.LocationOfGoods).HasMaxLength(100);
            Property(x => x.ConsigneeNo).HasMaxLength(24);
            Property(x => x.ImporterCode).HasMaxLength(24).IsRequired();
            Property(x => x.SupplierCode).HasMaxLength(24).IsRequired();
            Property(x => x.ConsigneeCode).HasMaxLength(24);
            Property(x => x.NetQuantityInHtsusUnits).HasMaxLength(50);
            Property(x => x.Destination).HasMaxLength(2);
        }
    }
}