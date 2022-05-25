using FilingPortal.Parts.Zones.Entry.Domain.Entities;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Common.Validation;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound XML configuration
    /// </summary>
    public class RuleImporterConfiguration : EntityTypeConfiguration<RuleImporter>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundXmlConfiguration"/> class.
        /// </summary>
        public RuleImporterConfiguration()
            : this("zones_entry")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public RuleImporterConfiguration(string schema)
        {
            ToTable("rule_importer", schema);

            HasRequired(x=>x.Importer).WithMany().HasForeignKey(x=>x.ImporterId).WillCascadeOnDelete(false);

            HasKey(x => x.Id);
        }
    }
}
