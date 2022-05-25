    using System.Data.Entity.ModelConfiguration;
    using FilingPortal.Domain.Entities.Truck;

    namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    /// <summary>
    /// Provides DB configuration for the <see cref="TruckSection"/>
    /// </summary>
    internal class TruckSectionsConfiguration : EntityTypeConfiguration<TruckSection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckSectionsConfiguration"/> class.
        /// </summary>
        public TruckSectionsConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckSectionsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckSectionsConfiguration(string schema)
        {
            ToTable("imp_truck_form_section_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Title).IsRequired();
            Property(x => x.Name).IsRequired();
            
            HasOptional(x => x.Parent).WithMany(x => x.Descendants).HasForeignKey(x => x.ParentId);
            HasIndex(x => x.ParentId).HasName("Idx__parent_id");
            HasIndex(x => x.Name).IsUnique().HasName("Idx__name");
        }
    }
}
