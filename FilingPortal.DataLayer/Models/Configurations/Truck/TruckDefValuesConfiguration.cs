using FilingPortal.Domain.Entities.Truck;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Truck
{
    /// <summary>
    /// Provides DB configuration for the <see cref="TruckDefValue"/>
    /// </summary>
    internal class TruckDefValuesConfiguration : EntityTypeConfiguration<TruckDefValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDefValuesConfiguration"/> class.
        /// </summary>
        public TruckDefValuesConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TruckDefValuesConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public TruckDefValuesConfiguration(string schema)
        {
            ToTable("imp_truck_form_configuration", schema);
            HasKey(x => x.Id);

            Property(x => x.Label).IsRequired();
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
