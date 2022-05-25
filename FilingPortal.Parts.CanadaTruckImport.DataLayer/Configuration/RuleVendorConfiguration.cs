using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Vendor Rule entity configuration
    /// </summary>
    internal class RuleVendorConfiguration : EntityTypeConfiguration<RuleVendor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleVendorConfiguration"/> class.
        /// </summary>
        public RuleVendorConfiguration() : this("canada_imp_truck")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleVendorConfiguration"/> class.
        /// </summary>
        /// <param name="schema">Database schema</param>
        public RuleVendorConfiguration(string schema)
        {
            ToTable("rule_vendor", schema);

            Property(x => x.ExportState).HasMaxLength(2);
            Property(x => x.CountryOfOrigin).HasMaxLength(2);
            Property(x => x.CreatedUser).IsRequired();

            HasRequired(x => x.Vendor)
                .WithMany()
                .HasForeignKey(x => x.VendorId)
                .WillCascadeOnDelete(false);
            HasRequired(x => x.Importer)
                .WithMany()
                .HasForeignKey(x => x.ImporterId)
                .WillCascadeOnDelete(false);
            HasRequired(x => x.Exporter)
                .WithMany()
                .HasForeignKey(x => x.ExporterId)
                .WillCascadeOnDelete(false);

            HasIndex(x => x.VendorId).IsUnique(true);
        }
    }
}
