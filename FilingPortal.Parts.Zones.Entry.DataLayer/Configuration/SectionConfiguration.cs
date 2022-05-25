using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.DataLayer.Configuration
{
    /// <summary>
    /// Provides Sections Configuration
    /// </summary>
    internal class SectionConfiguration : EntityTypeConfiguration<Section>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionConfiguration"/> class.
        /// </summary>
        public SectionConfiguration() : this("zones_entry")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public SectionConfiguration(string schema)
        {
            ToTable("form_section_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Title).IsRequired();
            Property(x => x.Name).IsRequired();

            HasIndex(x => x.Name).IsUnique();

            HasOptional(x => x.Parent).WithMany(x => x.Descendants).HasForeignKey(x => x.ParentId);

        }
    }
}
