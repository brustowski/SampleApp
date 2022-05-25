using System.Data.Entity.ModelConfiguration;
using FilingPortal.DataLayer.Models.Configurations.Truck;
using FilingPortal.Domain.Entities.TruckExport;

namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    /// <summary>
    /// Provides Truck Export Rule Consignee entity type configuration
    /// </summary>
    internal class TruckExportRuleExporterConsigneeConfiguration : EntityTypeConfiguration<TruckExportRuleExporterConsignee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRuleExporterConsigneeConfiguration"/> class.
        /// </summary>
        public TruckExportRuleExporterConsigneeConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRuleImporterConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckExportRuleExporterConsigneeConfiguration(string schema)
        {
            ToTable("exp_truck_rule_exporter_consignee", schema);

            HasKey(x => x.Id);
            Property(x => x.Exporter).IsRequired();
            Property(x => x.ConsigneeCode).IsRequired();
            Property(x => x.TranRelated).HasMaxLength(1);
            HasIndex(x => new {x.Exporter, x.ConsigneeCode}).IsUnique().HasName("Idx__consignee_code__exporter");
        }
    }
}
