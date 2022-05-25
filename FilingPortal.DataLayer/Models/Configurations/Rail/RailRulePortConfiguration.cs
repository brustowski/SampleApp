using FilingPortal.Domain.Entities.Rail;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    internal class RailRulePortConfiguration : EntityTypeConfiguration<RailRulePort>
    {
        public RailRulePortConfiguration() : this("dbo") { }

        public RailRulePortConfiguration(string schema)
        {
            ToTable("imp_rail_rule_port", schema);

            Property(x => x.FIRMsCode).HasColumnName("firms_code");
        }
    }
}
