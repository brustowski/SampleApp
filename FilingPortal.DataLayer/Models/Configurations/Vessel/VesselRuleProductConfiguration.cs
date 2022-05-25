namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    using FilingPortal.Domain.Entities.Vessel;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Vessel Rule Importer entity type configuration
    /// </summary>
    internal class VesselRuleProductConfiguration : EntityTypeConfiguration<VesselRuleProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleProductConfiguration"/> class.
        /// </summary>
        public VesselRuleProductConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleProductConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselRuleProductConfiguration(string schema)
        {
            ToTable("imp_vessel_rule_product", schema);

            HasKey(x => x.Id);
            HasIndex(x => x.Tariff).HasName("Idx__tariff").IsUnique();

            Property(x => x.Tariff).IsRequired().HasMaxLength(10);
            Property(x => x.CreatedUser).IsRequired();            
        }
    }
}
