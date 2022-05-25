using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundReadModelConfiguration : EntityTypeConfiguration<InboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundReadModelConfiguration"/> class.
        /// </summary>
        public InboundReadModelConfiguration()
            : this("canada_imp_truck")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundReadModelConfiguration(string schema)
        {
            ToTable("v_inbound_grid", schema);

            Property(x => x.FilingStatus).HasColumnType("int");
            Property(x => x.MappingStatus).HasColumnType("int");
        }
    }
}
