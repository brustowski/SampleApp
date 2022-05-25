using FilingPortal.Domain.Entities;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Handbooks;

namespace FilingPortal.DataLayer.Models.Configurations
{
    /// <summary>
    /// Provides SchB Tariff entity type configuration
    /// </summary>
    class SchbTariffConfiguration : EntityTypeConfiguration<SchbTariff>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchbTariff"/> class.
        /// </summary>
        public SchbTariffConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchbTariff"/> class.
        /// </summary>
        public SchbTariffConfiguration(string schema)
        {
            ToTable("SchB_Tariff", schema);
            Property(x => x.UB_Tariff).HasColumnName(@"UB_Tariff").HasColumnType("nvarchar").HasMaxLength(35).IsRequired();
            Property(x => x.Short_Description).HasColumnName(@"Short_Description").HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Tariff_Type).HasColumnName(@"Tariff_Type").HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.Unit).HasColumnName(@"Unit").HasColumnType("nvarchar").HasMaxLength(128);
            Property(x => x.LastUpdatedTime).HasColumnName("LastUpdatedTime").HasColumnType("datetime").IsRequired();
        }
    }
}
