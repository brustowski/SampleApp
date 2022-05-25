namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    using FilingPortal.Domain.Entities.VesselExport;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Vessel Export Sections Configuration
    /// </summary>
    internal class VesselExportSectionsConfiguration : EntityTypeConfiguration<VesselExportSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportSectionsConfiguration"/> class.
        /// </summary>
        public VesselExportSectionsConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportSectionsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselExportSectionsConfiguration(string schema)
        {
            ToTable("exp_vessel_form_section_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Title).IsRequired();
            Property(x => x.Name).IsRequired();

            HasIndex(x => x.Name).IsUnique();

            HasOptional(x => x.Parent).WithMany(x => x.Descendants).HasForeignKey(x => x.ParentId);

        }
    }
}
