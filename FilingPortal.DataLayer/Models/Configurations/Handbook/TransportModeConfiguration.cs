using FilingPortal.Domain.Entities.Handbooks;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Handbook
{
    /// <summary>
    /// Provides Model Configuration for <see cref="TransportMode"/>
    /// </summary>
    public class TransportModeConfiguration : EntityTypeConfiguration<TransportMode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransportModeConfiguration"/> class.
        /// </summary>
        public TransportModeConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportModeConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public TransportModeConfiguration(string schema)
        {
            ToTable("handbook_transport_mode", schema);
            HasKey(x => x.Id);

            //Columns
            Property(x => x.CodeNumber).HasMaxLength(2);
            Property(x => x.Code).HasMaxLength(3).IsRequired();
            Property(x => x.Country).HasMaxLength(2).IsRequired();
            Property(x => x.ServiceCode).HasMaxLength(10);
            Property(x => x.ContainerCode).HasMaxLength(10);
        }
    }
}
