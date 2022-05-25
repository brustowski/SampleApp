namespace FilingPortal.DataLayer.Models.Configurations.TruckExport
{
    using FilingPortal.Domain.Entities.TruckExport;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Truck Export Default Values type Configuration
    /// </summary>
    internal class TruckExportDefValuesConfiguration : EntityTypeConfiguration<TruckExportDefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDefValuesConfiguration"/> class.
        /// </summary>
        public TruckExportDefValuesConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckExportDefValuesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckExportDefValuesConfiguration(string schema)
        {
            ToTable("exp_truck_form_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Label).IsRequired();
            Property(x => x.SingleFilingOrder).IsOptional();
            Property(x => x.ColumnName).IsRequired();
            Property(x => x.CreatedUser).IsRequired();

            HasRequired(x => x.Section)
                .WithMany(x => x.Fields)
                .HasForeignKey(x => x.SectionId)
                .WillCascadeOnDelete(false);
            HasOptional(x => x.DependsOn).WithMany().HasForeignKey(x => x.DependsOnId);

        }
    }
}
