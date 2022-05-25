using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Provides Application Settings entity type configuration 
    /// </summary>
    class AppSettingsConfiguration : EntityTypeConfiguration<AppSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsConfiguration"/> class.
        /// </summary>
        public AppSettingsConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppSettingsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public AppSettingsConfiguration(string schema)
        {
            ToTable("app_settings", schema);

            HasKey(x => x.Id);
            Property(x => x.Description).HasMaxLength(500);
            Property(x => x.Value).HasMaxLength(500);
        }
    }
}
