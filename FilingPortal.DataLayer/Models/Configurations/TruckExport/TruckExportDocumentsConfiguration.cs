using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.TruckExport;

namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{

    public class TruckExportDocumentsConfiguration : EntityTypeConfiguration<TruckExportDocument>
    {
        public TruckExportDocumentsConfiguration() : this("dbo") { }

        public TruckExportDocumentsConfiguration(string schema)
        {
            ToTable("exp_truck_document", schema);
            HasKey(x => x.Id);

            Property(x => x.FileName).HasMaxLength(255).IsRequired();
            Property(x => x.Extension).HasColumnName("file_extension").IsRequired();
            Property(x => x.Description).HasColumnName("file_description").HasMaxLength(1000);
            Property(x => x.Content).HasColumnName("file_content");
            Property(x => x.CreatedUser).IsRequired();

            HasOptional(a => a.FilingHeader).WithMany(b => b.Documents).HasForeignKey(c => c.FilingHeaderId).WillCascadeOnDelete(false);
            HasOptional(x => x.InboundRecord).WithMany(x => x.Documents).HasForeignKey(x => x.InboundRecordId).WillCascadeOnDelete(false);
        }
    }

}
