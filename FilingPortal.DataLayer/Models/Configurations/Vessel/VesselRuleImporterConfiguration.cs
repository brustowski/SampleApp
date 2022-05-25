namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    using FilingPortal.Domain.Entities.Vessel;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Vessel Rule Importer entity type configuration
    /// </summary>
    internal class VesselRuleImporterConfiguration : EntityTypeConfiguration<VesselRuleImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleImporterConfiguration"/> class.
        /// </summary>
        public VesselRuleImporterConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleImporterConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselRuleImporterConfiguration(string schema)
        {
            ToTable("imp_vessel_rule_importer", schema);

            HasKey(x => x.Id);
            HasIndex(x => x.Importer).HasName("Idx__ior").IsUnique();

            Property(x => x.Importer).HasColumnName("ior").IsRequired();
            Property(x => x.CWImporter).HasColumnName("cw_ior");

            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
