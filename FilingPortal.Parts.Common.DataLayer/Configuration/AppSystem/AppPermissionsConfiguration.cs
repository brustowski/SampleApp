using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Provides Application Permission entity type configuration 
    /// </summary>
    internal class AppPermissionsConfiguration : EntityTypeConfiguration<AppPermission>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppPermissionsConfiguration"/> class.
        /// </summary>
        public AppPermissionsConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppPermissionsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public AppPermissionsConfiguration(string schema)
        {
            ToTable("App_Permissions", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);

            HasIndex(x => x.Name).IsUnique();
            Property(x => x.Name).IsRequired();

            HasMany(x => x.Roles).WithMany(x => x.Permissions).Map(x =>
               x.MapLeftKey("App_Permissions_FK").MapRightKey("App_Roles_FK").ToTable("App_Permissions_Roles", schema)
            );
        }
    }
}
