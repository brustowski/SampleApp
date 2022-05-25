namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    using FilingPortal.Domain.Entities.TruckExport;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Export Records entity type configuration
    /// </summary>
    internal class TruckExportConfiguration : EntityTypeConfiguration<TruckExportRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportConfiguration"/> class.
        /// </summary>
        public TruckExportConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public TruckExportConfiguration(string schema)
        {
            ToTable("exp_truck_inbound", schema);
            HasKey(x => x.Id);

            Property(x => x.Exporter).IsRequired();
            Property(x => x.Importer).IsRequired();
            Property(x => x.TariffType).IsRequired().HasMaxLength(3);
            Property(x => x.Tariff).IsRequired().HasMaxLength(35);
            Property(x => x.RoutedTran).IsRequired().HasMaxLength(10);
            Property(x => x.SoldEnRoute).IsRequired();
            Property(x => x.MasterBill).IsRequired();
            Property(x => x.Origin).IsRequired();
            Property(x => x.Export).IsRequired();
            Property(x => x.ECCN).IsRequired();
            Property(x => x.Hazardous).IsRequired().HasMaxLength(3);
            Property(x => x.OriginIndicator).IsRequired().HasMaxLength(1);
            Property(x => x.GoodsOrigin).IsRequired().HasMaxLength(10);
            Property(x => x.GoodsDescription).IsRequired().HasMaxLength(512);
            Property(x => x.GrossWeightUOM).IsRequired().HasMaxLength(3);
            Property(x => x.CreatedUser).IsRequired();
            Property(x => x.ValidationResult).IsMaxLength();
        }
    }
}
