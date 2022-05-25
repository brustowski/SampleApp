using FilingPortal.Domain.Entities.Rail;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    /// <summary>
    /// Defines the Rail Inbound DB mapping configuration
    /// </summary>
    public class RailInboundReadModelConfiguration : EntityTypeConfiguration<RailInboundReadModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailInboundReadModelConfiguration"/> class.
        /// </summary>
        public RailInboundReadModelConfiguration() : this("dbo") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RailInboundReadModelConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema name</param>
        public RailInboundReadModelConfiguration(string schema)
        {
            ToTable("v_imp_rail_inbound_grid", schema);

            Property(x => x.Id).HasColumnName(@"BD_Parsed_Id");
            Property(x => x.ManifestRecordId).HasColumnName(@"BD_Parsed_EDIMessage_Id");
            Property(x => x.FilingHeaderId).HasColumnName(@"Filing_Headers_id").IsOptional();
            Property(x => x.BdParsedImporterConsignee).HasColumnName(@"DB_Parsed_ImporterConsignee").HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.BdParsedSupplier).HasColumnName(@"BD_Parsed_Supplier").HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.PortCode).HasColumnName(@"BD_Parsed_PortOfUnlading");
            Property(x => x.BdParsedDescription1).HasColumnName(@"BD_Parsed_Description1").HasColumnType("nvarchar").HasMaxLength(500);
            Property(x => x.BOLNumber).HasColumnName(@"BD_Parsed_BillofLading").HasColumnType("nvarchar");
            Property(x => x.IssuerCode).HasColumnName("BD_Parsed_Issuer_Code").HasMaxLength(5);
            Property(x => x.ContainerNumber).HasColumnName(@"BD_Parsed_Container_Number").HasColumnType("nvarchar");
            Property(x => x.TrainNumber).HasColumnName(@"BD_Parsed_ReferenceNumber1").HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.CreatedDate).HasColumnName(@"BD_Parsed_CreatedDate");
            Property(x => x.IsDeleted).HasColumnName(@"BD_Parsed_FDeleted");
            Property(x => x.IsDuplicated).HasColumnName(@"BD_Parsed_Is_Duplicated");
            Property(x => x.Importer).HasColumnName(@"Rule_ImporterSupplier_Importer");
            Property(x => x.Supplier).HasColumnName(@"Rule_ImporterSupplier_Main_Supplier");
            Property(x => x.HTS).HasColumnName(@"Rule_Desc1_Desc2_Tariff");
            Property(x => x.RulePortPort).HasColumnName(@"Rule_Port_Port");
            Property(x => x.FilingNumber).HasColumnName(@"Filing_Headers_FilingNumber").HasMaxLength(255);
            Property(x => x.JobLink).HasColumnName("Filing_Headers_JobLink").IsMaxLength();
            Property(x => x.MappingStatus).HasColumnName(@"Filing_Headers_MappingStatus").HasColumnType("int").IsOptional();
            Property(x => x.MappingStatusTitle).HasColumnName(@"Filing_Headers_MappingStatus_Title").HasMaxLength(20);
            Property(x => x.FilingStatus).HasColumnName(@"Filing_Headers_FilingStatus").HasColumnType("int").IsOptional();
            Property(x => x.FilingStatusTitle).HasColumnName(@"Filing_Headers_FilingStatus_Title").HasMaxLength(20);
            Property(x => x.Description).HasColumnName(@"Description");
            Property(x => x.Destination).HasMaxLength(2);
        }
    }
}
