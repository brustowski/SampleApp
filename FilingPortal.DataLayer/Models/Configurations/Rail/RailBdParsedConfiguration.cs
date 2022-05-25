using FilingPortal.Domain.Entities.Rail;
using System.Data.Entity.ModelConfiguration;

namespace FilingPortal.DataLayer.Models.Configurations.Rail
{
    /// <summary>
    /// Provides Model Configuration for <see cref="RailBdParsed"/>
    /// </summary>
    public class RailBdParsedConfiguration : EntityTypeConfiguration<RailBdParsed>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RailBdParsedConfiguration"/> class.
        /// </summary>
        public RailBdParsedConfiguration() : this("dbo")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RailBdParsedConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema<see cref="string"/></param>
        public RailBdParsedConfiguration(string schema)
        {
            ToTable("imp_rail_inbound", schema);

            HasIndex(x => x.DuplicateOf).HasName("Idx__duplicate_of");
            HasIndex(x => new { x.EquipmentInitial, x.EquipmentNumber, x.BillOfLading, x.PortOfUnlading, x.ReferenceNumber1 })
                .HasName("Idx__search_duplicates");

            Property(x => x.RailEdiMessageId).HasColumnName(@"broker_download_id");
            Property(x => x.Importer).HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.Supplier).HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.EquipmentInitial).HasMaxLength(4);
            Property(x => x.EquipmentNumber).HasColumnType("nvarchar").HasMaxLength(6);
            Property(x => x.IssuerCode).HasMaxLength(5);
            Property(x => x.BillOfLading).HasColumnType("nvarchar").HasMaxLength(20);
            Property(x => x.PortOfUnlading).HasMaxLength(4);
            Property(x => x.Description1).HasColumnType("nvarchar").HasMaxLength(500);
            Property(x => x.ManifestUnits).HasMaxLength(3);
            Property(x => x.Weight).HasColumnType("nvarchar").HasMaxLength(10);
            Property(x => x.WeightUnit).HasMaxLength(3);
            Property(x => x.ReferenceNumber1).HasColumnType("nvarchar").HasMaxLength(50);
            Property(x => x.Consignee).HasColumnType("nvarchar").HasMaxLength(200);
            Property(x => x.Destination).HasMaxLength(2);

            HasOptional(a => a.RailEdiMessage)
                .WithMany(b => b.RailBdParseds)
                .HasForeignKey(c => c.RailEdiMessageId)
                .WillCascadeOnDelete(false);
        }
    }
}
