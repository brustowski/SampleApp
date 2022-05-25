using System.Data.Entity;
using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Parts.Rail.Export.Domain.Entities;

namespace FilingPortal.Parts.Rail.Export.DataLayer
{
    public class PluginContext : FpContext
    {
        /// <summary>
        /// Inbound records table
        /// </summary>
        public DbSet<InboundRecord> Inbound { get; set; }
        /// <summary>
        /// Inbound records main grid models
        /// </summary>
        public DbSet<InboundReadModel> InboundGrid { get; set; }
        /// <summary>
        /// Filing headers
        /// </summary>
        public DbSet<FilingHeader> FilingHeaders { get; set; }
        /// <summary>
        /// Configuration sections
        /// </summary>
        public DbSet<Section> Sections { get; set; }
        #region Rules

        #endregion
        #region Handbooks

        #endregion

        public override string DefaultSchema => "us_exp_rail";

        #region Constructor
        static PluginContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PluginContext, Migrations.Configuration>());
        }

        #endregion
    }
}
