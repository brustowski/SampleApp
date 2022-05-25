using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.Clients
{
    internal class ClientCodesConfiguration : EntityTypeConfiguration<ClientCode>
    {
        public ClientCodesConfiguration() : this("dbo") { }

        public ClientCodesConfiguration(string schema)
        {
            ToTable("client_code", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ClientId).IsOptional();
            Property(x => x.CodeType).HasMaxLength(3).IsRequired();
            Property(x => x.RegNumber).IsRequired();

            HasIndex(x => x.ClientId).HasName("Idx_ClientCode");
            HasIndex(x => x.CodeType).HasName("Idx_CodeType");
        }
    }
}
