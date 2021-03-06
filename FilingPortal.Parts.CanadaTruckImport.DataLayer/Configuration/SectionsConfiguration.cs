using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Sections Configuration
    /// </summary>
    internal class SectionsConfiguration : EntityTypeConfiguration<Section>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectionsConfiguration"/> class.
        /// </summary>
        public SectionsConfiguration() : this("canada_imp_truck")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public SectionsConfiguration(string schema)
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
