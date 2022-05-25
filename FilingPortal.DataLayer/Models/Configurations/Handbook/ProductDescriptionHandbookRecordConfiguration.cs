using FilingPortal.Domain.Entities.Vessel;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Domain.Entities.Handbooks;

namespace FilingPortal.DataLayer.Models.Configurations.Handbook
{
    /// <summary>
    /// Provides Model Configuration for <see cref="ProductDescriptionHandbookRecord"/>
    /// </summary>
    public class ProductDescriptionHandbookRecordConfiguration : EntityTypeConfiguration<ProductDescriptionHandbookRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDescriptionHandbookRecordConfiguration"/> class.
        /// </summary>
        public ProductDescriptionHandbookRecordConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDescriptionHandbookRecordConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public ProductDescriptionHandbookRecordConfiguration(string schema)
        {
            ToTable("handbook_tariff_product_description", schema);
            HasKey(x => x.Id);

            //Columns
            Property(x => x.Name).IsRequired();

            HasRequired(x => x.Tariff)
                .WithMany(x => x.ProductDescriptions)
                .HasForeignKey(x => x.TariffId)
                .WillCascadeOnDelete(false);

            HasIndex(x => x.TariffId).HasName("Idx__tariff_id");
        }
    }
}
