using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Configuration
{
    /// <summary>
    /// Provides Rule Exporter-Consignee entity type configuration
    /// </summary>
    internal class RuleExporterConsigneeConfiguration : EntityTypeConfiguration<RuleExporterConsignee>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleExporterConsigneeConfiguration"/> class.
        /// </summary>
        public RuleExporterConsigneeConfiguration() : this("us_exp_rail")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuleExporterConsigneeConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public RuleExporterConsigneeConfiguration(string schema)
        {
            ToTable("rule_exporter_consignee", schema);

            HasKey(x => x.Id);
            Property(x => x.Exporter).IsRequired();
            Property(x => x.ConsigneeCode).IsRequired();
            Property(x => x.TranRelated).HasMaxLength(1);
            HasIndex(x => new { x.Exporter, x.ConsigneeCode }).IsUnique().HasName("Idx__consignee_code__exporter");
        }
    }
}
