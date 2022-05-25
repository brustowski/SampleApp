using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Configuration
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
            : this("us_exp_rail")
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

            Property(x => x.IsDeleted).HasColumnName(@"deleted");
        }
    }
}
