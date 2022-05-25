namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    using FilingPortal.Domain.Entities.Truck;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Inbound Records entity type configuration
    /// </summary>
    internal class TruckInboundConfiguration : EntityTypeConfiguration<TruckInbound>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundConfiguration"/> class.
        /// </summary>
        public TruckInboundConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public TruckInboundConfiguration(string schema)
        {
            ToTable("imp_truck_inbound", schema);
            HasKey(x => x.Id);

            Property(x => x.ImporterCode).IsRequired();
            Property(x => x.SupplierCode).IsRequired();
            Property(x => x.PAPs).HasColumnName("paps").IsRequired().HasMaxLength(20);
            Property(x => x.Deleted).IsRequired();
            Property(x => x.CreatedUser).IsRequired();

            HasIndex(x => new {x.ImporterCode, x.SupplierCode}).HasName("Idx__importer_supplier");
        }
    }
}
