using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Port Rule entity configuration
    /// </summary>
    internal class RulePortConfiguration : EntityTypeConfiguration<RulePort>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RulePortConfiguration"/> class.
        /// </summary>
        public RulePortConfiguration() : this("canada_imp_truck")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RulePortConfiguration"/> class.
        /// </summary>
        /// <param name="schema">Database schema</param>
        public RulePortConfiguration(string schema)
        {
            ToTable("rule_port", schema);

            Property(x => x.PortOfClearance).IsRequired().HasMaxLength(4);
            Property(x => x.FirstPortOfArrival).HasMaxLength(5);
            Property(x => x.FinalDestination).HasMaxLength(5);
            Property(x => x.SubLocation).HasMaxLength(50);
            Property(x => x.CreatedUser).IsRequired();
        }
    }
}
