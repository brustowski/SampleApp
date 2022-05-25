using System.Data.Entity.ModelConfiguration;
using FilingPortal.Cargowise.Domain.Entities.CargoWise;

namespace FilingPortal.Cargowise.DataLayer.Configuration
{
    /// <summary>
    /// Provides synchronisable table for FIRMs Codes
    /// </summary>
    internal class CwFirmsCodesConfiguration : EntityTypeConfiguration<CargowiseFirmsCodes>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CwFirmsCodesConfiguration"/> class.
        /// </summary>
        public CwFirmsCodesConfiguration() : this("dbo") { }
        /// <summary>
        /// Initializes a new instance of the <see cref="CwFirmsCodesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public CwFirmsCodesConfiguration(string schema)
        {
            ToTable("cw_firms_codes", schema);

            HasKey(x => x.Id);

            Property(x => x.FirmsCode).HasColumnType("varchar").IsRequired().HasMaxLength(4);
            Property(x => x.Name).IsRequired();
            Property(x => x.IsActive).IsRequired();

            HasIndex(x => x.FirmsCode).IsUnique();

            HasOptional(x => x.Country).WithMany(x => x.FirmsCodes).HasForeignKey(x => x.CountryId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.State).WithMany(x => x.FirmsCodes).HasForeignKey(x => x.StateId)
                .WillCascadeOnDelete(false);
        }
    }
}
