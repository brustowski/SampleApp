using System.ComponentModel.DataAnnotations.Schema;
using FilingPortal.Domain.Entities.TruckExport;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    /// <summary>
    /// Provides the DB configuration for the Truck Export Filing Header
    /// </summary>
    public class TruckExportFilingHeaderConfiguration : EntityTypeConfiguration<TruckExportFilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportFilingHeaderConfiguration"/> class.
        /// </summary>
        public TruckExportFilingHeaderConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportFilingHeaderConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckExportFilingHeaderConfiguration(string schema)
        {
            ToTable("exp_truck_filing_header", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.JobLink).IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.JobStatus).HasColumnType("int");

            Property(x => x.LastModifiedDate).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed); // it should be read only!

            HasMany(t => t.TruckExports).WithMany(t => t.FilingHeaders)
            .Map(m =>
            {
                m.ToTable("exp_truck_filing_detail", "dbo");
                m.MapLeftKey("filing_header_id");
                m.MapRightKey("inbound_id");
            });
        }
    }
}
