using FilingPortal.Parts.Recon.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.Parts.Recon.DataLayer.Configuration
{
    /// <summary>
    /// Provides FTA Recon read model entity type configuration
    /// </summary>
    public class FtaReconReadModelConfiguration : EntityTypeConfiguration<FtaReconReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconReadModelConfiguration"/> class.
        /// </summary>
        public FtaReconReadModelConfiguration() : this("recon") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FtaReconReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public FtaReconReadModelConfiguration(string schema)
        {
            ToTable("v_fta_recon", schema);
        }
    }
}
