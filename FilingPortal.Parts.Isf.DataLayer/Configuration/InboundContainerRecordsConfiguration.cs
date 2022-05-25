using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Isf.Domain.Entities;

namespace FilingPortal.Parts.Isf.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound container records entity type configuration
    /// </summary>
    public class InboundContainerRecordsConfiguration : EntityTypeConfiguration<InboundContainerRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundContainerRecordsConfiguration"/> class.
        /// </summary>
        public InboundContainerRecordsConfiguration()
            : this("isf")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundContainerRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundContainerRecordsConfiguration(string schema)
        {
            ToTable("inbound_containers", schema);
            HasKey(x => x.Id);
            Property(x => x.InboundRecordId).IsRequired();
            Property(x => x.ContainerType).IsRequired();
            Property(x => x.ContainerNumber).IsRequired();

            HasRequired(x => x.Inbound).WithMany(x => x.Containers).HasForeignKey(x => x.InboundRecordId).WillCascadeOnDelete(true);
        }
    }
}
