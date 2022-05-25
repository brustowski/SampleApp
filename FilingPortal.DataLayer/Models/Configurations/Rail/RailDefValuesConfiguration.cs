namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    using FilingPortal.Domain.Entities.Rail;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Rail DefValues Model entity type configuration
    /// </summary>
    internal class RailDefValuesConfiguration : EntityTypeConfiguration<RailDefValues>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesConfiguration"/> class.
        /// </summary>
        public RailDefValuesConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RailDefValuesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public RailDefValuesConfiguration(string schema)
        {
            ToTable("imp_rail_form_configuration", schema);

            Property(x => x.Label).IsRequired();
            Property(x => x.ColumnName).IsRequired();
            Property(x => x.CreatedUser).IsRequired();

            HasRequired(x => x.Section)
                .WithMany(x => x.Fields)
                .HasForeignKey(x => x.SectionId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.DependsOn).WithMany().HasForeignKey(x => x.DependsOnId);
        }
    }
}
