using FilingPortal.Parts.Common.DataLayer.Base;
using FilingPortal.Domain.DocumentTypes.Entities;
using FilingPortal.Domain.Entities;
using FilingPortal.Domain.Entities.Audit.Rail;
using FilingPortal.Domain.Entities.Handbooks;
using FilingPortal.Domain.Entities.Pipeline;
using FilingPortal.Domain.Entities.Rail;
using FilingPortal.Domain.Entities.Truck;
using FilingPortal.Domain.Entities.TruckExport;
using FilingPortal.Domain.Entities.Vessel;
using FilingPortal.Domain.Entities.VesselExport;
using System.Data.Entity;
using FilingPortal.DataLayer.Migrations;
using FilingPortal.Parts.Common.Domain.Entities;
using FilingPortal.Parts.Common.Domain.Entities.AppSystem;
using FilingPortal.Parts.Common.Domain.Entities.Clients;
using FilingPortal.Cargowise.DataLayer;

namespace FilingPortal.DataLayer
{
    public class FilingPortalContext : FpContext
    {
        #region Common
        public DbSet<Client> ClientReadModels { get; set; }
        public DbSet<ClientAddress> ClientAddresses { get; set; }
        public DbSet<ClientCode> ClientCodes { get; set; }
        public DbSet<HeaderMappingStatus> HeaderMappingStatuses { get; set; }
        public DbSet<HeaderFilingStatus> HeaderFilingStatuses { get; set; }
        public DbSet<LookupMaster> LookupMasters { get; set; }
        
        public DbSet<HtsTariff> Tariffs { get; set; }
        
        public DbSet<IssuerCode> IssuerCodes { get; set; }
        public DbSet<TransportMode> TransportModes { get; set; }
        public DbSet<EntryType> EntryTypes { get; set; }
        public DbSet<EntryStatus> EntryStatuses { get; set; }
        #endregion

        #region Rail
        public DbSet<RailBdParsed> RailBdParseds { get; set; }
        public DbSet<RailDocument> RailDocuments { get; set; }
        public DbSet<RailEdiMessage> RailEdiMessages { get; set; }
        public DbSet<RailFilingHeader> RailFilingHeaders { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<RailInboundReadModel> RailInboundGrids { get; set; }
        public DbSet<RailSection> RailSections { get; set; }
        public DbSet<RailRuleImporterSupplier> RailRuleImporterSuppliers { get; set; }
        public DbSet<RailRuleDescription> RailRuleDescription { get; set; }
        public DbSet<RailRulePort> RailRulePort { get; set; }
        public DbSet<RailDefValues> RailDefValues { get; set; }
        public DbSet<RailTables> RailTables { get; set; }
        #endregion

        #region Pipeline
        public DbSet<PipelineInbound> PipelineInbounds { get; set; }
        public DbSet<PipelineInboundReadModel> PipelineInboundGrids { get; set; }
        public DbSet<PipelineSection> PipelineSections { get; set; }
        public DbSet<PipelineDefValue> PipelineDefValues { get; set; }
        public DbSet<PipelineDefValueReadModel> PipelineDefValueReadModels { get; set; }
        public DbSet<PipelineDocument> PipelineDocuments { get; set; }
        public DbSet<PipelineTable> PipelineTables { get; set; }
        public DbSet<PipelineRuleImporter> PipelineRuleImporters { get; set; }
        public DbSet<PipelineRuleBatchCode> PipelineRuleBatchCodes { get; set; }
        public DbSet<PipelineRuleConsigneeImporter> PipelineRuleConsigneeImporters { get; set; }
        #endregion

        #region Truck
        public DbSet<TruckInbound> TruckInbounds { get; set; }
        public DbSet<TruckFilingHeader> TruckFilingHeaders { get; set; }
        public DbSet<TruckInboundReadModel> TruckInboundGrids { get; set; }
        public DbSet<TruckDocument> TruckDocuments { get; set; }
        public DbSet<TruckDefValue> TruckDefValues { get; set; }
        public DbSet<TruckDefValueReadModel> TruckDefValuesReadModels { get; set; }
        public DbSet<TruckSection> TruckSections { get; set; }
        public DbSet<TruckRuleImporter> TruckRuleImporters { get; set; }
        public DbSet<TruckRulePort> TruckRulePorts { get; set; }
        #endregion

        #region TruckExport

        public DbSet<TruckExportRecord> TruckExports { get; set; }
        public DbSet<TruckExportReadModel> TruckExportReadModels { get; set; }
        public DbSet<TruckExportFilingHeader> TruckExportFilingHeaders { get; set; }
        public DbSet<TruckExportSection> TruckExportSections { get; set; }
        public DbSet<TruckExportDefValue> TruckExportDefValues { get; set; }
        public DbSet<TruckExportTable> TruckExportTables { get; set; }
        public DbSet<TruckExportDocument> TruckExportDocuments { get; set; }
        public DbSet<TruckExportRuleConsignee> TruckExportRuleConsignees { get; set; }
        public DbSet<TruckExportRuleExporterConsignee> TruckExportRuleExportersConsignees { get; set; }

        #endregion

        #region Vessel
        public DbSet<VesselRuleImporter> VesselRuleImporters { get; set; }
        public DbSet<VesselRulePort> VesselRulePorts { get; set; }
        public DbSet<VesselRuleProduct> VesselRuleProducts { get; set; }
        public DbSet<VesselImportFilingHeader> VesselImportFilingHeaders { get; set; }
        public DbSet<VesselImportRecord> VesselImports { get; set; }
        public DbSet<VesselImportReadModel> VesselInboundGrids { get; set; }
        public DbSet<VesselImportSection> VesselImportSections { get; set; }
        public DbSet<VesselImportDefValue> VesselImportDefValues { get; set; }
        public DbSet<VesselImportDefValueReadModel> VesselImportDefValueReadModels { get; set; }
        public DbSet<VesselImportTable> VesselImportTables { get; set; }
        public DbSet<VesselImportDocument> VesselImportDocuments { get; set; }


        public DbSet<VesselHandbookRecord> VesselsHandbook { get; set; }
        #endregion

        #region VesselExport

        public DbSet<VesselExportRecord> VesselExports { get; set; }
        public DbSet<VesselExportReadModel> VesselExportReadModels { get; set; }
        public DbSet<VesselExportFilingHeader> VesselExportFilingHeaders { get; set; }
        public DbSet<VesselExportSection> VesselExportSections { get; set; }
        public DbSet<VesselExportDefValue> VesselExportDefValues { get; set; }
        public DbSet<VesselExportDefValueReadModel> VesselExportDefValueReadModels { get; set; }
        public DbSet<VesselExportTable> VesselExportTables { get; set; }
        public DbSet<VesselExportDocument> VesselExportDocuments { get; set; }
        public DbSet<VesselExportRuleUsppiConsignee> VesselExportRuleUsppisConsignees { get; set; }

        #endregion

        #region App
        public DbSet<AppAdmin> AppAdmins { get; set; }
        public DbSet<AppUsersModel> AppUsersModels { get; set; }
        public DbSet<AppUsersStatusModel> AppUsersStatusModels { get; set; }
        public DbSet<AppPermission> AppPermissions { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUsersData> AppUsersData { get; set; }
        public DbSet<AppDocument> AppDocuments { get; set; }
        #endregion

        #region Audit

        #region Rail

        public DbSet<AuditRailTrainConsistSheet> AuditRailTrainConsistSheetSet { get; set; }

        #endregion
        #endregion



        #region Constructor
        static FilingPortalContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FilingPortalContext, Configuration>());
        }

        
        #endregion

        public override string DefaultSchema => "dbo";

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(typeof(CargoWiseContext).Assembly);
        }
    }
}
