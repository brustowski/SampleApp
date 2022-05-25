using FilingPortal.Domain.Entities.Vessel;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    /// <summary>
    /// Provides Vessel Import Default Values type Configuration
    /// </summary>
    internal class VesselImportDefValuesConfiguration : EntityTypeConfiguration<VesselImportDefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDefValuesConfiguration"/> class.
        /// </summary>
        public VesselImportDefValuesConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportDefValuesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselImportDefValuesConfiguration(string schema)
        {
            ToTable("imp_vessel_form_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Label).IsRequired();
            Property(x => x.ColumnName).IsRequired();
            Property(x => x.CreatedUser).IsRequired();

            HasRequired(x => x.Section)
                .WithMany(x => x.Fields)
                .HasForeignKey(x => x.SectionId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.DependsOn).WithMany().HasForeignKey(x => x.DependsOnId);

        }
    }
}
