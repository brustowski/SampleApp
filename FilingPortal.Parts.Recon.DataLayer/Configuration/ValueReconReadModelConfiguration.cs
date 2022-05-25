using FilingPortal.Parts.Recon.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides Value Recon read model entity type configuration
    /// </summary>
    public class ValueReconReadModelConfiguration : EntityTypeConfiguration<ValueReconReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconReadModelConfiguration"/> class.
        /// </summary>
        public ValueReconReadModelConfiguration() : this("recon") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueReconReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public ValueReconReadModelConfiguration(string schema)
        {
            ToTable("v_value_recon", schema);
        }
    }
}
