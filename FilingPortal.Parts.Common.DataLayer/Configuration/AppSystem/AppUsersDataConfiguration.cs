using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Provides App users entity type configuration 
    /// </summary>
    internal class AppUsersDataConfiguration : EntityTypeConfiguration<AppUsersData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUsersModelConfiguration"/> class.
        /// </summary>
        public AppUsersDataConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUsersModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public AppUsersDataConfiguration(string schema)
        {
            ToTable("app_users_data", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"UserAccount").IsRequired();
            Property(x => x.Branch).HasColumnName("Branch").IsOptional();
            Property(x => x.Broker).HasColumnName("Broker").IsOptional();
            Property(x => x.Location).HasColumnName("Location").IsOptional();
        }
    }
}
