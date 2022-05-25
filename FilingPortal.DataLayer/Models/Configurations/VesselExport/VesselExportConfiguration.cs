namespace FilingPortal.DataLayer.Models.Configurations.VesselExport
{
    using FilingPortal.Domain.Entities.VesselExport;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Vessel Export Records entity type configuration
    /// </summary>
    internal class VesselExportConfiguration : EntityTypeConfiguration<VesselExportRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportConfiguration"/> class.
        /// </summary>
        public VesselExportConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselExportConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public VesselExportConfiguration(string schema)
        {
            ToTable("exp_vessel_inbound", schema);
            HasKey(x => x.Id);

            Property(x => x.LoadPort).IsRequired();
            Property(x => x.TransportRef).IsRequired();
            Property(x => x.Value).IsRequired();
            Property(x => x.Container).IsRequired();
            Property(x => x.TariffType).IsRequired().HasMaxLength(4);
            Property(x => x.Tariff).IsRequired();
            Property(x => x.DischargePort).IsRequired();
            Property(x => x.ExportAdjustmentValue).IsRequired();
            Property(x => x.InBond).IsRequired().HasMaxLength(2);
            Property(x => x.SoldEnRoute).IsRequired();
            Property(x => x.OriginIndicator).IsRequired().HasMaxLength(1);
            Property(x => x.GoodsDescription).IsRequired().HasMaxLength(512);
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.ReferenceNumber).IsRequired();
            Property(x => x.RoutedTransaction).IsRequired().HasMaxLength(1);
            Property(x => x.Description).IsRequired().HasMaxLength(512);

            HasRequired(x => x.Usppi).WithMany().HasForeignKey(x => x.UsppiId).WillCascadeOnDelete(false);
            HasOptional(x => x.Address).WithMany().HasForeignKey(x=>x.AddressId).WillCascadeOnDelete(false);
            HasRequired(x => x.Importer).WithMany().HasForeignKey(x => x.ImporterId).WillCascadeOnDelete(false);
            HasRequired(x => x.Vessel).WithMany(x => x.VesselExports).HasForeignKey(x => x.VesselId).WillCascadeOnDelete(false);
            HasRequired(x => x.CountryOfDestination).WithMany().HasForeignKey(x => x.CountryOfDestinationId).WillCascadeOnDelete(false);
            HasOptional(x=>x.Contact).WithMany().HasForeignKey(x=>x.ContactId).WillCascadeOnDelete(false);
        }
    }
}
