using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.Clients
{
    public class ClientManagementReadModelConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientManagementReadModelConfiguration() : this("dbo") { }

        public ClientManagementReadModelConfiguration(string schema)
        {
            ToTable("Clients", schema);

            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnType("uniqueidentifier").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ClientCode).HasColumnName(@"ClientCode").HasColumnType("nvarchar").HasMaxLength(12).IsRequired();
            Property(x => x.ClientName).HasColumnName(@"ClientName").HasColumnType("nvarchar").HasMaxLength(100);
            Property(x => x.LastUpdatedTime).HasColumnName("LastUpdatedTime");

            Ignore(x => x.ClientType);
        }
    }
}
