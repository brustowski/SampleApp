using FilingPortal.Domain.Entities.Vessel;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    /// <summary>
    /// Provides Vessel Import Sections Configuration
    /// </summary>
    internal class VesselImportSectionsConfiguration : EntityTypeConfiguration<VesselImportSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportSectionsConfiguration"/> class.
        /// </summary>
        public VesselImportSectionsConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportSectionsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselImportSectionsConfiguration(string schema)
        {
            ToTable("imp_vessel_form_section_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Title).IsRequired();
            Property(x => x.Name).IsRequired();

            HasIndex(x => x.Name).IsUnique().HasName("Idx__name");
            HasIndex(x => x.ParentId).HasName("Idx__parent_id");

            HasOptional(x => x.Parent).WithMany(x => x.Descendants).HasForeignKey(x => x.ParentId);

        }
    }
}
