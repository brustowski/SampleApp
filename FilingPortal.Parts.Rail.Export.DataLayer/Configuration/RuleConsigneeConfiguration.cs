using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Configuration
{
    /// <summary>
    /// Provides Rule Consignee entity type configuration
    /// </summary>
    internal class RuleConsigneeConfiguration : EntityTypeConfiguration<RuleConsignee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleConsigneeConfiguration"/> class.
        /// </summary>
        public RuleConsigneeConfiguration() : this("us_exp_rail")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleConsigneeConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public RuleConsigneeConfiguration(string schema)
        {
            ToTable("rule_consignee", schema);

            HasKey(x => x.Id);
            Property(x => x.ConsigneeCode).IsRequired();
            Property(x => x.Country).HasMaxLength(2);
            Property(x => x.UltimateConsigneeType).HasMaxLength(1);
            Property(x => x.Destination).HasMaxLength(5);

            HasIndex(x => x.ConsigneeCode).IsUnique().HasName("Idx__consignee_code");
        }
    }
}
