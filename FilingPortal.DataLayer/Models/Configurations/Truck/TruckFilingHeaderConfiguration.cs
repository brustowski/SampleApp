using FilingPortal.Domain.Entities.Truck;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    /// <summary>
    /// Provides DB Configuration for the <see cref="TruckFilingHeader"/>
    /// </summary>
    public class TruckFilingHeaderConfiguration : EntityTypeConfiguration<TruckFilingHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckFilingHeaderConfiguration"/> class.
        /// </summary>
        public TruckFilingHeaderConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckFilingHeaderConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckFilingHeaderConfiguration(string schema)
        {
            ToTable("imp_truck_filing_header", schema);
            HasKey(x => x.Id);

            Property(x => x.FilingNumber).HasMaxLength(255);
            Property(x => x.JobLink).IsMaxLength();
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.MappingStatus).HasColumnType("int");
            Property(x => x.FilingStatus).HasColumnType("int");

            HasMany(t => t.TruckInbounds).WithMany(t => t.FilingHeaders).Map(m =>
            {
                m.ToTable("imp_truck_filing_detail", "dbo");
                m.MapLeftKey("filing_header_id");
                m.MapRightKey("inbound_id");
            });
        }
    }
}
