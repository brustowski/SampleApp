using System.Data.Entity.ModelConfiguration;
using FilingPortal.Cargowise.Domain.Entities;

namespace FilingPortal.Cargowise.DataLayer.Configuration
{
    /// <summary>
    /// Provides Model Configuration for <see cref="PortOfClearance"/>
    /// </summary>
    public class PortOfClearanceConfiguration : EntityTypeConfiguration<PortOfClearance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PortOfClearanceConfiguration"/> class.
        /// </summary>
        public PortOfClearanceConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PortOfClearanceConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public PortOfClearanceConfiguration(string schema)
        {
            ToTable("handbook_cw_ports_of_clearance", schema);
            HasKey(x => x.Id);

            //Columns
            Property(x => x.Code).HasColumnName("value").HasMaxLength(4).HasColumnType("nvarchar").IsRequired();
            Property(x => x.Office).HasColumnName("display_value").HasColumnType("nvarchar").HasMaxLength(255);
            Property(x => x.Province).HasMaxLength(2).HasColumnType("char");

            HasIndex(x => x.Code).HasName("Idx_value").IsUnique();
        }
    }
}
