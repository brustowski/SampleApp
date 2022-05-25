using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Defines the <see cref="AppAdmin"/> entity to DB mapping configuration 
    /// </summary>
    internal class AppAdminConfiguration : EntityTypeConfiguration<AppAdmin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppAdminConfiguration"/> class.
        /// </summary>
        public AppAdminConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppAdminConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public AppAdminConfiguration(string schema)
        {
            ToTable("app_admins", schema);

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            Property(x => x.FullName).HasColumnName("FullName").HasColumnType("varchar").HasMaxLength(100).IsRequired();

            HasIndex(x => x.Email).IsUnique();
            Property(x => x.Email).HasColumnName("Email").HasColumnType("varchar").HasMaxLength(255).IsRequired();
        }
    }
}
