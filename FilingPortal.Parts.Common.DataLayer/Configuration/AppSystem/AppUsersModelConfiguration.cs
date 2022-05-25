using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Provides App users entity type configuration 
    /// </summary>
    internal class AppUsersModelConfiguration : EntityTypeConfiguration<AppUsersModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUsersModelConfiguration"/> class.
        /// </summary>
        public AppUsersModelConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="AppUsersModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public AppUsersModelConfiguration(string schema)
        {
            ToTable("app_users", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName(@"UserAccount").IsRequired();
            Property(x => x.RequestInfo).HasColumnName("RequestInfo").HasColumnType("nvarchar").HasMaxLength(4000).IsOptional();
            Property(x => x.StatusId).HasColumnName("StatusId").HasColumnType("int").IsRequired();

            HasRequired(x => x.Status).WithMany(x => x.AppUsers).HasForeignKey(x => x.StatusId).WillCascadeOnDelete(false);
            HasMany(x => x.Roles).WithMany(x => x.AppUsers).Map(x =>
                x.MapLeftKey("App_Users_FK").MapRightKey("App_Roles_FK").ToTable("App_Users_Roles", "dbo")
            );
        }
    }
}
