using FilingPortal.Domain.Entities.Truck;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    /// <summary>
    /// Provides DB configuration for the <see cref="TruckDocument"/>
    /// </summary>
    public class TruckDocumentConfiguration : EntityTypeConfiguration<TruckDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDocumentConfiguration"/> class.
        /// </summary>
        public TruckDocumentConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDocumentConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckDocumentConfiguration(string schema)
        {
            ToTable("imp_truck_document", schema);
            HasKey(x => x.Id);

            Property(x => x.FileName).HasMaxLength(255);
            Property(x => x.Extension).HasColumnName(@"file_extension");
            Property(x => x.Description).HasColumnName(@"file_description").HasMaxLength(1000);
            Property(x => x.Content).HasColumnName(@"file_content").HasColumnType("varbinary(max)").IsOptional();
            Property(x => x.CreatedUser).IsRequired();

            HasOptional(a => a.TruckFilingHeader)
                .WithMany(b => b.TruckDocuments)
                .HasForeignKey(c => c.FilingHeaderId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.TruckInbound)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.InboundRecordId)
                .WillCascadeOnDelete(false);

            HasIndex(x => x.FilingHeaderId).HasName("Idx__filing_header_id");
            HasIndex(x => x.InboundRecordId).HasName("Idx__inbound_record_id");
        }
    }

}
