using System.Data.Entity;
using FilingPortal.DataLayer;
using FilingPortal.Parts.CanadaTruckImport.Domain.Entities;
using FilingPortal.Parts.Common.DataLayer.Base;

namespace FilingPortal.Parts.CanadaTruckImport.DataLayer
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
        /// <summary>
        /// The Vendor rules
        /// </summary>
        public DbSet<RuleVendor> VendorRules { get; set; }
        /// <summary>
        /// Rules for ports
        /// </summary>
        public DbSet<RulePort> PortRules { get; set; }
        /// <summary>
        /// The Product Rules 
        /// </summary>
        public DbSet<RuleProduct> ProductRules { get; set; }
        #endregion
        #region Handbooks
        /// <summary>
        /// Carriers handbook
        /// </summary>
        public DbSet<Carrier> Carriers { get; set; }
        /// <summary>
        /// Product code handbook
        /// </summary>
        public DbSet<ProductCode> ProductCodes { get; set; }
        #endregion

        public override string DefaultSchema => "canada_imp_truck";

        #region Constructor
        static PluginContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PluginContext, Migrations.Configuration>());
        }
        #endregion
    }
}
