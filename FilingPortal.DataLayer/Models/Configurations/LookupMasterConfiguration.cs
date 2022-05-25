using FilingPortal.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations
{
    /// <summary>
    /// Provides lookupmaster entity type configuration
    /// </summary>
    class LookupMasterConfiguration : EntityTypeConfiguration<LookupMaster>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LookupMaster"/> class.
        /// </summary>
        public LookupMasterConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LookupMaster"/> class.
        /// </summary>
        public LookupMasterConfiguration(string schema)
        {
            ToTable("LookupMaster", schema);
            Property(x => x.DisplayValue).HasColumnName(@"DisplayValue").HasColumnType("nvarchar").HasMaxLength(50).IsRequired();
            Property(x => x.Value).HasColumnName(@"Value").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Type).HasColumnName(@"Type").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.Description).HasColumnName(@"Description").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.LastUpdatedTime).HasColumnName("LastUpdatedTime").HasColumnType("datetime").IsRequired();
        }
    }
}
