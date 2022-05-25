using FilingPortal.Domain.Entities.Rail;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    internal class RailRuleDescriptionConfiguration : EntityTypeConfiguration<RailRuleDescription>
    {
        public RailRuleDescriptionConfiguration() : this("dbo") { }

        public RailRuleDescriptionConfiguration(string schema)
        {
            ToTable("imp_rail_rule_product", schema);

            HasKey(x => x.Id);
            HasIndex(x => new { x.Description1, x.Importer, x.Supplier, x.Port, x.Destination }).HasName("Idx__description_importer_supplier_port_dest").IsUnique();

            Property(x => x.Description1).IsRequired().HasColumnType("nvarchar").HasMaxLength(500);
            Property(x => x.Port).HasMaxLength(4);
            Property(x => x.Destination).HasMaxLength(2);
            Property(x => x.ProductID).HasColumnName(@"prod_id_1");
            Property(x => x.Attribute1).HasColumnName(@"attribute_1");
            Property(x => x.Attribute2).HasColumnName(@"attribute_2");
        }
    }
}
