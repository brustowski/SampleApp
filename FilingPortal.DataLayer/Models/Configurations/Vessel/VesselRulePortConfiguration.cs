namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    using FilingPortal.Domain.Entities.Vessel;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Vessel Rule Port entity type configuration
    /// </summary>
    internal class VesselRulePortConfiguration : EntityTypeConfiguration<VesselRulePort>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRulePortConfiguration"/> class.
        /// </summary>
        public VesselRulePortConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRulePortConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselRulePortConfiguration(string schema)
        {
            ToTable("imp_vessel_rule_port", schema);
            HasKey(x => x.Id);

            HasIndex(x => x.FirmsCodeId).IsUnique().HasName("Idx__firms_code_id");

            Property(x => x.EntryPort).HasMaxLength(4);
            Property(x => x.DischargePort).HasMaxLength(4);
            Property(x => x.HMF).HasMaxLength(1);

            HasRequired(x => x.FirmsCode).WithMany().HasForeignKey(x => x.FirmsCodeId)
                .WillCascadeOnDelete(false);
            
            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
