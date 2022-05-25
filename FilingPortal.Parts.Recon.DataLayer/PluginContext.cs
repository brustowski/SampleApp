using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Parts.Recon.Domain.Entities;
using System.Data.Entity;

namespace FilingPortal.Parts.Recon.DataLayer
{
    public class PluginContext : FpContext
    {
        /// <summary>
        /// Inbound records table
        /// </summary>
        public DbSet<InboundRecord> Inbound { get; set; }
        /// <summary>
        /// Gets or sets the Inbound Record Read model db set
        /// </summary>
        public DbSet<InboundRecordReadModel> InboundRecordReadModels { get; set; }

        /// <summary>
        /// Gets or sets the FTA Recon db set
        /// </summary>
        public DbSet<FtaRecon> FtaRecons { get; set; }
        /// <summary>
        /// Gets or sets the FTA Recon Read model db set
        /// </summary>
        public DbSet<FtaReconReadModel> FtaReconReadModels { get; set; }
        /// <summary>
        /// Gets or sets the FTA Recon status db set
        /// </summary>
        public DbSet<FtaReconStatus> FtaReconStatuses { get; set; }

        /// <summary>
        /// Gets or sets the value recon db set
        /// </summary>
        public DbSet<ValueRecon> ValueRecons { get; set; }
        /// <summary>
        /// Gets or sets the value recon read model db set
        /// </summary>
        public DbSet<ValueReconReadModel> ValueReconReadModels { get; set; }
        /// <summary>
        /// Gets or sets the Value Recon status db set
        /// </summary>
        public DbSet<ValueReconStatus> ValueReconStatuses { get; set; }

        /// <summary>
        /// Gets or sets the context schema
        /// </summary>
        public override string DefaultSchema => "recon";

        #region Constructor
        static PluginContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PluginContext, Migrations.Configuration>());
        }
        #endregion
    }
}
