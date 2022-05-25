using System.Data.Entity.ModelConfiguration;
using FilingPortal.DataLayer.Models.Configurations.Vessel;
using FilingPortal.Domain.Entities.VesselExport;

namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    /// <summary>
    /// Provides Vessel Export Rule Consignee entity type configuration
    /// </summary>
    internal class VesselExportRuleUsppiConsigneeConfiguration : EntityTypeConfiguration<VesselExportRuleUsppiConsignee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportRuleUsppiConsigneeConfiguration"/> class.
        /// </summary>
        public VesselExportRuleUsppiConsigneeConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselRuleImporterConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselExportRuleUsppiConsigneeConfiguration(string schema)
        {
            ToTable("exp_vessel_rule_usppi_consignee", schema);

            HasKey(x => x.Id);
            Property(x => x.UsppiId).IsRequired();
            Property(x => x.ConsigneeId).IsRequired();
            Property(x => x.UltimateConsigneeType).HasMaxLength(10);
            HasIndex(x => new {x.UsppiId, x.ConsigneeId}).IsUnique();

            HasRequired(x=>x.Usppi).WithMany().HasForeignKey(x=>x.UsppiId).WillCascadeOnDelete(false);
            HasRequired(x => x.Consignee).WithMany().HasForeignKey(x => x.ConsigneeId).WillCascadeOnDelete(false);
            HasOptional(x => x.ConsigneeAddress).WithMany().HasForeignKey(x => x.ConsigneeAddressId).WillCascadeOnDelete(false);
        }
    }
}
