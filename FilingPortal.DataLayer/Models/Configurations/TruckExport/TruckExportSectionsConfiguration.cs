namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    using FilingPortal.Domain.Entities.TruckExport;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Export Sections Configuration
    /// </summary>
    internal class TruckExportSectionsConfiguration : EntityTypeConfiguration<TruckExportSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportSectionsConfiguration"/> class.
        /// </summary>
        public TruckExportSectionsConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportSectionsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckExportSectionsConfiguration(string schema)
        {
            ToTable("exp_truck_form_section_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Title).IsRequired();
            Property(x => x.Name).IsRequired();
            
            HasOptional(x => x.Parent).WithMany(x => x.Descendants).HasForeignKey(x => x.ParentId);

            HasIndex(x => x.Name).IsUnique().HasName("Idx__name");
            HasIndex(x => x.ParentId).HasName("Idx__parent_id");
        }
    }
}
