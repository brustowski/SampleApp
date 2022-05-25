using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Isf.Domain.Entities;

namespace FilingPortal.Parts.Isf.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound bills records entity type configuration
    /// </summary>
    public class InboundBillRecordsConfiguration : EntityTypeConfiguration<InboundBillRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundBillRecordsConfiguration"/> class.
        /// </summary>
        public InboundBillRecordsConfiguration()
            : this("isf")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundBillRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundBillRecordsConfiguration(string schema)
        {
            ToTable("inbound_bills", schema);
            HasKey(x => x.Id);
            Property(x => x.InboundRecordId).IsRequired();
            Property(x => x.BillType).IsRequired().HasMaxLength(2);
            Property(x => x.BillNumber).IsRequired().HasMaxLength(16);

            HasRequired(x => x.Inbound).WithMany(x => x.Bills).HasForeignKey(x => x.InboundRecordId).WillCascadeOnDelete(true);
        }
    }
}
