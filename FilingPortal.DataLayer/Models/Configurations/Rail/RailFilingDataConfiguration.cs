namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    using FilingPortal.Domain.Entities.Rail;

    /// <summary>
    /// Provides Model Configuration for <see cref="RailFilingData" />
    /// </summary>
    public class RailFilingDataConfiguration : System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<RailFilingData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailFilingDataConfiguration"/> class.
        /// </summary>
        public RailFilingDataConfiguration()
            : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RailFilingDataConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public RailFilingDataConfiguration(string schema)
        {
            ToTable("v_imp_rail_review_grid", schema);

            Property(x => x.BOLNumber).HasColumnName(@"bill_of_lading");
            Property(x => x.ManifestRecordId).HasColumnName(@"manifest_id");
        }
    }
}
