using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Audit.Rail;

namespace FilingPortal.DataLayer.Models.Configurations.Audit.Rail
{
    /// <summary>
    /// Provides Model Configuration for <see cref="AuditRailDaily"/>
    /// </summary>
    public class AuditRailDailyRulesConfiguration : EntityTypeConfiguration<AuditRailDailyRule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyRulesConfiguration"/> class.
        /// </summary>
        public AuditRailDailyRulesConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailyRulesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public AuditRailDailyRulesConfiguration(string schema)
        {
            ToTable("imp_rail_audit_daily_rules", schema);
            Property(x => x.ImporterCode).HasMaxLength(24).IsRequired();
            Property(x => x.Tariff).HasMaxLength(50).IsRequired();
            Property(x => x.GoodsDescription).HasMaxLength(525).IsRequired();

            Property(x => x.ConsigneeCode).HasMaxLength(24);
            Property(x => x.SupplierCode).HasMaxLength(24);
            Property(x => x.DestinationState).HasMaxLength(2);
            Property(x => x.PortCode).HasMaxLength(5);

            Property(x => x.Carrier).HasMaxLength(4);
            Property(x => x.CountryOfOrigin).HasMaxLength(2);
            Property(x => x.SupplierMid).HasMaxLength(16);
            Property(x => x.ManufacturerMid).HasMaxLength(16);
            Property(x => x.ExportingCountry).HasMaxLength(2);
            Property(x => x.UltimateConsigneeName).HasMaxLength(500);
            Property(x => x.InvoiceQtyUnit).HasMaxLength(3);
            Property(x => x.CustomsQtyUnit).HasMaxLength(3);
            Property(x => x.GrossWeightUq).HasMaxLength(3);
            Property(x => x.CustomsAttrib1).HasMaxLength(50);
            Property(x => x.CustomsAttrib4).HasMaxLength(50);
            Property(x => x.ValueRecon).HasMaxLength(2);
            Property(x => x.NaftaRecon).HasMaxLength(1);
            Property(x => x.UnitPrice).HasColumnType("money");
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.TransactionsRelated).HasMaxLength(1);

            HasIndex(t => new { t.ImporterCode, t.SupplierCode, t.ConsigneeCode, t.GoodsDescription, t.Tariff, t.PortCode, t.DestinationState, t.CustomsAttrib4 }).IsUnique();
            HasRequired(x => x.Author).WithMany().HasForeignKey(x => x.CreatedUser).WillCascadeOnDelete(true);
        }
    }
}