using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.AppSystem
{
    /// <summary>
    /// Defines the <see cref="AppAddress"/> entity to DB mapping configuration 
    /// </summary>
    internal class AppAddressConfiguration : EntityTypeConfiguration<AppAddress>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppAdminConfiguration"/> class.
        /// </summary>
        public AppAddressConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppAddressConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public AppAddressConfiguration(string schema)
        {
            ToTable("app_addresses", schema);

            HasKey(x => x.Id);

            Property(x => x.IsOverriden).IsRequired();
            HasOptional(x => x.CwAddress).WithMany().HasForeignKey(x => x.CwAddressId).WillCascadeOnDelete(true);
        }
    }
}
