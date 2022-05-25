namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    using FilingPortal.Domain.Entities.Truck;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Rule Port entity type configuration
    /// </summary>
    internal class TruckRulePortConfiguration : EntityTypeConfiguration<TruckRulePort>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRulePortConfiguration"/> class.
        /// </summary>
        public TruckRulePortConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRulePortConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckRulePortConfiguration(string schema)
        {
            ToTable("imp_truck_rule_port", schema);

            HasKey(x => x.Id);
            
            Property(x => x.EntryPort).IsRequired();
            Property(x => x.FIRMsCode).HasColumnName(@"firms_code");
            HasIndex(x => x.EntryPort).IsUnique().HasName("Idx__entry_port");
        }
    }
}
