using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Audit.Rail;

namespace FilingPortal.DataLayer.Models.Configurations.Audit.Rail
{
    /// <summary>
    /// Provides Model Configuration for <see cref="AuditRailTrainConsistSheet"/>
    /// </summary>
    public class AuditRailTrainConsistSheetConfiguration : EntityTypeConfiguration<AuditRailTrainConsistSheet>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailTrainConsistSheetConfiguration"/> class.
        /// </summary>
        public AuditRailTrainConsistSheetConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuditRailTrainConsistSheetConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public AuditRailTrainConsistSheetConfiguration(string schema)
        {
            ToTable("imp_rail_audit_train_consist_sheet", schema);

            Property(x => x.CreatedUser).IsRequired();

            HasRequired(x => x.Author).WithMany().HasForeignKey(x => x.CreatedUser).WillCascadeOnDelete(true);
        }
    }
}