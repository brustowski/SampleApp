using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Inbond.Domain.Entities;

namespace FilingPortal.Parts.Inbond.DataLayer.Configuration
{
    /// <summary>
    /// Provides Rail DefValues Model entity type configuration
    /// </summary>
    internal class DefValuesConfiguration : EntityTypeConfiguration<DefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesConfiguration"/> class.
        /// </summary>
        public DefValuesConfiguration() : this("inbond")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefValuesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="DefValuesConfiguration"/></param>
        public DefValuesConfiguration(string schema)
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
