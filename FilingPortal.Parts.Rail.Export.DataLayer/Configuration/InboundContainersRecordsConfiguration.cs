using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records Containers entity type configuration
    /// </summary>
    public class InboundContainersRecordsConfiguration : EntityTypeConfiguration<InboundRecordContainer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundContainersRecordsConfiguration"/> class.
        /// </summary>
        public InboundContainersRecordsConfiguration()
            : this("us_exp_rail")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundContainersRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundContainersRecordsConfiguration(string schema)
        {
            ToTable("inbound_containers", schema);
            HasKey(x => x.Id);

            Property(x => x.InboundRecordId).IsRequired();
            Property(x => x.ContainerNumber).IsRequired().HasMaxLength(10);
            Property(x => x.Type).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            HasRequired(x=>x.InboundRecord).WithMany(x=>x.Containers).HasForeignKey(x=>x.InboundRecordId).WillCascadeOnDelete(true);
        }
    }
}
