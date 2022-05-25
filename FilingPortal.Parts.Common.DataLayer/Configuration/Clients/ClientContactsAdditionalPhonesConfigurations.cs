using System.Data.Entity.ModelConfiguration;
using FilingPortal.Parts.Common.Domain.Entities.Clients;

namespace FilingPortal.Parts.Common.DataLayer.Configuration.Clients
{
    /// <summary>
    /// Client contacts additional phones DB model configuration
    /// </summary>
    internal class ClientContactsAdditionalPhonesConfigurations : EntityTypeConfiguration<ClientContactAdditionalPhone>
    {
        public ClientContactsAdditionalPhonesConfigurations() : this("dbo") { }

        public ClientContactsAdditionalPhonesConfigurations(string schema)
        {
            ToTable("handbook_client_contacts_additional_phones", schema);

            HasKey(x => x.Id);

            Property(x => x.Phone).IsRequired();
            Property(x => x.ContactId).IsRequired();
            HasRequired(x => x.Contact).WithMany(x => x.AdditionalPhones).HasForeignKey(x => x.ContactId)
                .WillCascadeOnDelete(true);
        }
    }
}
