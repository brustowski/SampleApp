using FilingPortal.Domain.Entities.Handbooks;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Handbook
{
    /// <summary>
    /// Provides Model Configuration for <see cref="EntryStatus"/>
    /// </summary>
    public class EntryStatusConfiguration : EntityTypeConfiguration<EntryStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryStatusConfiguration"/> class.
        /// </summary>
        public EntryStatusConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntryStatusConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public EntryStatusConfiguration(string schema)
        {
            ToTable("handbook_entry_status", schema);
            HasKey(x => x.Id);

            //Columns
            Property(x => x.Code).HasMaxLength(3).IsRequired();
            Property(x => x.StatusType).IsRequired();
            Property(x => x.Description).IsRequired();

            HasIndex(x => new { x.Code, x.StatusType }).IsUnique();
        }
    }
}
