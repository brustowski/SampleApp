using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Audit.Rail;

namespace FilingPortal.DataLayer.Models.Configurations.Audit.Rail
{
    /// <summary>
    /// Provides Model Configuration for <see cref="AuditRailDailySpiRule"/>
    /// </summary>
    public class AuditRailDailySpiRulesConfiguration : EntityTypeConfiguration<AuditRailDailySpiRule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailySpiRulesConfiguration"/> class.
        /// </summary>
        public AuditRailDailySpiRulesConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailDailySpiRulesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public AuditRailDailySpiRulesConfiguration(string schema)
        {
            ToTable("imp_rail_audit_daily_spi_rules", schema);
            Property(x => x.ImporterCode).HasMaxLength(24).IsRequired();
            Property(x => x.SupplierCode).HasMaxLength(24);
            Property(x => x.GoodsDescription).HasMaxLength(525).IsRequired();
            Property(x => x.DestinationState).HasMaxLength(2);
            Property(x => x.CustomsAttrib4).HasMaxLength(50);

            Property(x => x.Spi).HasMaxLength(3).IsRequired();
            Property(x => x.CreatedUser).IsRequired();

            HasIndex(t => new { t.ImporterCode, t.SupplierCode, t.GoodsDescription, t.DestinationState, t.DateFrom, t.DateTo, t.CustomsAttrib4 }).IsUnique();
            HasRequired(x => x.Author).WithMany().HasForeignKey(x => x.CreatedUser).WillCascadeOnDelete(true);
        }
    }
}