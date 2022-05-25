using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Product Rule entity configuration
    /// </summary>
    internal class RuleProductConfiguration : EntityTypeConfiguration<RuleProduct>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleProductConfiguration"/> class.
        /// </summary>
        public RuleProductConfiguration() : this("canada_imp_truck")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleProductConfiguration"/> class.
        /// </summary>
        /// <param name="schema">Database schema</param>
        public RuleProductConfiguration(string schema)
        {
            ToTable("rule_product", schema);

            Property(x => x.GrossWeightUnit).IsRequired().HasMaxLength(3);
            Property(x => x.PackagesUnit).IsRequired().HasMaxLength(3);
            Property(x => x.InvoiceUQ).IsRequired().HasMaxLength(3);
            Property(x => x.CreatedUser).IsRequired();

            HasRequired(x=>x.ProductCode).WithMany().HasForeignKey(x=>x.ProductCodeId).WillCascadeOnDelete(false);

            HasIndex(x => x.ProductCodeId).IsUnique(true);
        }
    }
}
