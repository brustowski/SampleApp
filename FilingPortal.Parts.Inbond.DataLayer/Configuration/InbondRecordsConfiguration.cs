using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbond Records entity type configuration
    /// </summary>
    public class InbondRecordsConfiguration : EntityTypeConfiguration<InboundRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InbondRecordsConfiguration"/> class.
        /// </summary>
        public InbondRecordsConfiguration()
            : this("inbond")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InbondRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InbondRecordsConfiguration(string schema)
        {
            ToTable("inbound", schema);
            HasKey(x => x.Id);

            Property(x => x.ImporterId).IsRequired();
            Property(x => x.PortOfArrival).HasMaxLength(4);
            Property(x => x.PortOfDestination).HasMaxLength(4).IsRequired();
            Property(x => x.Carrier).IsRequired();

            HasRequired(x => x.Importer).WithMany().HasForeignKey(x => x.ImporterId).WillCascadeOnDelete(false);
        }
    }
}
