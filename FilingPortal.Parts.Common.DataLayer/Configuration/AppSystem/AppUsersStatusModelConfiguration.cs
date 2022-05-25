using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Provides App users status entity type configuration
    /// </summary>
    class AppUsersStatusModelConfiguration : EntityTypeConfiguration<AppUsersStatusModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUsersStatusModelConfiguration"/> class.
        /// </summary>
        public AppUsersStatusModelConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUsersStatusModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public AppUsersStatusModelConfiguration(string schema)
        {
            ToTable("app_users_status", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"ID").HasColumnType("int").IsRequired();
            Property(x=>x.Name).HasColumnName(@"Name").HasColumnType("nvarchar").HasMaxLength(4000).IsRequired();
        }
    }
}
