using System.Data.Entity.ModelConfiguration;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;

namespace FilingPortal.Cargowise.DataLayer.Configuration
{
    /// <summary>
    /// Provides USStates entity type configuration
    /// </summary>
    class USStatesConfiguration : EntityTypeConfiguration<USStates>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="USStates"/> class.
        /// </summary>
        public USStatesConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="USStates"/> class.
        /// </summary>
        public USStatesConfiguration(string schema)
        {
            ToTable("US_States", schema);
            Property(x => x.StateName).HasColumnName(@"StateName").HasColumnType("nvarchar").HasMaxLength(128).IsRequired();
            Property(x => x.StateCode).HasColumnName(@"StateCode").HasColumnType("nvarchar").HasMaxLength(2);
        }
    }
}
