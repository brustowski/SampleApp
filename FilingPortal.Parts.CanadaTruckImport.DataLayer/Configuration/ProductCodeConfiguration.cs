using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer.Configuration
{
    /// <summary>
    /// Provides Product Code entity configuration
    /// </summary>
    internal class ProductCodeConfiguration : EntityTypeConfiguration<ProductCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCodeConfiguration"/> class.
        /// </summary>
        public ProductCodeConfiguration() : this("canada_imp_truck")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCodeConfiguration"/> class.
        /// </summary>
        /// <param name="schema">Database schema</param>
        public ProductCodeConfiguration(string schema)
        {
            ToTable("handbook_product_code", schema);
            HasKey(x => x.Id);

            Property(x => x.Code).IsRequired().HasMaxLength(35);
            Property(x => x.Description).IsRequired().HasMaxLength(80);
        }
    }
}
