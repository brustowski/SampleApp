using FilingPortal.Parts.Inbond.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides the Entry rule entity type configuration
    /// </summary>
    public class RuleEntryConfiguration : EntityTypeConfiguration<RuleEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleEntryConfiguration"/> class.
        /// </summary>
        public RuleEntryConfiguration() : this("inbond")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleEntryConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public RuleEntryConfiguration(string schema)
        {
            ToTable("rule_entry", schema);
            HasKey(x => x.Id);

            Property(x => x.FirmsCodeId).IsRequired();
            Property(x => x.ImporterId).IsRequired();
            Property(x => x.Carrier).IsRequired();
            Property(x => x.ConsigneeId).IsRequired();
            Property(x => x.UsPortOfDestination).HasMaxLength(4).IsRequired();
            Property(x => x.TransportMode).HasMaxLength(2);

            HasIndex(x => new { x.FirmsCodeId, x.ImporterId, x.Carrier, x.ConsigneeId, x.UsPortOfDestination}).IsUnique();

            HasRequired(x => x.FirmsCode).WithMany().HasForeignKey(x => x.FirmsCodeId).WillCascadeOnDelete(false);
            HasRequired(x => x.Importer).WithMany().HasForeignKey(x => x.ImporterId).WillCascadeOnDelete(false);
            HasRequired(x => x.Consignee).WithMany().HasForeignKey(x => x.ConsigneeId).WillCascadeOnDelete(false);
            HasRequired(x => x.Shipper).WithMany().HasForeignKey(x => x.ShipperId).WillCascadeOnDelete(false);
            HasOptional(x => x.ConsigneeAddress).WithMany().HasForeignKey(x => x.ConsigneeAddressId).WillCascadeOnDelete(false);
        }
    }
}
