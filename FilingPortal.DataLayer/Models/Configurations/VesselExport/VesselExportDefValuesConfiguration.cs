namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    using FilingPortal.Domain.Entities.VesselExport;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Vessel Export Default Values type Configuration
    /// </summary>
    internal class VesselExportDefValuesConfiguration : EntityTypeConfiguration<VesselExportDefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDefValuesConfiguration"/> class.
        /// </summary>
        public VesselExportDefValuesConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportDefValuesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselExportDefValuesConfiguration(string schema)
        {
            ToTable("exp_vessel_form_configuration", schema);
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
