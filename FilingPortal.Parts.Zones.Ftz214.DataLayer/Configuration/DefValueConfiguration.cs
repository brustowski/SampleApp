using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Zones.Ftz214.Domain.Entities;

namespace FilingPortal.Parts.Zones.Ftz214.DataLayer.Configuration
{
    /// <summary>
    /// Provides DefValues Model entity type configuration
    /// </summary>
    internal class DefValueConfiguration : EntityTypeConfiguration<DefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValueConfiguration"/> class.
        /// </summary>
        public DefValueConfiguration() : this("zones_ftz214")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefValueConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="DefValueConfiguration"/></param>
        public DefValueConfiguration(string schema)
        {
            ToTable("form_configuration", schema);

            Property(x => x.Label).IsRequired();
            Property(x => x.ColumnName).IsRequired();
            Property(x => x.CreatedUser).IsRequired();

            HasRequired(x => x.Section)
                .WithMany(x => x.Fields)
                .HasForeignKey(x => x.SectionId)
                .WillCascadeOnDelete(false);
        }
    }
}
