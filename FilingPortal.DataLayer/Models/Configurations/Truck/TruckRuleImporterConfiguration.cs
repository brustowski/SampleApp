namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    using FilingPortal.Domain.Entities.Truck;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Rule Importer  entity type configuration
    /// </summary>
    internal class TruckRuleImporterConfiguration : EntityTypeConfiguration<TruckRuleImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRuleImporterConfiguration"/> class.
        /// </summary>
        public TruckRuleImporterConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRuleImporterConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckRuleImporterConfiguration(string schema)
        {
            ToTable("imp_truck_rule_importer", schema);

            HasKey(x => x.Id);
            HasIndex(x => new {x.CWIOR, x.CWSupplier}).IsUnique().HasName("Idx__importer_supplier");
            HasIndex(x => new {x.ArrivalPort, x.EntryPort}).HasName("Idx__arrival_port__entry_port");

            Property(x => x.CWIOR).HasColumnName(@"cw_ior").IsRequired();
            Property(x => x.CWSupplier).IsRequired();
        }
    }
}
