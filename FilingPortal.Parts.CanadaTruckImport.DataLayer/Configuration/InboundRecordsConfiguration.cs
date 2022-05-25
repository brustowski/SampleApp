using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundRecordsConfiguration : EntityTypeConfiguration<InboundRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        public InboundRecordsConfiguration()
            : this("canada_imp_truck")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundRecordsConfiguration(string schema)
        {
            ToTable("inbound", schema);
            HasKey(x => x.Id);

            Property(x => x.Port).HasMaxLength(4).IsRequired();
            Property(x => x.ParsNumber).IsRequired();

            HasRequired(x => x.Vendor).WithMany().HasForeignKey(x => x.VendorId).WillCascadeOnDelete(false);
            HasRequired(x => x.Consignee).WithMany().HasForeignKey(x => x.ConsigneeId).WillCascadeOnDelete(false);
            HasRequired(x => x.ProductCode).WithMany().HasForeignKey(x => x.ProductCodeId).WillCascadeOnDelete(false);
        }
    }
}
