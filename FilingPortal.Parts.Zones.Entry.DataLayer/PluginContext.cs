using System.Data.Entity;
using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Parts.Zones.Entry.Domain.Entities;

namespace FilingPortal.Parts.Zones.Entry.DataLayer
{
    public class PluginContext : FpContext
    {
        /// <summary>
        /// Inbound records table
        /// </summary>
        public DbSet<InboundRecord> Inbound { get; set; }
        /// <summary>
        /// Inbound Document table
        /// </summary>
        public DbSet<Document> Documents { get; set; }
        /// <summary>
        /// The Schema
        /// </summary>
        public override string DefaultSchema => "zones_entry";

        #region Constructor
        static PluginContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PluginContext, Migrations.Configuration>());
        }
        #endregion
    }
}
