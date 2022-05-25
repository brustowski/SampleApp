using System.Data.Entity;
using FilingPortal.DataLayer;
using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Parts.Isf.Domain.Entities;

namespace FilingPortal.Parts.Isf.DataLayer
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

        public override string DefaultSchema => "isf";

        #region Constructor
        static PluginContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PluginContext, Migrations.Configuration>());
        }
        #endregion
    }
}
