using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Isf.Domain.Entities;

namespace FilingPortal.Parts.Isf.DataLayer.Configuration
{
    /// <summary>
    /// Provides Inbound Records entity type configuration
    /// </summary>
    public class InboundRecordsConfiguration : EntityTypeConfiguration<InboundRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        public InboundRecordsConfiguration()
            : this("isf")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InboundRecordsConfiguration"/> class.
        /// </summary>
        /// <param name="schema">The schema</param>
        public InboundRecordsConfiguration(string schema)
        {
            ToTable("inbound", schema);
            HasKey(x => x.Id);
            Property(x => x.ImporterId).IsRequired();
            
            HasRequired(x => x.Importer).WithMany().HasForeignKey(x => x.ImporterId).WillCascadeOnDelete(false);
            HasOptional(x => x.Buyer).WithMany().HasForeignKey(x => x.BuyerId).WillCascadeOnDelete(false);
            HasOptional(x => x.BuyerAppAddress).WithMany().HasForeignKey(x => x.BuyerAppAddressId).WillCascadeOnDelete(false);
            HasOptional(x => x.Consignee).WithMany().HasForeignKey(x => x.ConsigneeId).WillCascadeOnDelete(false);
            HasOptional(x => x.Seller).WithMany().HasForeignKey(x => x.SellerId).WillCascadeOnDelete(false);
            HasOptional(x => x.SellerAppAddress).WithMany().HasForeignKey(x => x.SellerAppAddressId).WillCascadeOnDelete(false);
            HasOptional(x => x.ContainerStuffingLocation).WithMany().HasForeignKey(x => x.ContainerStuffingLocationId).WillCascadeOnDelete(false);
            HasOptional(x => x.ContainerStuffingLocationAppAddress).WithMany().HasForeignKey(x => x.ContainerStuffingLocationAppAddressId).WillCascadeOnDelete(false);
            HasOptional(x => x.Consolidator).WithMany().HasForeignKey(x => x.ConsolidatorId).WillCascadeOnDelete(false);
            HasOptional(x => x.ConsolidatorAppAddress).WithMany().HasForeignKey(x => x.ConsolidatorAppAddressId).WillCascadeOnDelete(false);
            HasOptional(x => x.ShipTo).WithMany().HasForeignKey(x => x.ShipToId).WillCascadeOnDelete(false);
            HasOptional(x => x.ShipToAppAddress).WithMany().HasForeignKey(x => x.ShipToAppAddressId).WillCascadeOnDelete(false);
        }
    }
}
