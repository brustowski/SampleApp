using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.Clients
{
    internal class ClientAddressConfiguration : EntityTypeConfiguration<ClientAddress>
    {
        public ClientAddressConfiguration() : this("dbo") { }

        public ClientAddressConfiguration(string schema)
        {
            ToTable("Client_Addresses", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ClientId).IsOptional();
            Property(x => x.Code).IsRequired();

            HasIndex(x => x.ClientId).HasName("Idx_ClientCode");
            HasIndex(x => x.Code).HasName("Idx_Code");
        }
    }
}
