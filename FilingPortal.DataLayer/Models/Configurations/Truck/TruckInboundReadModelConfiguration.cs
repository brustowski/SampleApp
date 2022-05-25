using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Truck;

namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    /// <summary>
    /// Defines the Truck Inbound DB mapping configuration
    /// </summary>
    public class TruckInboundReadModelConfiguration : EntityTypeConfiguration<TruckInboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundReadModelConfiguration"/> class.
        /// </summary>
        public TruckInboundReadModelConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckInboundReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema name</param>
        public TruckInboundReadModelConfiguration(string schema)
        {
            ToTable("v_imp_truck_inbound_grid", schema);

            Property(x => x.FilingHeaderId).IsOptional();
            Property(x => x.Importer).IsRequired();
            Property(x => x.Supplier).IsRequired();
            Property(x => x.PAPs).HasColumnName("paps").HasMaxLength(20);
            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.MappingStatus).HasColumnType("int");
            Property(x => x.MappingStatusTitle).HasMaxLength(20);
            Property(x => x.FilingStatus).HasColumnType("int");
            Property(x => x.FilingStatusTitle).HasMaxLength(20);
            Property(x => x.IsDeleted).HasColumnName(@"deleted");
        }
    }
}
