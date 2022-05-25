using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Provides Application Role entity type configuration 
    /// </summary>
    class AppRolesConfiguration : EntityTypeConfiguration<AppRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppRolesConfiguration"/> class.
        /// </summary>
        public AppRolesConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppRolesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public AppRolesConfiguration(string schema)
        {
            ToTable("App_Roles", schema);

            HasKey(x => x.Id);
        }
    }
}
