using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.Clients
{
    /// <summary>
    /// Client contacts DB model configuration
    /// </summary>
    internal class ClientContactsConfiguration : EntityTypeConfiguration<ClientContact>
    {
        public ClientContactsConfiguration() : this("dbo") { }

        public ClientContactsConfiguration(string schema)
        {
            ToTable("handbook_cw_client_contacts", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ClientId).IsRequired();
            Property(x => x.ContactName).IsRequired();

            HasIndex(x => x.ClientId).HasName("Idx_ClientCode");
            HasIndex(x => x.ContactName).HasName("Idx_ContactName");

            HasRequired(x => x.Client).WithMany(x => x.Contacts).HasForeignKey(x => x.ClientId)
                .WillCascadeOnDelete(true);
        }
    }
}
