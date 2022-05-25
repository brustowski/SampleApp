namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    using FilingPortal.Domain.Entities.Rail;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Rail Sections Configuration
    /// </summary>
    internal class RailSectionsConfiguration : EntityTypeConfiguration<RailSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailSectionsConfiguration"/> class.
        /// </summary>
        public RailSectionsConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RailSectionsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public RailSectionsConfiguration(string schema)
        {
            ToTable("imp_rail_form_section_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Title).IsRequired();
            Property(x => x.Name).IsRequired();

            HasIndex(x => x.Name).IsUnique();

            HasOptional(x => x.Parent).WithMany(x => x.Descendants).HasForeignKey(x => x.ParentId);

        }
    }
}
