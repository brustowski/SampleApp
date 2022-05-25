using FilingPortal.Domain.Entities.Handbooks;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Handbook
{
    /// <summary>
    /// Provides Model Configuration for <see cref="EntryType"/>
    /// </summary>
    public class EntryTypeConfiguration : EntityTypeConfiguration<EntryType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryTypeConfiguration"/> class.
        /// </summary>
        public EntryTypeConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryTypeConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public EntryTypeConfiguration(string schema)
        {
            ToTable("handbook_entry_type", schema);
            HasKey(x => x.Id);

            //Columns
            Property(x => x.Id).HasMaxLength(2);
        }
    }
}
