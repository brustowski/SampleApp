using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Admin;

namespace FilingPortal.DataLayer.Models.Configurations.Admin
{
    /// <summary>
    /// Defines the <see cref="AutoCreateRecord"/> entity to DB mapping configuration 
    /// </summary>
    internal class AutoCreateConfiguration : EntityTypeConfiguration<AutoCreateRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCreateConfiguration"/> class.
        /// </summary>
        public AutoCreateConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppAddressConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public AutoCreateConfiguration(string schema)
        {
            ToTable("admin_auto_create", schema);

            HasKey(x => x.Id);
        }
    }
}
