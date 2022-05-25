namespace FilingPortal.DataLayer.Models.Configurations.Vessel
{
    using FilingPortal.Domain.Entities.Vessel;
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Provides Model Configuration for <see cref="VesselImportRecord"/>
    /// </summary>
    public class VesselImportConfiguration : EntityTypeConfiguration<VesselImportRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportConfiguration"/> class.
        /// </summary>
        public VesselImportConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VesselImportConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public VesselImportConfiguration(string schema)
        {
            ToTable("imp_vessel_inbound", schema);
            HasKey(x => x.Id);

            // Columns
            Property(x => x.Container).IsRequired();
            Property(x => x.EntryType).IsRequired();
            Property(x => x.UserId).IsRequired();
            Property(x => x.OwnerRef).IsRequired();

            HasRequired(a => a.Importer).WithMany().HasForeignKey(c => c.ImporterId).WillCascadeOnDelete(false);
            HasOptional(a => a.Supplier).WithMany().HasForeignKey(c => c.SupplierId).WillCascadeOnDelete(false);
            HasOptional(a => a.State).WithMany().HasForeignKey(c => c.StateId).WillCascadeOnDelete(false);
            HasRequired(a => a.Classification).WithMany(b => b.VesselImports).HasForeignKey(c => c.ClassificationId).WillCascadeOnDelete(false);
            HasRequired(a => a.Filer).WithMany().HasForeignKey(c => c.UserId).WillCascadeOnDelete(false);
            HasRequired(a => a.Vessel).WithMany(b => b.VesselImports).HasForeignKey(c => c.VesselId).WillCascadeOnDelete(false);
            HasRequired(a => a.FirmsCode).WithMany().HasForeignKey(c => c.FirmsCodeId).WillCascadeOnDelete(false);
            HasRequired(a => a.ProductDescription).WithMany(b => b.VesselImports).HasForeignKey(c => c.ProductDescriptionId).WillCascadeOnDelete(false);
            HasOptional(a => a.CountryOfOrigin).WithMany().HasForeignKey(c => c.CountryOfOriginId).WillCascadeOnDelete(false);

            HasIndex(x => x.ClassificationId).HasName("Idx__classification_id");
            HasIndex(x => x.CountryOfOriginId).HasName("Idx__country_of_origin_id");
            HasIndex(x => x.FirmsCodeId).HasName("Idx__firms_code_id");
            HasIndex(x => x.ImporterId).HasName("Idx__importer_id");
            HasIndex(x => x.ProductDescriptionId).HasName("Idx__product_description_id");
            HasIndex(x => x.StateId).HasName("Idx__state_id");
            HasIndex(x => x.SupplierId).HasName("Idx__supplier_id");
            HasIndex(x => x.UserId).HasName("Idx__user_id");
            HasIndex(x => x.VesselId).HasName("Idx__vessel_id");

        }
    }
}
