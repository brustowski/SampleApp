using FilingPortal.Domain.Entities.Rail;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    internal class RailRuleImporterSupplierConfiguration : EntityTypeConfiguration<RailRuleImporterSupplier>
    {
        public RailRuleImporterSupplierConfiguration() : this("dbo") { }

        public RailRuleImporterSupplierConfiguration(string schema)
        {
            ToTable("imp_rail_rule_importer_supplier", schema);

            Property(x => x.ImporterName).HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.SupplierName).HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.ProductDescription).HasColumnType("nvarchar").HasMaxLength(500);
            Property(x => x.Port).HasMaxLength(4);
            Property(x => x.Destination).HasMaxLength(2);
            Property(x => x.Value).HasColumnType("numeric").HasPrecision(18, 6);

            HasIndex(x => new { x.ImporterName, x.SupplierName, x.ProductDescription, x.Port, x.Destination }).HasName("Idx__importer_supplier_description_port_destination").IsUnique();
        }
    }
}
