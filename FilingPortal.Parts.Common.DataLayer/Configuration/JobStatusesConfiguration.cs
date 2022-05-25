using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities;

namespace FilingPortal.Parts.Common.DataLayer.Configuration
{
    /// <summary>
    /// Provides Job Status entity type configuration
    /// </summary>
    internal class JobStatusesConfiguration : EntityTypeConfiguration<HeaderJobStatus>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobStatusesConfiguration"/> class.
        /// </summary>
        public JobStatusesConfiguration()
        {
            ToTable("job_statuses", "common");

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasMaxLength(20).IsRequired();
            Property(x => x.Code).HasMaxLength(3);
        }
    }
}
