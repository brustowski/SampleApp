using FilingPortal.Domain.Entities;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Handbooks;

namespace FilingPortal.DataLayer.Models.Configurations
{
    /// <summary>
    /// Provides Tariff entity type configuration
    /// </summary>
    class HtsTariffConfiguration : EntityTypeConfiguration<HtsTariff>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtsTariff"/> class.
        /// </summary>
        public HtsTariffConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtsTariff"/> class.
        /// </summary>
        public HtsTariffConfiguration(string schema)
        {
            ToTable("Tariff", schema);
            Property(x => x.USC_Tariff).HasColumnName(@"USC_Tariff").HasColumnType("nvarchar").HasMaxLength(35).IsRequired();
            Property(x => x.FromDateTime).HasColumnName(@"FromDateTime").HasColumnType("datetime");
            Property(x => x.ToDateTime).HasColumnName(@"ToDateTime").HasColumnType("datetime");
            Property(x => x.Short_Description).HasColumnName(@"Short_Description").HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Tariff_Type).HasColumnName(@"Tariff_Type").HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Unit).HasColumnName(@"Unit").HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.LastUpdatedTime).HasColumnName("LastUpdatedTime").HasColumnType("datetime").IsRequired();
        }
    }
}
