using System.Data.Entity.ModelConfiguration;
using FilingPortal.DataLayer.Models.Configurations.Truck;
using FilingPortal.Domain.Entities.TruckExport;

namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    /// <summary>
    /// Provides Truck Export Rule Consignee entity type configuration
    /// </summary>
    internal class TruckExportRuleConsigneeConfiguration : EntityTypeConfiguration<TruckExportRuleConsignee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportRuleConsigneeConfiguration"/> class.
        /// </summary>
        public TruckExportRuleConsigneeConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckRuleImporterConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckExportRuleConsigneeConfiguration(string schema)
        {
            ToTable("exp_truck_rule_consignee", schema);

            HasKey(x => x.Id);
            Property(x => x.ConsigneeCode).IsRequired();
            Property(x => x.Country).HasMaxLength(2);
            Property(x => x.UltimateConsigneeType).HasMaxLength(1);
            Property(x => x.Destination).HasMaxLength(5);

            HasIndex(x => x.ConsigneeCode).IsUnique().HasName("Idx__consignee_code");
        }
    }
}
